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
        public static IEnumerable<Token> Tokenize(string? input)
        {
            var reader = new Reader(input ?? string.Empty);
            StringBuilder? sb = null;

            while (reader.Read() is { } ch)
            {
                //
                // Identifier or function
                //
                if (ch == '-' || IsNmStart(ch))
                {
                    reader.Mark();
                    if (ch == '-')
                    {
                        if (reader.Read() is not { } n || !IsNmStart(n))
                            throw new FormatException($"Invalid identifier at position {reader.Position}.");
                    }

                    var r = reader.Read();
                    while (r is { } nm && IsNmChar(nm))
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
                    do { /* NOP */ } while (reader.Read() is { } d && IsDigit(d));
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
#pragma warning disable IDE0011 // Add braces (save indentation for readability)
                else switch(ch)
#pragma warning restore IDE0011 // Add braces
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
                            yield return ch is '*' or '|'
                                ? Token.Char(ch)
                                : Token.Tilde();
                        }
                        break;
                    }
                    case '^': // ^=
                    case '$': // $=
                    {
                        if (reader.Read() != '=')
                            throw new FormatException($"Invalid character at position {reader.Position}.");

#pragma warning disable IDE0010 // Add missing cases (handled by compiler)
                        switch (ch)
#pragma warning restore IDE0010 // Add missing cases
                        {
                            case '^': yield return Token.PrefixMatch(); break;
                            case '$': yield return Token.SuffixMatch(); break;
                        }
                        break;
                    }
                    //
                    // Single-character punctuation (mostly)
                    //
                    case '.': yield return Token.Dot(); break;
                    case ',':  yield return Token.Comma(); break;
                    case '=':  yield return Token.Equals(); break;
                    case '[':  yield return Token.LeftBracket(); break;
                    case ']':  yield return Token.RightBracket(); break;
                    case ')':  yield return Token.RightParenthesis(); break;
                    case '+': yield return Token.Plus(); break;
                    case '>':  yield return Token.Greater(); break;
                    case '#':  yield return Token.Hash(ParseHash(reader)); break;
                    case ':':
                    {
                        var pos = reader.Position;
#pragma warning disable IDE0078 // Use pattern matching (may change code meaning)
                        if (reader.Read() == 'n' &&
                            reader.Read() == 'o' &&
                            reader.Read() == 't' &&
                            reader.Read() == '(')
#pragma warning restore IDE0078 // Use pattern matching
                        {
                            yield return Token.Not(); // ":"{N}{O}{T}"("  return NOT;
                            break;
                        }

                        while (reader.Position > pos)
                            reader.Unread();

                        yield return Token.Colon();
                        break;
                    }
                    //
                    // Single- or double-quoted strings
                    //
                    case '\"':
                    case '\'': yield return ParseString(reader, /* quote */ ch, ref sb); break;

                    default:
                        throw new FormatException($"Invalid character at position {reader.Position}.");
                }
            }
            yield return Token.Eoi();
        }

        static string ParseWhiteSpace(Reader reader)
        {
            reader.Mark();
            while (reader.Read() is { } ch && IsS(ch)) { /* NOP */ }
            return reader.MarkedWithUnread();
        }

        static string ParseHash(Reader reader)
        {
            reader.MarkFromNext(); // skipping #
            while (reader.Read() is { } ch && IsNmChar(ch)) { /* NOP */ }
            var text = reader.MarkedWithUnread();
            if (text.Length == 0)
                throw new FormatException($"Invalid hash at position {reader.Position}.");
            return text;
        }

        static Token ParseString(Reader reader, char quote, ref StringBuilder? sb)
        {
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

            _ = sb?.Clear();

            for (var done = false; !done;)
            {
#pragma warning disable IDE0010 // Add missing cases (handled by compiler)
                switch (reader.Read())
#pragma warning restore IDE0010 // Add missing cases
                {
                    case null:
                        throw new FormatException($"Unterminated string at position {strpos}.");

                    case { } ch when ch == quote:
                        done = true;
                        break;

                    case '\\':
                    {
                        //
                        // NOTE: Only escaping of quote and backslash supported!
                        //

                        var esc = reader.Read();
                        if (esc != quote && esc != '\\')
                            throw new FormatException($"Invalid escape sequence at position {reader.Position} in a string at position {strpos}.");

                        sb ??= new StringBuilder();
                        _ = sb.Append(reader.MarkedExceptLast());
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
            ch is >= '0' and <= '9';

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsS(char ch) => // [ \t\r\n\f]
            ch is ' ' or '\t' or '\r' or '\n' or '\f';

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsNmStart(char ch) => // [_a-z]|{nonascii}|{escape}
            ch is '_' or >= 'a' and <= 'z' or >= 'A' and <= 'Z';

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsNmChar(char ch) => // [_a-z0-9-]|{nonascii}|{escape}
            IsNmStart(ch) || ch is '-' or >= '0' and <= '9';

        sealed class Reader(string input)
        {
            readonly string input = input;
            int index = -1;
            int start = -1;

            public int Position => this.index + 1;

            public void Mark() => this.start = this.index;
            public void MarkFromNext() => this.start = this.index + 1;
            public string Marked() => Marked(0);
            public string MarkedExceptLast() => Marked(-1);

            string Marked(int trim)
            {
                var start = this.start;
                return Math.Min(this.input.Length, this.index + trim) - start is var count and > 0
                     ? this.input.Substring(start, count)
                     : string.Empty;
            }

            public char? Read()
            {
                var input = this.input;

                var i = this.index = Position >= input.Length
                                   ? input.Length
                                   : this.index + 1;

                return i >= 0 && i < input.Length ? input[i] : null;
            }

            public void Unread() => this.index = Math.Max(-1, this.index - 1);

            public string MarkedWithUnread()
            {
                var text = Marked();
                Unread();
                return text;
            }
        }
    }
}
