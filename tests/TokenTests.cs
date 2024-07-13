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
            Assert.That(() => Token.Ident(null!), Throws.ArgumentNullException("text"));
        }

        [Test]
        public void IdentEmptyText()
        {
            Assert.That(() => Token.Ident(string.Empty), Throws.ArgumentException("text"));
        }

        [Test]
        public void Hash()
        {
            AssertToken(TokenKind.Hash, "foo", Token.Hash("foo"));
        }

        [Test]
        public void HashNullText()
        {
            Assert.That(() => Token.Hash(null!), Throws.ArgumentNullException("text"));
        }

        [Test]
        public void HashEmptyText()
        {
            Assert.That(() => Token.Hash(string.Empty), Throws.ArgumentException("text"));
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

        [Test]
        public void Function()
        {
            AssertToken(TokenKind.Function, "foo", Token.Function("foo"));
        }

        [Test]
        public void FunctionNullText()
        {
            Assert.That(() => Token.Function(null!), Throws.ArgumentNullException("text"));
        }

        [Test]
        public void FunctionEmptyText()
        {
            Assert.That(() => Token.Function(string.Empty), Throws.ArgumentException("text"));
        }

        [Test]
        public void Not()
        {
            AssertToken(TokenKind.Not, Token.Not());
        }

        [Test]
        public void WhiteSpace()
        {
            AssertToken(TokenKind.WhiteSpace, " \n ", Token.WhiteSpace(" \n "));
        }

        [Test]
        public void WhiteSpaceNullText()
        {
            Assert.That(() => Token.WhiteSpace(null!), Throws.ArgumentNullException("text"));
        }

        [Test]
        public void WhiteSpaceEmptyText()
        {
            Assert.That(() => Token.WhiteSpace(string.Empty), Throws.ArgumentException("text"));
        }

        [Test]
        public void Integer()
        {
            AssertToken(TokenKind.Integer, "123", Token.Integer("123"));
        }

        [Test]
        public void IntegerNullText()
        {
            Assert.That(() => Token.Integer(null!), Throws.ArgumentNullException("text"));
        }

        [Test]
        public void IntegerEmptyText()
        {
            Assert.That(() => Token.Integer(string.Empty), Throws.ArgumentException("text"));

        }

        static void AssertToken(TokenKind kindExpected, Token token)
        {
            AssertToken(kindExpected, null, token);
        }

        static void AssertToken(TokenKind expectedKind, string? expectedText, Token token)
        {
            Assert.That(token.Kind, Is.EqualTo(expectedKind));
            if (expectedText == null)
                Assert.That(token.Text, Is.Null);
            else
                Assert.That(token.Text, Is.EqualTo(expectedText));
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
            Assert.That(Token.Eoi().ToString(), Is.EqualTo("Eoi"));
            Assert.That(Token.Ident("foo").ToString(), Is.EqualTo("Ident: foo"));
            Assert.That(Token.Hash("foo").ToString(), Is.EqualTo("Hash: foo"));
            Assert.That(Token.Includes().ToString(), Is.EqualTo("Includes"));
            Assert.That(Token.DashMatch().ToString(), Is.EqualTo("DashMatch"));
            Assert.That(Token.PrefixMatch().ToString(), Is.EqualTo("PrefixMatch"));
            Assert.That(Token.SuffixMatch().ToString(), Is.EqualTo("SuffixMatch"));
            Assert.That(Token.SubstringMatch().ToString(), Is.EqualTo("SubstringMatch"));
            Assert.That(Token.String("foo").ToString(), Is.EqualTo("String: foo"));
            Assert.That(Token.Plus().ToString(), Is.EqualTo("Plus"));
            Assert.That(Token.Greater().ToString(), Is.EqualTo("Greater"));
            Assert.That(Token.WhiteSpace(" ").ToString(), Is.EqualTo("WhiteSpace:  "));
            Assert.That(Token.Function("foo").ToString(), Is.EqualTo("Function: foo"));
            Assert.That(Token.Integer("42").ToString(), Is.EqualTo("Integer: 42"));
            Assert.That(Token.Tilde().ToString(), Is.EqualTo("Tilde"));
            Assert.That(Token.Star().ToString(), Is.EqualTo("Char: *"));
            Assert.That(Token.Dot().ToString(), Is.EqualTo("Char: ."));
            Assert.That(Token.Colon().ToString(), Is.EqualTo("Char: :"));
            Assert.That(Token.Comma().ToString(), Is.EqualTo("Char: ,"));
            Assert.That(Token.Equals().ToString(), Is.EqualTo("Char: ="));
            Assert.That(Token.LeftBracket().ToString(), Is.EqualTo("Char: ["));
            Assert.That(Token.RightBracket().ToString(), Is.EqualTo("Char: ]"));
            Assert.That(Token.RightParenthesis().ToString(), Is.EqualTo("Char: )"));
            Assert.That(Token.Pipe().ToString(), Is.EqualTo("Char: |"));
        }
    }
}
