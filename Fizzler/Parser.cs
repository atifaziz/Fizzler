using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace Fizzler
{
    /// <summary>
    /// Semantic parser for CSS selector grammar.
    /// </summary>
    public sealed class Parser
    {
        private readonly Reader<Token> _reader;
        private readonly ISelectorGenerator _generator;
        
        private Parser(Reader<Token> reader, ISelectorGenerator generator)
        {
            Debug.Assert(reader != null);
            Debug.Assert(generator != null);
            _reader = reader;
            _generator = generator;
        }

        /// <summary>
        /// Parses a CSS selector group and generates its implementation.
        /// </summary>
        public static void Parse(string selectors, ISelectorGenerator generator)
        {
            if (selectors == null) throw new ArgumentNullException("selectors");
            if (selectors.Length == 0) throw new ArgumentException(null, "selectors");
            Parse(Tokener.Tokenize(selectors), generator);
        }

        /// <summary>
        /// Parses a tokenized stream representing a CSS selector group and 
        /// generates its implementation.
        /// </summary>
        public static void Parse(IEnumerable<Token> tokens, ISelectorGenerator generator)
        {
            if (tokens == null) throw new ArgumentNullException("tokens");
            new Parser(new Reader<Token>(tokens.GetEnumerator()), generator).Parse();
        }

        private void Parse()
        {
            _generator.OnInit();
            SelectorGroup();
            _generator.OnClose();
        }

        private void SelectorGroup()
        {
            //selectors_group
            //  : selector [ COMMA S* selector ]*
            //  ;

            Selector();
            while (TryRead(TokenKind.Comma) != null)
            {
                TryRead(TokenKind.WhiteSpace);
                Selector();
            }

            Read(TokenKind.Eoi);
        }

        private void Selector()
        {
            _generator.OnSelector();

            //selector
            //  : simple_selector [ combinator simple_selector ]*
            //  ;

            SimpleSelector();
            while (TryCombinator())
                SimpleSelector();
        }

        private bool TryCombinator()
        {
            //combinator
            //  : PLUS S*
            //  | GREATER S*
            //  | S
            //  ;

            var token = TryRead(TokenKind.Plus, TokenKind.Greater, TokenKind.WhiteSpace);
            
            if (token == null)
                return false;

            if (token.Value.Kind == TokenKind.WhiteSpace)
            {
                _generator.Descendant();
            }
            else
            {
                switch (token.Value.Kind)
                {
                    case TokenKind.Greater: _generator.Child(); break;
                    case TokenKind.Plus: _generator.Adjacent(); break;
                }

                TryRead(TokenKind.WhiteSpace);
            }
            
            return true;
        }

        private void SimpleSelector()
        {
            //simple_selector
            //  : element_name [ HASH | class | attrib | pseudo ]*
            //  | [ HASH | class | attrib | pseudo ]+
            //  ;

            var named = false;
            for (var modifiers = 0; ; modifiers++)
            {
                var token = TryRead(TokenKind.Hash, TokenKind.Dot, TokenKind.LeftBracket, TokenKind.Colon);

                if (token == null)
                {
                    if (named || modifiers > 0)
                        break;
                    ElementName();
                    named = true;
                }
                else if (token.Value.Kind == TokenKind.Hash)
                {
                    _generator.Id(token.Value.Text);
                }
                else
                {
                    if (modifiers == 0 && !named) 
                        _generator.Universal(); // implied
                    Unread(token.Value);
                    switch (token.Value.Kind)
                    {
                        case TokenKind.Dot: Class(); break;
                        case TokenKind.LeftBracket: Attrib(); break;
                        case TokenKind.Colon: Pseudo(); break;
                    }
                }
            }
        }

        private void Pseudo()
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

        private void PseudoClass()
        {
            //pseudo
            //  : ':' [ IDENT | functional_pseudo ]
            //  ;

            Read(TokenKind.Colon);
            if (!TryFunctionalPseudo())
            {
                var clazz = Read(TokenKind.Ident).Text;
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

        private bool TryFunctionalPseudo()
        {
            //functional_pseudo
            //  : FUNCTION S* expression ')'
            //  ;

            var token = TryRead(TokenKind.Function);
            if (token == null)
                return false;

            TryRead(TokenKind.WhiteSpace);

            var func = token.Value.Text;
            switch (func)
            {
                case "nth-child": Nth(); break;
                default:
                {
                    throw new FormatException(string.Format(
                        "Unknown functional pseudo '{0}'. Only nth-child is supported.", func));
                }
            }

            Read(TokenKind.RightParenthesis);
            return true;
        }

        private void Nth()
        {
            //nth
            //  : S* [ ['-'|'+']? INTEGER? {N} [ S* ['-'|'+'] S* INTEGER ]? |
            //         ['-'|'+']? INTEGER | {O}{D}{D} | {E}{V}{E}{N} ] S*
            //  ;

            // TODO Add support for the full syntax
            // At present, only INTEGER is allowed

            var pos = int.Parse(Read(TokenKind.Integer).Text, CultureInfo.InvariantCulture);
            _generator.NthChild(pos);
        }

        private void Attrib()
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

            Read(TokenKind.LeftBracket);
            var name = Read(TokenKind.Ident).Text;
            
            var hasValue = false;
            while (true)
            {
                var op = TryRead(
                    TokenKind.Equals, 
                    TokenKind.Includes, 
                    TokenKind.DashMatch, 
                    TokenKind.PrefixMatch, 
                    TokenKind.SuffixMatch,
                    TokenKind.SubstringMatch);
                
                if(op == null) 
                    break;
                
                hasValue = true;
                var value = Read(TokenKind.String, TokenKind.Ident).Text;
                
                switch (op.Value.Kind)
                {
                    case TokenKind.Equals: _generator.AttributeExact(name, value); break;
                    case TokenKind.Includes: _generator.AttributeIncludes(name, value); break;
                    case TokenKind.DashMatch: _generator.AttributeDashMatch(name, value); break;
                    case TokenKind.PrefixMatch: _generator.AttributePrefixMatch(name, value); break;
                    case TokenKind.SuffixMatch: _generator.AttributeSuffixMatch(name, value); break;
                    case TokenKind.SubstringMatch: _generator.AttributeSubstring(name, value); break;
                }
            }
            
            if (!hasValue)
                _generator.AttributeExists(name);
            
            Read(TokenKind.RightBracket);
        }

        private void Class()
        {
            //class
            //  : '.' IDENT
            //  ;
            
            Read(TokenKind.Dot);
            _generator.Class(Read(TokenKind.Ident).Text);
        }

        private void ElementName()
        {
            //element_name
            //  : IDENT | '*'
            //  ;

            var token = Read(TokenKind.Ident, TokenKind.Star);
            if (token.Kind == TokenKind.Ident)
                _generator.Type(token.Text);
            else
                _generator.Universal();
        }

        private Token Peek()
        {
            return _reader.Peek();
        }

        private Token Read(TokenKind kind)
        {
            var token = TryRead(kind);
            if (token == null)
            {
                throw new FormatException(
                    string.Format(@"Unexpected token {{{0}}} where {{{1}}} was expected.",
                    Peek().Kind, kind));
            }
            return token.Value;
        }

        private Token Read(params TokenKind[] kinds)
        {
            var token = TryRead(kinds);
            if (token == null)
            {
                throw new FormatException(string.Format(
                    @"Unexpected token {{{0}}} where one of [{1}] was expected.", 
                    Peek().Kind, string.Join(", ", kinds.Select(k => k.ToString()).ToArray())));
            }
            return token.Value;
        }

        private Token? TryRead(params TokenKind[] kinds)
        {
            foreach (var kind in kinds)
            {
                var token = TryRead(kind);
                if (token != null)
                    return token;
            }
            return null;
        }

        private Token? TryRead(TokenKind kind)
        {
            var token = Peek();
            if (token.Kind != kind)
                return null;
            _reader.Read();
            return token;
        }

        private void Unread(Token token)
        {
            _reader.Unread(token);
        }
    }
}
