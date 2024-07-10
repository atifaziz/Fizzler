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

namespace Fizzler.Tests
{
    #region Imports

    using System;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class TokenerTests
    {
        [Test]
        public void NullInput()
        {
            Assert.That(Tokener.Tokenize((string) null).Single(), Is.EqualTo(Token.Eoi()));
        }

        [Test]
        public void EmptyInput()
        {
            Assert.That(Tokener.Tokenize(string.Empty).Single(), Is.EqualTo(Token.Eoi()));
        }

        [TestCase(" \r \n \f \t ", " \r \n \f \t ")]
        [TestCase(" \r \n \f \t ", " \r \n \f \t etc")]
        public void WhiteSpace(string ws, string input)
        {
            Assert.That(Tokener.Tokenize(input).First(), Is.EqualTo(Token.WhiteSpace(ws)));
        }

        [Test]
        public void Colon()
        {
            Assert.That(Tokener.Tokenize(":").First(), Is.EqualTo(Token.Colon()));
        }

        [Test]
        public void Comma()
        {
            Assert.That(Tokener.Tokenize(",").First(), Is.EqualTo(Token.Comma()));
        }

        [Test]
        public void CommaWhiteSpacePrepended()
        {
            Assert.That(Tokener.Tokenize("  ,").First(), Is.EqualTo(Token.Comma()));
        }

        [Test]
        public void Plus()
        {
            Assert.That(Tokener.Tokenize("+").First(), Is.EqualTo(Token.Plus()));
        }

        [Test]
        public void Equals()
        {
            Assert.That(Tokener.Tokenize("=").First(), Is.EqualTo(Token.Equals()));
        }

        [Test]
        public void LeftBracket()
        {
            Assert.That(Tokener.Tokenize("[").First(), Is.EqualTo(Token.LeftBracket()));
        }

        [Test]
        public void RightBracket()
        {
            Assert.That(Tokener.Tokenize("]").First(), Is.EqualTo(Token.RightBracket()));
        }

        [Test]
        public void PlusWhiteSpacePrepended()
        {
            Assert.That(Tokener.Tokenize("  +").First(), Is.EqualTo(Token.Plus()));
        }

        [Test]
        public void RightParenthesis()
        {
            Assert.That(Tokener.Tokenize(")").First(), Is.EqualTo(Token.RightParenthesis()));
        }

        [Test]
        public void Greater()
        {
            Assert.That(Tokener.Tokenize(">").First(), Is.EqualTo(Token.Greater()));
        }

        [Test]
        public void GreaterWhiteSpacePrepended()
        {
            Assert.That(Tokener.Tokenize("  >").First(), Is.EqualTo(Token.Greater()));
        }

        [Test]
        public void IdentifierLowerCaseOnly()
        {
            Assert.That(Tokener.Tokenize("foo").First(), Is.EqualTo(Token.Ident("foo")));
        }

        [Test]
        public void IdentifierMixedCase()
        {
            Assert.That(Tokener.Tokenize("FoObAr").First(), Is.EqualTo(Token.Ident("FoObAr")));
        }

        [Test]
        public void IdentifierIncludingDigits()
        {
            Assert.That(Tokener.Tokenize("foobar42").First(), Is.EqualTo(Token.Ident("foobar42")));
        }

        [Test]
        public void IdentifierWithUnderscores()
        {
            Assert.That(Tokener.Tokenize("_foo_BAR_42_").First(), Is.EqualTo(Token.Ident("_foo_BAR_42_")));
        }

        [Test]
        public void IdentifierWithHypens()
        {
            Assert.That(Tokener.Tokenize("foo-BAR-42").First(), Is.EqualTo(Token.Ident("foo-BAR-42")));
        }

        [Test]
        public void IdentifierUsingVendorExtensionSyntax()
        {
            Assert.That(Tokener.Tokenize("-foo-BAR-42").First(), Is.EqualTo(Token.Ident("-foo-BAR-42")));
        }

        [Test]
        public void IdentifierUsingVendorExtensionSyntaxCannotBeginWithDigit()
        {
            Assert.Throws<FormatException>(() =>
                Tokener.Tokenize("-42").ToArray());
        }

        [Test]
        public void Hash()
        {
            Assert.That(Tokener.Tokenize("#foo_BAR-baz-42").First(), Is.EqualTo(Token.Hash("foo_BAR-baz-42")));
        }

        [Test]
        public void Includes()
        {
            Assert.That(Tokener.Tokenize("~=").First().Kind, Is.EqualTo(TokenKind.Includes));
        }

        [Test]
        public void TildeTilde()
        {
            Assert.That(Tokener.Tokenize("~~").Take(2).ToArray(), Is.EqualTo(new[] { Token.Tilde(), Token.Tilde() }));
        }

        [Test]
        public void DashMatch()
        {
            Assert.That(Tokener.Tokenize("|=").First().Kind, Is.EqualTo(TokenKind.DashMatch));
        }

        [Test]
        public void Pipe()
        {
            Assert.That(Tokener.Tokenize("||").Take(2).ToArray(),
                        Is.EqualTo(new[] { Token.Char('|'), Token.Char('|') }));
        }

        [TestCase("\"\"")]
        [TestCase("''")]
        public void EmptyString(string input)
        {
            Assert.That(Tokener.Tokenize(input).First(), Is.EqualTo(Token.String(string.Empty)));
        }

        [Test]
        public void StringSingleQuoted()
        {
            Assert.That(Tokener.Tokenize("'foo bar'").First(), Is.EqualTo(Token.String("foo bar")));
        }

        [Test]
        public void StringDoubleQuoted()
        {
            Assert.That(Tokener.Tokenize("\"foo bar\"").First(), Is.EqualTo(Token.String("foo bar")));
        }

        [Test]
        public void StringDoubleQuotedWithEscapedDoubleQuotes()
        {
            Assert.That(Tokener.Tokenize("\"foo \\\"bar\\\" baz\"").First(), Is.EqualTo(Token.String("foo \"bar\" baz")));
        }

        [Test]
        public void StringSingleQuotedWithEscapedSingleQuotes()
        {
            Assert.That(Tokener.Tokenize(@"'foo \'bar\' baz'").First(), Is.EqualTo(Token.String("foo 'bar' baz")));
        }

        [Test]
        public void StringDoubleQuotedWithEscapedBackslashes()
        {
            Assert.That(Tokener.Tokenize("\"foo \\\\bar\\\\ baz\"").First(), Is.EqualTo(Token.String(@"foo \bar\ baz")));
        }

        [Test]
        public void StringSingleQuotedWithEscapedBackslashes()
        {
            Assert.That(Tokener.Tokenize(@"'foo \\bar\\ baz'").First(), Is.EqualTo(Token.String(@"foo \bar\ baz")));
        }

        [Test]
        public void BracketedIdent()
        {
            var token = Tokener.Tokenize("[foo]").GetEnumerator();
            Assert.That(token.MoveNext(), Is.True); Assert.That(token.Current, Is.EqualTo(Token.LeftBracket()));
            Assert.That(token.MoveNext(), Is.True); Assert.That(token.Current, Is.EqualTo(Token.Ident("foo")));
            Assert.That(token.MoveNext(), Is.True); Assert.That(token.Current, Is.EqualTo(Token.RightBracket()));
            Assert.That(token.MoveNext(), Is.True); Assert.That(token.Current, Is.EqualTo(Token.Eoi()));
            Assert.That(token.MoveNext(), Is.False);
        }

        [Test]
        public void BadHash()
        {
            Assert.Throws<FormatException>(() =>
                Tokener.Tokenize("#").ToArray());
        }

        [Test]
        public void HashDelimitedCorrectly()
        {
            var token = Tokener.Tokenize("#foo.").GetEnumerator();
            Assert.That(token.MoveNext(), Is.True); Assert.That(token.Current, Is.EqualTo(Token.Hash("foo")));
            Assert.That(token.MoveNext(), Is.True); Assert.That(token.Current, Is.EqualTo(Token.Dot()));
            Assert.That(token.MoveNext(), Is.True); Assert.That(token.Current, Is.EqualTo(Token.Eoi()));
            Assert.That(token.MoveNext(), Is.False);
        }

        [Test]
        public void Function()
        {
            Assert.That(Tokener.Tokenize("funky(").First(), Is.EqualTo(Token.Function("funky")));
        }

        [Test]
        public void FunctionWithEnclosedIdent()
        {
            var token = Tokener.Tokenize("foo(bar)").GetEnumerator();
            Assert.That(token.MoveNext(), Is.True); Assert.That(token.Current, Is.EqualTo(Token.Function("foo")));
            Assert.That(token.MoveNext(), Is.True); Assert.That(token.Current, Is.EqualTo(Token.Ident("bar")));
            Assert.That(token.MoveNext(), Is.True); Assert.That(token.Current, Is.EqualTo(Token.RightParenthesis()));
            Assert.That(token.MoveNext(), Is.True); Assert.That(token.Current, Is.EqualTo(Token.Eoi()));
            Assert.That(token.MoveNext(), Is.False);
        }

        [Test]
        public void Integer()
        {
            Assert.That(Tokener.Tokenize("42").First(), Is.EqualTo(Token.Integer("42")));
        }

        [Test]
        public void IntegerEnclosed()
        {
            var token = Tokener.Tokenize("[42]").GetEnumerator();
            Assert.That(token.MoveNext(), Is.True); Assert.That(token.Current, Is.EqualTo(Token.LeftBracket()));
            Assert.That(token.MoveNext(), Is.True); Assert.That(token.Current, Is.EqualTo(Token.Integer("42")));
            Assert.That(token.MoveNext(), Is.True); Assert.That(token.Current, Is.EqualTo(Token.RightBracket()));
            Assert.That(token.MoveNext(), Is.True); Assert.That(token.Current, Is.EqualTo(Token.Eoi()));
            Assert.That(token.MoveNext(), Is.False);
        }

        [Test]
        public void SubstringMatch()
        {
            Assert.That(Tokener.Tokenize("*=").First().Kind, Is.EqualTo(TokenKind.SubstringMatch));
        }

        [Test]
        public void Star()
        {
            Assert.That(Tokener.Tokenize("*").First(), Is.EqualTo(Token.Char('*')));
        }

        [Test]
        public void StarStar()
        {
            Assert.That(Tokener.Tokenize("**").Take(2).ToArray(),
                        Is.EqualTo(new[] { Token.Char('*'), Token.Char('*') }));
        }

        [Test]
        public void Tilde()
        {
            Assert.That(Tokener.Tokenize("~").First().Kind, Is.EqualTo(TokenKind.Tilde));
        }

        [Test]
        public void TildeWhitespacePrepended()
        {
            Assert.That(Tokener.Tokenize("  ~").First().Kind, Is.EqualTo(TokenKind.Tilde));
        }

        [Test]
        public void StringSingleQuoteUnterminated()
        {
            Assert.Throws<FormatException>(() =>
                Tokener.Tokenize("'foo").ToArray());
        }

        [Test]
        public void StringDoubleQuoteUnterminated()
        {
            Assert.Throws<FormatException>(() =>
                Tokener.Tokenize("\"foo").ToArray());
        }

        [TestCase(@"'f\oo")]
        [TestCase(@"'foo\")]
        public void StringInvalidEscaping(string input)
        {
            Assert.Throws<FormatException>(() =>
                Tokener.Tokenize(input).ToArray());
        }

        [Test]
        public void NullReader()
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                Tokener.Tokenize((TextReader) null));
            Assert.That(e.ParamName, Is.EqualTo("reader"));
        }

        [Test]
        public void StringReader()
        {
            Assert.That(Tokener.Tokenize(new StringReader("123,*")).ToArray(),
                        Is.EqualTo(new[] { Token.Integer("123"), Token.Comma(), Token.Char('*'), Token.Eoi() }));
        }

        [Test]
        public void InvalidChar()
        {
            Assert.Throws<FormatException>(() =>
                Tokener.Tokenize("what?").ToArray());
        }

        [Test]
        public void Not()
        {
            Assert.That(Tokener.Tokenize(":not(").First(), Is.EqualTo(Token.Not()));
        }

        [Test]
        public void NotNotFunction()
        {
            Assert.That(Tokener.Tokenize(":notnot(").Take(2),
                        Is.EqualTo(new[] { Token.Char(':'), Token.Function("notnot") }));
        }
    }
}
