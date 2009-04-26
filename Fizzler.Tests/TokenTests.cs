using System;
using Fizzler.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
    [TestClass]
    public class TokenTests
    {
        [TestMethod]
        public void DefaultState()
        {
            AssertToken(TokenKind.Eoi, new Token());
        }

        [TestMethod]
        public void Star()
        {
            AssertToken(TokenKind.Star, Token.Star());
        }

        [TestMethod]
        public void Dot()
        {
            AssertToken(TokenKind.Dot, Token.Dot());
        }

        [TestMethod]
        public void Colon()
        {
            AssertToken(TokenKind.Colon, Token.Colon());
        }

        [TestMethod]
        public void Comma()
        {
            AssertToken(TokenKind.Comma, Token.Comma());
        }

        [TestMethod]
        public void Equals()
        {
            AssertToken(TokenKind.Equals, Token.Equals());
        }

        [TestMethod]
        public void LeftBracket()
        {
            AssertToken(TokenKind.LeftBracket, Token.LeftBracket());
        }

        [TestMethod]
        public void RightBracket()
        {
            AssertToken(TokenKind.RightBracket, Token.RightBracket());
        }

        [TestMethod]
        public void Plus()
        {
            AssertToken(TokenKind.Plus, Token.Plus());
        }

        [TestMethod]
        public void Greater()
        {
            AssertToken(TokenKind.Greater, Token.Greater());
        }

        [TestMethod]
        public void RightParenthesis()
        {
            AssertToken(TokenKind.RightParenthesis, Token.RightParenthesis());
        }

        [TestMethod]
        public void Eoi()
        {
            AssertToken(TokenKind.Eoi, Token.Eoi());
        }

        [TestMethod]
        public void Includes()
        {
            AssertToken(TokenKind.Includes, Token.Includes());
        }

        [TestMethod]
        public void DashMatch()
        {
            AssertToken(TokenKind.DashMatch, Token.DashMatch());
        }

        [TestMethod]
        public void Ident()
        {
            AssertToken(TokenKind.Ident, "foo", Token.Ident("foo"));
        }

        [TestMethod,ExpectedException(typeof(ArgumentNullException))]
        public void IdentNullText()
        {
            Token.Ident(null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void IdentEmptyText()
        {
            Token.Ident(string.Empty);
        }

        [TestMethod]
        public void Hash()
        {
            AssertToken(TokenKind.Hash, "foo", Token.Hash("foo"));
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void HashNullText()
        {
            Token.Hash(null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void HashEmptyText()
        {
            Token.Hash(string.Empty);
        }

        [TestMethod]
        public void String()
        {
            AssertToken(TokenKind.String, "foo", Token.String("foo"));
        }

        [TestMethod]
        public void StringNullText()
        {
            Token.String(null);
        }

        [TestMethod]
        public void StringEmptyText()
        {
            AssertToken(TokenKind.String, string.Empty, Token.String(string.Empty));
        }

        public void Function()
        {
            AssertToken(TokenKind.Function, "foo", Token.Function("foo"));
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void FunctionNullText()
        {
            Token.Function(null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void FunctionEmptyText()
        {
            Token.Function(string.Empty);
        }

        public void WhiteSpace()
        {
            AssertToken(TokenKind.WhiteSpace, " \n ", Token.WhiteSpace("foo"));
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void WhiteSpaceNullText()
        {
            Token.WhiteSpace(null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void WhiteSpaceEmptyText()
        {
            Token.WhiteSpace(string.Empty);
        }

        public void Integer()
        {
            AssertToken(TokenKind.Integer, "123", Token.Integer("123"));
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void IntegerNullText()
        {
            Token.Integer(null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
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
    }
}
