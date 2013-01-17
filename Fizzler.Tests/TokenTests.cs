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

        [Test,ExpectedException(typeof(ArgumentNullException))]
        public void IdentNullText()
        {
            Token.Ident(null);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void IdentEmptyText()
        {
            Token.Ident(string.Empty);
        }

        [Test]
        public void Hash()
        {
            AssertToken(TokenKind.Hash, "foo", Token.Hash("foo"));
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void HashNullText()
        {
            Token.Hash(null);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void HashEmptyText()
        {
            Token.Hash(string.Empty);
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

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void FunctionNullText()
        {
            Token.Function(null);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void FunctionEmptyText()
        {
            Token.Function(string.Empty);
        }

        public void WhiteSpace()
        {
            AssertToken(TokenKind.WhiteSpace, " \n ", Token.WhiteSpace("foo"));
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void WhiteSpaceNullText()
        {
            Token.WhiteSpace(null);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void WhiteSpaceEmptyText()
        {
            Token.WhiteSpace(string.Empty);
        }

        public void Integer()
        {
            AssertToken(TokenKind.Integer, "123", Token.Integer("123"));
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void IntegerNullText()
        {
            Token.Integer(null);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void IntegerEmptyText()
        {
            Token.Integer(string.Empty);
        }
       
        private static void AssertToken(TokenKind kindExpected, Token token)
        {
            AssertToken(kindExpected, null, token);
        }

        private static void AssertToken(TokenKind expectedKind, string expectedText, Token token)
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
