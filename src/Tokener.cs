#region Copyright and License
//
// Fizzler - CSS Selector Engine for Microsoft .NET Framework
// Copyright (c) 2009 Atif Aziz, Colin Ramsay. All rights reserved.
//
// This library is free software; you can redistribute it and/or modify it under
// the terms of the GNU Lesser General Public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
// details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this library; if not, write to the Free Software Foundation, Inc.,
// 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
#endregion

namespace Fizzler
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    #endregion

    /// <summary>
    /// Lexer for tokens in CSS selector grammar.
    /// </summary>
    public static class Tokener
    {
        /// <summary>
        /// Parses tokens from a given text source.
        /// </summary>
        public static IEnumerable<Token> Tokenize(TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            return Tokenize(reader.ReadToEnd());
        }

        /// <summary>
        /// Parses tokens from a given string.
        /// </summary>
        public static IEnumerable<Token> Tokenize(string input)
        {
            var reader = new Reader(input ?? string.Empty);
            StringBuilder sb = null;

            while (reader.Read() is char ch)
            {
                //
                // Identifier or function
                //
                if (ch == '-' || IsNmStart(ch))
                {
                    reader.Mark();
                    if (ch == '-')
                    {
                        if (!(reader.Read() is char n) || !IsNmStart(n))
                            throw new FormatException(string.Format("Invalid identifier at position {0}.", reader.Position));
                    }

                    var r = reader.Read();
                    while (r is char nm && IsNmChar(nm))
                        r = reader.Read();

                    if (r == '(')
                        yield return Token.Function(reader.Marked());
                    else
                        yield return Token.Ident(reader.MarkedWithUnread());
                }
                //
                // Integer
                //
                else if (IsDigit(ch))
                {
                    reader.Mark();
                    do { /* NOP */ } while (reader.Read() is char d && IsDigit(d));
                    yield return Token.Integer(reader.MarkedWithUnread());
                }
                //
                // Whitespace, including that which is coupled with some punctuation
                //
                else if (IsS(ch))
                {
                    var space = ParseWhiteSpace(reader);
                    switch (reader.Read())
                    {
                        case ',': yield return Token.Comma(); break;
                        case '+': yield return Token.Plus(); break;
                        case '>': yield return Token.Greater(); break;
                        case '~': yield return Token.Tilde(); break;

                        default:
                            reader.Unread();
                            yield return Token.WhiteSpace(space);
                            break;
                    }
                }
                else switch(ch)
                {
                    case '*': // * or *=
                    case '~': // ~ or ~=
                    case '|': // | or |=
                    {
                        if (reader.Read() == '=')
                        {
                            yield return ch == '*'
                                       ? Token.SubstringMatch()
                                       : ch == '|' ? Token.DashMatch()
                                       : Token.Includes();
                        }
                        else
                        {
                            reader.Unread();
                            yield return ch == '*' || ch == '|'
                                ? Token.Char(ch)
                                : Token.Tilde();
                        }
                        break;
                    }
                    case '^': // ^=
                    case '$': // $=
                    {
                        if (reader.Read() != '=')
                            throw new FormatException(string.Format("Invalid character at position {0}.", reader.Position));

                        switch (ch)
                        {
                            case '^': yield return Token.PrefixMatch(); break;
                            case '$': yield return Token.SuffixMatch(); break;
                        }
                        break;
                    }
                    //
                    // Single-character punctuation
                    //
                    case '.': yield return Token.Dot(); break;
                    case ':':
                    {
                        var pos = reader.Position;
                        if (reader.Read() == 'n'
                            && reader.Read() == 'o'
                            && reader.Read() == 't'
                            && reader.Read() == '(')
                        {
                            yield return Token.Not();
                            break;
                        }

                        while (reader.Position > pos)
                            reader.Unread();

                        yield return Token.Colon();
                        break;
                    }
                    case ',':  yield return Token.Comma(); break;
                    case '=':  yield return Token.Equals(); break;
                    case '[':  yield return Token.LeftBracket(); break;
                    case ']':  yield return Token.RightBracket(); break;
                    case ')':  yield return Token.RightParenthesis(); break;
                    case '+': yield return Token.Plus(); break;
                    case '>':  yield return Token.Greater(); break;
                    case '#':  yield return Token.Hash(ParseHash(reader)); break;
                    //
                    // Single- or double-quoted strings
                    //
                    case '\"':
                    case '\'': yield return ParseString(reader, /* quote */ ch, ref sb); break;

                    default:
                        throw new FormatException(string.Format("Invalid character at position {0}.", reader.Position));
                }
            }
            yield return Token.Eoi();
        }

        static string ParseWhiteSpace(Reader reader)
        {
            Debug.Assert(reader != null);

            reader.Mark();
            while (reader.Read() is char ch && IsS(ch)) { /* NOP */ }
            return reader.MarkedWithUnread();
        }

        static string ParseHash(Reader reader)
        {
            Debug.Assert(reader != null);

            reader.MarkFromNext(); // skipping #
            while (reader.Read() is char ch && IsNmChar(ch)) { /* NOP */ }
            var text = reader.MarkedWithUnread();
            if (text.Length == 0)
                throw new FormatException(string.Format("Invalid hash at position {0}.", reader.Position));
            return text;
        }

        static Token ParseString(Reader reader, char quote, ref StringBuilder sb)
        {
            Debug.Assert(reader != null);

            //
            // TODO Support full string syntax!
            //
            // string    {string1}|{string2}
            // string1   \"([^\n\r\f\\"]|\\{nl}|{nonascii}|{escape})*\"
            // string2   \'([^\n\r\f\\']|\\{nl}|{nonascii}|{escape})*\'
            // nonascii  [^\0-\177]
            // escape    {unicode}|\\[^\n\r\f0-9a-f]
            // unicode   \\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?
            //

            var strpos = reader.Position;
            reader.MarkFromNext(); // skipping quote

            sb?.Clear();

            for (var done = false; !done;)
            {
                switch (reader.Read())
                {
                    case null:
                        throw new FormatException(string.Format("Unterminated string at position {0}.", strpos));

                    case char ch when ch == quote:
                        done = true;
                        break;

                    case char ch when ch == '\\':
                    {
                        //
                        // NOTE: Only escaping of quote and backslash supported!
                        //

                        var esc = reader.Read();
                        if (esc != quote && esc != '\\')
                            throw new FormatException(string.Format("Invalid escape sequence at position {0} in a string at position {1}.", reader.Position, strpos));

                        if (sb == null)
                            sb = new StringBuilder();

                        sb.Append(reader.MarkedExceptLast());
                        reader.Mark();
                        break;
                    }
                }
            }

            var text = reader.Marked();

            if (sb != null)
                text = sb.Append(text).ToString();

            return Token.String(text);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsDigit(char ch) => // [0-9]
            ch >= '0' && ch <= '9';

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsS(char ch) => // [ \t\r\n\f]
            ch == ' ' || ch == '\t' || ch == '\r' || ch == '\n' || ch == '\f';

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsNmStart(char ch) // [_a-z]|{nonascii}|{escape}
            => ch == '_'
            || (ch >= 'a' && ch <= 'z')
            || (ch >= 'A' && ch <= 'Z');

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsNmChar(char ch) => // [_a-z0-9-]|{nonascii}|{escape}
            IsNmStart(ch) || ch == '-' || (ch >= '0' && ch <= '9');

        sealed class Reader
        {
            readonly string _input;
            int _index = -1;
            int _start = -1;

            public Reader(string input) => _input = input;

            public int Position => _index + 1;

            public void Mark() => _start = _index;
            public void MarkFromNext() => _start = _index + 1;
            public string Marked() => Marked(0);
            public string MarkedExceptLast() => Marked(-1);

            string Marked(int trim)
            {
                var start = _start;
                var count = Math.Min(_input.Length, _index + trim) - start;
                return count > 0
                     ? _input.Substring(start, count)
                     : string.Empty;
            }

            public char? Read()
            {
                var input = _input;

                var i = _index = Position >= input.Length
                               ? input.Length
                               : _index + 1;

                return i >= 0 && i < input.Length ? input[i] : (char?) null;
            }

            public void Unread() => _index = Math.Max(-1, _index - 1);

            public string MarkedWithUnread()
            {
                var text = Marked();
                Unread();
                return text;
            }
        }
    }
}
