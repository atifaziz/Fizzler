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
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class TokenTests
    {
        [Test]
        public void DefaultState()
        {
            AssertToken(TokenKind.Eoi, new Token());
        }

        [Test]
        public void Star()
        {
            AssertToken(TokenKind.Char, "*", Token.Star());
        }

        [Test]
        public void Dot()
        {
            AssertToken(TokenKind.Char, ".", Token.Dot());
        }

        [Test]
        public void Colon()
        {
            AssertToken(TokenKind.Char, ":", Token.Colon());
        }

        [Test]
        public void Comma()
        {
            AssertToken(TokenKind.Char, ",", Token.Comma());

        }

        [Test]
        public void Equals()
        {
            AssertToken(TokenKind.Char, "=", Token.Equals());

        }

        [Test]
        public void LeftBracket()
        {
            AssertToken(TokenKind.Char, "[", Token.LeftBracket());

        }

        [Test]
        public void RightBracket()
        {
            AssertToken(TokenKind.Char, "]", Token.RightBracket());
        }

        [Test]
        public void Plus()
        {
            AssertToken(TokenKind.Plus, Token.Plus());
        }

        [Test]
        public void Greater()
        {
            AssertToken(TokenKind.Greater, Token.Greater());
        }

        [Test]
        public void RightParenthesis()
        {
            AssertToken(TokenKind.Char, ")", Token.RightParenthesis());
        }

        [Test]
        public void Eoi()
        {
            AssertToken(TokenKind.Eoi, Token.Eoi());
        }

        [Test]
        public void Includes()
        {
            AssertToken(TokenKind.Includes, Token.Includes());
        }

        [Test]
        public void DashMatch()
        {
            AssertToken(TokenKind.DashMatch, Token.DashMatch());
        }

        [Test]
        public void Ident()
        {
            AssertToken(TokenKind.Ident, "foo", Token.Ident("foo"));
        }

        [Test]
        public void IdentNullText()
        {
            var e = Assert.Throws<ArgumentNullException>(() => Token.Ident(null));
            Assert.That(e.ParamName, Is.EqualTo("text"));
        }

        [Test]
        public void IdentEmptyText()
        {
            var e = Assert.Throws<ArgumentException>(() => Token.Ident(string.Empty));
            Assert.That(e.ParamName, Is.EqualTo("text"));
        }

        [Test]
        public void Hash()
        {
            AssertToken(TokenKind.Hash, "foo", Token.Hash("foo"));
        }

        [Test]
        public void HashNullText()
        {
            var e = Assert.Throws<ArgumentNullException>(() => Token.Hash(null));
            Assert.That(e.ParamName, Is.EqualTo("text"));
        }

        [Test]
        public void HashEmptyText()
        {
            var e = Assert.Throws<ArgumentException>(() => Token.Hash(string.Empty));
            Assert.That(e.ParamName, Is.EqualTo("text"));
        }

        [Test]
        public void String()
        {
            AssertToken(TokenKind.String, "foo", Token.String("foo"));
        }

        [Test]
        public void StringNullText()
        {
            Token.String(null);
        }

        [Test]
        public void StringEmptyText()
        {
            AssertToken(TokenKind.String, string.Empty, Token.String(string.Empty));
        }

        public void Function()
        {
            AssertToken(TokenKind.Function, "foo", Token.Function("foo"));
        }

        [Test]
        public void FunctionNullText()
        {
            var e = Assert.Throws<ArgumentNullException>(() => Token.Function(null));
            Assert.That(e.ParamName, Is.EqualTo("text"));
        }

        [Test]
        public void FunctionEmptyText()
        {
            var e = Assert.Throws<ArgumentException>(() => Token.Function(string.Empty));
            Assert.That(e.ParamName, Is.EqualTo("text"));
        }

        public void WhiteSpace()
        {
            AssertToken(TokenKind.WhiteSpace, " \n ", Token.WhiteSpace("foo"));
        }

        [Test]
        public void WhiteSpaceNullText()
        {
            Assert.Throws<ArgumentNullException>(() => Token.WhiteSpace(null));
        }

        [Test]
        public void WhiteSpaceEmptyText()
        {
            Assert.Throws<ArgumentException>(() => Token.WhiteSpace(string.Empty));
        }

        public void Integer()
        {
            AssertToken(TokenKind.Integer, "123", Token.Integer("123"));
        }

        [Test]
        public void IntegerNullText()
        {
            var e = Assert.Throws<ArgumentNullException>(() => Token.Integer(null));
            Assert.That(e.ParamName, Is.EqualTo("text"));
        }

        [Test]
        public void IntegerEmptyText()
        {
            var e = Assert.Throws<ArgumentException>(() => Token.Integer(string.Empty));
            Assert.That(e.ParamName, Is.EqualTo("text"));
        }

        static void AssertToken(TokenKind kindExpected, Token token)
        {
            AssertToken(kindExpected, null, token);
        }

        static void AssertToken(TokenKind expectedKind, string expectedText, Token token)
        {
            Assert.AreEqual(expectedKind, token.Kind);
            if (expectedText == null)
                Assert.IsNull(token.Text);
            else
                Assert.AreEqual(expectedText, token.Text);
        }

        [Test]
        public void PrefixMatch()
        {
            AssertToken(TokenKind.PrefixMatch, Token.PrefixMatch());
        }

        [Test]
        public void SuffixMatch()
        {
            AssertToken(TokenKind.SuffixMatch, Token.SuffixMatch());
        }

        [Test]
        public void SubstringMatch()
        {
            AssertToken(TokenKind.SubstringMatch, Token.SubstringMatch());
        }

        [Test]
        public void GeneralSibling()
        {
            AssertToken(TokenKind.Tilde, Token.Tilde());
        }

        [Test]
        public void Pipe()
        {
            var pipe = Token.Pipe();
            Assert.That(pipe.Kind, Is.EqualTo(TokenKind.Char));
            Assert.That(pipe.Text, Is.EqualTo("|"));
        }

        [Test]
        public void StringRepresentations()
        {
            Assert.AreEqual("Eoi", Token.Eoi().ToString());
            Assert.AreEqual("Ident: foo", Token.Ident("foo").ToString());
            Assert.AreEqual("Hash: foo", Token.Hash("foo").ToString());
            Assert.AreEqual("Includes", Token.Includes().ToString());
            Assert.AreEqual("DashMatch", Token.DashMatch().ToString());
            Assert.AreEqual("PrefixMatch", Token.PrefixMatch().ToString());
            Assert.AreEqual("SuffixMatch", Token.SuffixMatch().ToString());
            Assert.AreEqual("SubstringMatch", Token.SubstringMatch().ToString());
            Assert.AreEqual("String: foo", Token.String("foo").ToString());
            Assert.AreEqual("Plus", Token.Plus().ToString());
            Assert.AreEqual("Greater", Token.Greater().ToString());
            Assert.AreEqual("WhiteSpace:  ", Token.WhiteSpace(" ").ToString());
            Assert.AreEqual("Function: foo", Token.Function("foo").ToString());
            Assert.AreEqual("Integer: 42", Token.Integer("42").ToString());
            Assert.AreEqual("Tilde", Token.Tilde().ToString());
            Assert.AreEqual("Char: *", Token.Star().ToString());
            Assert.AreEqual("Char: .", Token.Dot().ToString());
            Assert.AreEqual("Char: :", Token.Colon().ToString());
            Assert.AreEqual("Char: ,", Token.Comma().ToString());
            Assert.AreEqual("Char: =", Token.Equals().ToString());
            Assert.AreEqual("Char: [", Token.LeftBracket().ToString());
            Assert.AreEqual("Char: ]", Token.RightBracket().ToString());
            Assert.AreEqual("Char: )", Token.RightParenthesis().ToString());
            Assert.AreEqual("Char: |", Token.Pipe().ToString());
        }
    }
}
