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
            Assert.AreEqual(Token.Eoi(), Tokener.Tokenize((string) null).Single());
        }

        [Test]
        public void EmptyInput()
        {
            Assert.AreEqual(Token.Eoi(), Tokener.Tokenize(string.Empty).Single());
        }

        [TestCase(" \r \n \f \t ", " \r \n \f \t ")]
        [TestCase(" \r \n \f \t ", " \r \n \f \t etc")]
        public void WhiteSpace(string ws, string input)
        {
            Assert.AreEqual(Token.WhiteSpace(ws), Tokener.Tokenize(input).First());
        }

        [Test]
        public void Colon()
        {
            Assert.AreEqual(Token.Colon(), Tokener.Tokenize(":").First());
        }

        [Test]
        public void Comma()
        {
            Assert.AreEqual(Token.Comma(), Tokener.Tokenize(",").First());
        }

        [Test]
        public void CommaWhiteSpacePrepended()
        {
            Assert.AreEqual(Token.Comma(), Tokener.Tokenize("  ,").First());
        }

        [Test]
        public void Plus()
        {
            Assert.AreEqual(Token.Plus(), Tokener.Tokenize("+").First());
        }

        [Test]
        public void Equals()
        {
            Assert.AreEqual(Token.Equals(), Tokener.Tokenize("=").First());
        }

        [Test]
        public void LeftBracket()
        {
            Assert.AreEqual(Token.LeftBracket(), Tokener.Tokenize("[").First());
        }

        [Test]
        public void RightBracket()
        {
            Assert.AreEqual(Token.RightBracket(), Tokener.Tokenize("]").First());
        }

        [Test]
        public void PlusWhiteSpacePrepended()
        {
            Assert.AreEqual(Token.Plus(), Tokener.Tokenize("  +").First());
        }

        [Test]
        public void RightParenthesis()
        {
            Assert.AreEqual(Token.RightParenthesis(), Tokener.Tokenize(")").First());
        }

        [Test]
        public void Greater()
        {
            Assert.AreEqual(Token.Greater(), Tokener.Tokenize(">").First());
        }

        [Test]
        public void GreaterWhiteSpacePrepended()
        {
            Assert.AreEqual(Token.Greater(), Tokener.Tokenize("  >").First());
        }

        [Test]
        public void IdentifierLowerCaseOnly()
        {
            Assert.AreEqual(Token.Ident("foo"), Tokener.Tokenize("foo").First());
        }

        [Test]
        public void IdentifierMixedCase()
        {
            Assert.AreEqual(Token.Ident("FoObAr"), Tokener.Tokenize("FoObAr").First());
        }

        [Test]
        public void IdentifierIncludingDigits()
        {
            Assert.AreEqual(Token.Ident("foobar42"), Tokener.Tokenize("foobar42").First());
        }

        [Test]
        public void IdentifierWithUnderscores()
        {
            Assert.AreEqual(Token.Ident("_foo_BAR_42_"), Tokener.Tokenize("_foo_BAR_42_").First());
        }

        [Test]
        public void IdentifierWithHypens()
        {
            Assert.AreEqual(Token.Ident("foo-BAR-42"), Tokener.Tokenize("foo-BAR-42").First());
        }

        [Test]
        public void IdentifierUsingVendorExtensionSyntax()
        {
            Assert.AreEqual(Token.Ident("-foo-BAR-42"), Tokener.Tokenize("-foo-BAR-42").First());
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
            Assert.AreEqual(Token.Hash("foo_BAR-baz-42"), Tokener.Tokenize("#foo_BAR-baz-42").First());
        }

        [Test]
        public void Includes()
        {
            Assert.AreEqual(TokenKind.Includes, Tokener.Tokenize("~=").First().Kind);
        }

        [Test]
        public void TildeTilde()
        {
            Assert.AreEqual(new[] { Token.Tilde(), Token.Tilde() }, Tokener.Tokenize("~~").Take(2).ToArray());
        }

        [Test]
        public void DashMatch()
        {
            Assert.AreEqual(TokenKind.DashMatch, Tokener.Tokenize("|=").First().Kind);
        }

        [Test]
        public void Pipe()
        {
            Assert.AreEqual(new[] { Token.Char('|'), Token.Char('|') }, Tokener.Tokenize("||").Take(2).ToArray());
        }

        [TestCase("\"\"")]
        [TestCase("''")]
        public void EmptyString(string input)
        {
            Assert.AreEqual(Token.String(string.Empty), Tokener.Tokenize(input).First());
        }

        [Test]
        public void StringSingleQuoted()
        {
            Assert.AreEqual(Token.String("foo bar"), Tokener.Tokenize("'foo bar'").First());
        }

        [Test]
        public void StringDoubleQuoted()
        {
            Assert.AreEqual(Token.String("foo bar"), Tokener.Tokenize("\"foo bar\"").First());
        }

        [Test]
        public void StringDoubleQuotedWithEscapedDoubleQuotes()
        {
            Assert.AreEqual(Token.String("foo \"bar\" baz"), Tokener.Tokenize("\"foo \\\"bar\\\" baz\"").First());
        }

        [Test]
        public void StringSingleQuotedWithEscapedSingleQuotes()
        {
            Assert.AreEqual(Token.String("foo 'bar' baz"), Tokener.Tokenize(@"'foo \'bar\' baz'").First());
        }

        [Test]
        public void StringDoubleQuotedWithEscapedBackslashes()
        {
            Assert.AreEqual(Token.String(@"foo \bar\ baz"), Tokener.Tokenize("\"foo \\\\bar\\\\ baz\"").First());
        }

        [Test]
        public void StringSingleQuotedWithEscapedBackslashes()
        {
            Assert.AreEqual(Token.String(@"foo \bar\ baz"), Tokener.Tokenize(@"'foo \\bar\\ baz'").First());
        }

        [Test]
        public void BracketedIdent()
        {
            var token = Tokener.Tokenize("[foo]").GetEnumerator();
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.LeftBracket(), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Ident("foo"), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.RightBracket(), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Eoi(), token.Current);
            Assert.IsFalse(token.MoveNext());
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
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Hash("foo"), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Dot(), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Eoi(), token.Current);
            Assert.IsFalse(token.MoveNext());
        }

        [Test]
        public void Function()
        {
            Assert.AreEqual(Token.Function("funky"), Tokener.Tokenize("funky(").First());
        }

        [Test]
        public void FunctionWithEnclosedIdent()
        {
            var token = Tokener.Tokenize("foo(bar)").GetEnumerator();
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Function("foo"), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Ident("bar"), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.RightParenthesis(), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Eoi(), token.Current);
            Assert.IsFalse(token.MoveNext());
        }

        [Test]
        public void Integer()
        {
            Assert.AreEqual(Token.Integer("42"), Tokener.Tokenize("42").First());
        }

        [Test]
        public void IntegerEnclosed()
        {
            var token = Tokener.Tokenize("[42]").GetEnumerator();
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.LeftBracket(), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Integer("42"), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.RightBracket(), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Eoi(), token.Current);
            Assert.IsFalse(token.MoveNext());
        }

        [Test]
        public void SubstringMatch()
        {
            Assert.AreEqual(TokenKind.SubstringMatch, Tokener.Tokenize("*=").First().Kind);
        }

        [Test]
        public void Star()
        {
            Assert.AreEqual(Token.Char('*'), Tokener.Tokenize("*").First());
        }

        [Test]
        public void StarStar()
        {
            Assert.AreEqual(new[] { Token.Char('*'), Token.Char('*') }, Tokener.Tokenize("**").Take(2).ToArray());
        }

        [Test]
        public void Tilde()
        {
            Assert.AreEqual(TokenKind.Tilde, Tokener.Tokenize("~").First().Kind);
        }

        [Test]
        public void TildeWhitespacePrepended()
        {
            Assert.AreEqual(TokenKind.Tilde, Tokener.Tokenize("  ~").First().Kind);
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
            Assert.AreEqual(new[] { Token.Integer("123"), Token.Comma(), Token.Char('*'), Token.Eoi() },
                Tokener.Tokenize(new StringReader("123,*")).ToArray());
        }

        [Test]
        public void InvalidChar()
        {
            Assert.Throws<FormatException>(() =>
                Tokener.Tokenize("what?").ToArray());
        }
    }
}
