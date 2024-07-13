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
    using System.Globalization;
    using System.Linq;

    using TokenSpec = Either<TokenKind, Token>;

    #endregion

    /// <summary>
    /// Semantic parser for CSS selector grammar.
    /// </summary>
    public sealed class Parser
    {
        readonly Reader<Token> _reader;
        readonly ISelectorGenerator _generator;

        Parser(Reader<Token> reader, ISelectorGenerator generator)
        {
            _reader = reader;
            _generator = generator;
        }

        /// <summary>
        /// Parses a CSS selector group and generates its implementation.
        /// </summary>
        public static TGenerator Parse<TGenerator>(string selectors, TGenerator generator)
            where TGenerator : ISelectorGenerator =>
            Parse(selectors, generator, g => g);

        /// <summary>
        /// Parses a CSS selector group and generates its implementation.
        /// </summary>
        public static T Parse<TGenerator, T>(string selectors, TGenerator generator, Func<TGenerator, T> resultor)
            where TGenerator : ISelectorGenerator
        {
            if (selectors == null) throw new ArgumentNullException(nameof(selectors));
            if (selectors.Length == 0) throw new ArgumentException(null, nameof(selectors));

            return Parse(Tokener.Tokenize(selectors), generator, resultor);
        }

        /// <summary>
        /// Parses a tokenized stream representing a CSS selector group and
        /// generates its implementation.
        /// </summary>
        public static TGenerator Parse<TGenerator>(IEnumerable<Token> tokens, TGenerator generator)
            where TGenerator : ISelectorGenerator =>
            Parse(tokens, generator, g => g);

        /// <summary>
        /// Parses a tokenized stream representing a CSS selector group and
        /// generates its implementation.
        /// </summary>
        public static T Parse<TGenerator, T>(IEnumerable<Token> tokens, TGenerator generator, Func<TGenerator, T> resultor)
            where TGenerator : ISelectorGenerator
        {
            if (tokens == null) throw new ArgumentNullException(nameof(tokens));
            if (resultor == null) throw new ArgumentNullException(nameof(resultor));

            new Parser(new Reader<Token>(tokens.GetEnumerator()), generator).Parse();
            return resultor(generator);
        }

        static partial class TokenSpecs { }

        void Parse()
        {
            _generator.OnInit();
            SelectorGroup();
            _generator.OnClose();
        }

        void SelectorGroup()
        {
            //selectors_group
            //  : selector [ COMMA S* selector ]*
            //  ;

            Selector();
            while (TryRead(ToTokenSpec(Token.Comma())) != null)
            {
                TryRead(ToTokenSpec(TokenKind.WhiteSpace));
                Selector();
            }

            Read(ToTokenSpec(TokenKind.Eoi));
        }

        void Selector()
        {
            _generator.OnSelector();

            //selector
            //  : simple_selector_sequence [ combinator simple_selector_sequence ]*
            //  ;

            SimpleSelectorSequence();
            while (TryCombinator())
                SimpleSelectorSequence();
        }

        partial class TokenSpecs // ReSharper disable once InconsistentNaming
        {
            public static readonly TokenSpec[] Plus_Greater_Tilde_WhiteSpace =
            {
                ToTokenSpec(TokenKind.Plus),
                ToTokenSpec(TokenKind.Greater),
                ToTokenSpec(TokenKind.Tilde),
                ToTokenSpec(TokenKind.WhiteSpace)
            };
        }

        bool TryCombinator()
        {
            //combinator
            //  /* combinators can be surrounded by whitespace */
            //  : PLUS S* | GREATER S* | TILDE S* | S+
            //  ;

            if (TryRead(TokenSpecs.Plus_Greater_Tilde_WhiteSpace) is not { } token)
                return false;

            if (token.Kind == TokenKind.WhiteSpace)
            {
                _generator.Descendant();
            }
            else
            {
                switch (token.Kind)
                {
                    case TokenKind.Tilde: _generator.GeneralSibling(); break;
                    case TokenKind.Greater: _generator.Child(); break;
                    case TokenKind.Plus: _generator.Adjacent(); break;
                }

                TryRead(ToTokenSpec(TokenKind.WhiteSpace));
            }

            return true;
        }

        partial class TokenSpecs // ReSharper disable once InconsistentNaming
        {
            public static readonly TokenSpec[] Hash_Dot_LeftBracket_Colon =
            {
                ToTokenSpec(TokenKind.Hash),
                ToTokenSpec(Token.Dot()),
                ToTokenSpec(Token.LeftBracket()),
                ToTokenSpec(Token.Colon())
            };

            public static readonly TokenSpec[] Hash_Dot_LeftBracket_Colon_Not =
            {
                ToTokenSpec(TokenKind.Hash),
                ToTokenSpec(Token.Dot()),
                ToTokenSpec(Token.LeftBracket()),
                ToTokenSpec(Token.Colon()),
                ToTokenSpec(Token.Not())
            };
        }

        void SimpleSelectorSequence()
        {
            //simple_selector_sequence
            //  : [ type_selector | universal ]
            //    [ HASH | class | attrib | pseudo | negation ]*
            //  | [ HASH | class | attrib | pseudo | negation ]+
            //  ;

            var named = false;
            for (var modifiers = 0; ; modifiers++)
            {
                if (TryRead(TokenSpecs.Hash_Dot_LeftBracket_Colon_Not) is { } token)
                {
                    if (modifiers == 0 && !named)
                        _generator.Universal(NamespacePrefix.None); // implied

                    switch (token.Kind)
                    {
                        case TokenKind.Not:
                        {
                            Unread(token);
                            Negation();
                            break;
                        }
                        case TokenKind.Hash:
                        {
                            _generator.Id(token.SomeText);
                            break;
                        }
                        default:
                        {
                            Unread(token);
                            switch (token.Text)
                            {
                                case ['.']: Class(); break;
                                case ['[']: Attrib(); break;
                                case [':']: Pseudo(); break;
                                default: throw new Exception("Internal error.");
                            }
                            break;
                        }
                    }
                }
                else
                {
                    if (named || modifiers > 0)
                        break;
                    TypeSelectorOrUniversal();
                    named = true;
                }
            }
        }

        void Negation()
        {
            //negation
            //  : NOT S* negation_arg S* ')'
            //  ;

            Read(ToTokenSpec(TokenKind.Not));
            TryRead(ToTokenSpec(TokenKind.WhiteSpace));
            var generator = _generator as INegationSelectorGenerator;
            if (generator == null)
                throw new NotSupportedException("Negation pseudo-class is not supported.");

            generator.BeginNegation();

            //negation_arg
            //  : type_selector | universal | HASH | class | attrib | pseudo
            //  ;

            if (TryRead(TokenSpecs.Hash_Dot_LeftBracket_Colon) is { } token)
            {
                _generator.Universal(NamespacePrefix.None); // implied

                if (token.Kind == TokenKind.Hash)
                {
                    _generator.Id(token.SomeText);
                }
                else
                {
                    Unread(token);
                    switch (token.Text)
                    {
                        case ['.']: Class(); break;
                        case ['[']: Attrib(); break;
                        case [':']: Pseudo(); break;
                        default: throw new Exception("Internal error.");
                    }
                }
            }
            else
            {
                TypeSelectorOrUniversal();
            }

            TryRead(ToTokenSpec(TokenKind.WhiteSpace));
            Read(ToTokenSpec(Token.RightParenthesis()));

            generator.EndNegation();
        }

        void Pseudo()
        {
            //pseudo
            //  /* '::' starts a pseudo-element, ':' a pseudo-class */
            //  /* Exceptions: :first-line, :first-letter, :before and :after. */
            //  /* Note that pseudo-elements are restricted to one per selector and */
            //  /* occur only in the last simple_selector_sequence. */
            //  : ':' ':'? [ IDENT | functional_pseudo ]
            //  ;

            PseudoClass(); // We do pseudo-class only for now
        }

        void PseudoClass()
        {
            //pseudo
            //  : ':' [ IDENT | functional_pseudo ]
            //  ;

            Read(ToTokenSpec(Token.Colon()));
            if (!TryFunctionalPseudo())
            {
                var clazz = Read(ToTokenSpec(TokenKind.Ident)).Text;
                switch (clazz)
                {
                    case "first-child": _generator.FirstChild(); break;
                    case "last-child": _generator.LastChild(); break;
                    case "only-child": _generator.OnlyChild(); break;
                    case "empty": _generator.Empty(); break;
                    default:
                        {
                            throw new FormatException(string.Format(
                                "Unknown pseudo-class '{0}'. Use either first-child, last-child, only-child or empty.", clazz));
                        }
                }
            }
        }

        bool TryFunctionalPseudo()
        {
            //functional_pseudo
            //  : FUNCTION S* expression ')'
            //  ;

            if (TryRead(ToTokenSpec(TokenKind.Function)) is not { } token)
                return false;

            TryRead(ToTokenSpec(TokenKind.WhiteSpace));

            switch (token.Text)
            {
                case "nth-child": Nth(); break;
                case "nth-last-child": NthLast(); break;
                case var func:
                    {
                        throw new FormatException(string.Format(
                            "Unknown functional pseudo '{0}'. Only nth-child and nth-last-child are supported.", func));
                    }
            }

            Read(ToTokenSpec(Token.RightParenthesis()));
            return true;
        }

        void Nth()
        {
            //nth
            //  : S* [ ['-'|'+']? INTEGER? {N} [ S* ['-'|'+'] S* INTEGER ]? |
            //         ['-'|'+']? INTEGER | {O}{D}{D} | {E}{V}{E}{N} ] S*
            //  ;

            // TODO Add support for the full syntax
            // At present, only INTEGER is allowed

            _generator.NthChild(1, NthB());
        }

        void NthLast()
        {
            //nth
            //  : S* [ ['-'|'+']? INTEGER? {N} [ S* ['-'|'+'] S* INTEGER ]? |
            //         ['-'|'+']? INTEGER | {O}{D}{D} | {E}{V}{E}{N} ] S*
            //  ;

            // TODO Add support for the full syntax
            // At present, only INTEGER is allowed

            _generator.NthLastChild(1, NthB());
        }

        int NthB()
        {
            return int.Parse(Read(ToTokenSpec(TokenKind.Integer)).Text, CultureInfo.InvariantCulture);
        }

        partial class TokenSpecs
        {
            // ReSharper disable once InconsistentNaming
            public static readonly TokenSpec[] Equals_Includes_DashMatch_PrefixMatch_SuffixMatch_SubstringMatch =
            {
                ToTokenSpec(Token.Equals()),
                ToTokenSpec(TokenKind.Includes),
                ToTokenSpec(TokenKind.DashMatch),
                ToTokenSpec(TokenKind.PrefixMatch),
                ToTokenSpec(TokenKind.SuffixMatch),
                ToTokenSpec(TokenKind.SubstringMatch)
            };

            // ReSharper disable once InconsistentNaming
            public static readonly TokenSpec[] String_Ident =
            {
                ToTokenSpec(TokenKind.String),
                ToTokenSpec(TokenKind.Ident)
            };
        }

        void Attrib()
        {
            //attrib
            //  : '[' S* [ namespace_prefix ]? IDENT S*
            //        [ [ PREFIXMATCH |
            //            SUFFIXMATCH |
            //            SUBSTRINGMATCH |
            //            '=' |
            //            INCLUDES |
            //            DASHMATCH ] S* [ IDENT | STRING ] S*
            //        ]? ']'
            //  ;

            Read(ToTokenSpec(Token.LeftBracket()));
            var prefix = TryNamespacePrefix() ?? NamespacePrefix.None;
            var name = Read(ToTokenSpec(TokenKind.Ident)).SomeText;

            var hasValue = false;
            while (true)
            {
                if (TryRead(TokenSpecs.Equals_Includes_DashMatch_PrefixMatch_SuffixMatch_SubstringMatch) is { } op)
                {
                    hasValue = true;
                    var value = Read(TokenSpecs.String_Ident).SomeText;

                    if (op == Token.Equals())
                    {
                        _generator.AttributeExact(prefix, name, value);
                    }
                    else
                    {
                        switch (op.Kind)
                        {
                            case TokenKind.Includes: _generator.AttributeIncludes(prefix, name, value); break;
                            case TokenKind.DashMatch: _generator.AttributeDashMatch(prefix, name, value); break;
                            case TokenKind.PrefixMatch: _generator.AttributePrefixMatch(prefix, name, value); break;
                            case TokenKind.SuffixMatch: _generator.AttributeSuffixMatch(prefix, name, value); break;
                            case TokenKind.SubstringMatch: _generator.AttributeSubstring(prefix, name, value); break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            if (!hasValue)
                _generator.AttributeExists(prefix, name);

            Read(ToTokenSpec(Token.RightBracket()));
        }

        void Class()
        {
            //class
            //  : '.' IDENT
            //  ;

            Read(ToTokenSpec(Token.Dot()));
            _generator.Class(Read(ToTokenSpec(TokenKind.Ident)).SomeText);
        }

        partial class TokenSpecs // ReSharper disable once InconsistentNaming
        {
            public static readonly TokenSpec[] Ident_Star_Pipe =
            {
                ToTokenSpec(TokenKind.Ident),
                ToTokenSpec(Token.Star()),
                ToTokenSpec(Token.Pipe())
            };
        }

        NamespacePrefix? TryNamespacePrefix()
        {
            //namespace_prefix
            //  : [ IDENT | '*' ]? '|'
            //  ;

            var pipe = Token.Pipe();
            if (TryRead(TokenSpecs.Ident_Star_Pipe) is not { } token)
                return null;

            if (token == pipe)
                return NamespacePrefix.Empty;

            var prefix = token;
            if (TryRead(ToTokenSpec(pipe)) == null)
            {
                Unread(prefix);
                return null;
            }

            return prefix.Kind == TokenKind.Ident
                 ? new NamespacePrefix(prefix.Text)
                 : NamespacePrefix.Any;
        }

        partial class TokenSpecs // ReSharper disable once InconsistentNaming
        {
            public static readonly TokenSpec[] Ident_Star =
            {
                ToTokenSpec(TokenKind.Ident),
                ToTokenSpec(Token.Star())
            };
        }

        void TypeSelectorOrUniversal()
        {
            //type_selector
            //  : [ namespace_prefix ]? element_name
            //  ;
            //element_name
            //  : IDENT
            //  ;
            //universal
            //  : [ namespace_prefix ]? '*'
            //  ;

            var prefix = TryNamespacePrefix() ?? NamespacePrefix.None;
            var token = Read(TokenSpecs.Ident_Star);
            if (token.Kind == TokenKind.Ident)
                _generator.Type(prefix, token.SomeText);
            else
                _generator.Universal(prefix);
        }

        Token Peek() => _reader.Peek();

        Token Read(TokenSpec spec) =>
            TryRead(spec)
            ?? throw new FormatException(string.Format(
                    @"Unexpected token {{{0}}} where {{{1}}} was expected.",
                    Peek().Kind, spec));

        Token Read(params TokenSpec[] specs) =>
            TryRead(specs)
            ?? throw new FormatException(string.Format(
                   @"Unexpected token {{{0}}} where one of [{1}] was expected.",
                   Peek().Kind,
                   string.Join(", ", from k in specs select k.ToString())));

        Token? TryRead(params TokenSpec[] specs)
        {
            foreach (var kind in specs)
            {
                var token = TryRead(kind);
                if (token != null)
                    return token;
            }
            return null;
        }

        Token? TryRead(TokenSpec spec)
        {
            var token = Peek();
            if (!spec.Fold(token, (t, a) => a == t.Kind, (t, b) => b == t))
                return null;
            _reader.Read();
            return token;
        }

        void Unread(Token token) => _reader.Unread(token);

        static TokenSpec ToTokenSpec(TokenKind kind) => TokenSpec.A(kind);
        static TokenSpec ToTokenSpec(Token token) => TokenSpec.B(token);
    }
}
