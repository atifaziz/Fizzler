using System;
using System.Linq;
using Fizzler.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
    [TestClass]
    public class TokenerTests
    {
        [TestMethod]
        public void NullInput()
        {
            Assert.AreEqual(Token.Eoi(), Tokener.Tokenize((string) null).Single());
        }

        [TestMethod]
        public void EmptyInput()
        {
            Assert.AreEqual(Token.Eoi(), Tokener.Tokenize(string.Empty).Single());
        }

        [TestMethod]
        public void WhiteSpace()
        {
            Assert.AreEqual(Token.WhiteSpace(" \r \n \f \t "), Tokener.Tokenize(" \r \n \f \t etc").First());
        }

        [TestMethod]
        public void Colon()
        {
            Assert.AreEqual(Token.Colon(), Tokener.Tokenize(":").First());
        }

        [TestMethod]
        public void Comma()
        {
            Assert.AreEqual(Token.Comma(), Tokener.Tokenize(",").First());
        }

        [TestMethod]
        public void CommaWhiteSpacePrepended()
        {
            Assert.AreEqual(Token.Comma(), Tokener.Tokenize("  ,").First());
        }

        [TestMethod]
        public void Plus()
        {
            Assert.AreEqual(Token.Plus(), Tokener.Tokenize("+").First());
        }

        [TestMethod]
        public void Equals()
        {
            Assert.AreEqual(Token.Equals(), Tokener.Tokenize("=").First());
        }

        [TestMethod]
        public void LeftBracket()
        {
            Assert.AreEqual(Token.LeftBracket(), Tokener.Tokenize("[").First());
        }

        [TestMethod]
        public void RightBracket()
        {
            Assert.AreEqual(Token.RightBracket(), Tokener.Tokenize("]").First());
        }

        [TestMethod]
        public void PlusWhiteSpacePrepended()
        {
            Assert.AreEqual(Token.Plus(), Tokener.Tokenize("  +").First());
        }

        [TestMethod]
        public void RightParenthesis()
        {
            Assert.AreEqual(Token.RightParenthesis(), Tokener.Tokenize(")").First());
        }

        [TestMethod]
        public void Greater()
        {
            Assert.AreEqual(Token.Greater(), Tokener.Tokenize(">").First());
        }

        [TestMethod]
        public void GreaterWhiteSpacePrepended()
        {
            Assert.AreEqual(Token.Greater(), Tokener.Tokenize("  >").First());
        }

        [TestMethod]
        public void IdentifierLowerCaseOnly()
        {
            Assert.AreEqual(Token.Ident("foo"), Tokener.Tokenize("foo").First());
        }

        [TestMethod]
        public void IdentifierMixedCase()
        {
            Assert.AreEqual(Token.Ident("FoObAr"), Tokener.Tokenize("FoObAr").First());
        }

        [TestMethod]
        public void IdentifierIncludingDigits()
        {
            Assert.AreEqual(Token.Ident("foobar42"), Tokener.Tokenize("foobar42").First());
        }

        [TestMethod]
        public void IdentifierWithUnderscores()
        {
            Assert.AreEqual(Token.Ident("_foo_BAR_42_"), Tokener.Tokenize("_foo_BAR_42_").First());
        }

        [TestMethod]
        public void IdentifierWithHypens()
        {
            Assert.AreEqual(Token.Ident("foo-BAR-42"), Tokener.Tokenize("foo-BAR-42").First());
        }

        [TestMethod]
        public void IdentifierUsingVendorExtensionSyntax()
        {
            Assert.AreEqual(Token.Ident("-foo-BAR-42"), Tokener.Tokenize("-foo-BAR-42").First());
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void IdentifierUsingVendorExtensionSyntaxCannotBeginWithDigit()
        {
            Tokener.Tokenize("-42").ToArray();
        }

        [TestMethod]
        public void Hash()
        {
            Assert.AreEqual(Token.Hash("foo_BAR-baz-42"), Tokener.Tokenize("#foo_BAR-baz-42").First());
        }

        [TestMethod]
        public void Includes()
        {
            Assert.AreEqual(TokenKind.Includes, Tokener.Tokenize("~=").First().Kind);
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void BadIncludes()
        {
            Tokener.Tokenize("~~").ToArray();
        }

        [TestMethod]
        public void DashMatch()
        {
            Assert.AreEqual(TokenKind.DashMatch, Tokener.Tokenize("|=").First().Kind);
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void BadDashMatch()
        {
            Tokener.Tokenize("||").ToArray();
        }

        [TestMethod]
        public void StringSingleQuoted()
        {
            Assert.AreEqual(Token.String("foo bar"), Tokener.Tokenize("'foo bar'").First());
        }

        [TestMethod]
        public void StringDoubleQuoted()
        {
            Assert.AreEqual(Token.String("foo bar"), Tokener.Tokenize("\"foo bar\"").First());
        }

        [TestMethod]
        public void StringDoubleQuotedWithEscapedDoubleQuotes()
        {
            Assert.AreEqual(Token.String("foo \"bar\" baz"), Tokener.Tokenize("\"foo \\\"bar\\\" baz\"").First());
        }

        [TestMethod]
        public void StringSingleQuotedWithEscapedSingleQuotes()
        {
            Assert.AreEqual(Token.String("foo 'bar' baz"), Tokener.Tokenize(@"'foo \'bar\' baz'").First());
        }

        [TestMethod]
        public void StringDoubleQuotedWithEscapedBackslashes()
        {
            Assert.AreEqual(Token.String(@"foo \bar\ baz"), Tokener.Tokenize("\"foo \\\\bar\\\\ baz\"").First());
        }

        [TestMethod]
        public void StringSingleQuotedWithEscapedBackslashes()
        {
            Assert.AreEqual(Token.String(@"foo \bar\ baz"), Tokener.Tokenize(@"'foo \\bar\\ baz'").First());
        }

        [TestMethod]
        public void BracketedIdent()
        {
            var token = Tokener.Tokenize("[foo]").GetEnumerator();
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.LeftBracket(), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Ident("foo"), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.RightBracket(), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Eoi(), token.Current);
            Assert.IsFalse(token.MoveNext());
        }
        
        [TestMethod, ExpectedException(typeof(FormatException))]
        public void BadHash()
        {
            Tokener.Tokenize("#").ToArray();
        }

        [TestMethod]
        public void HashDelimitedCorrectly()
        {
            var token = Tokener.Tokenize("#foo.").GetEnumerator();
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Hash("foo"), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Dot(), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Eoi(), token.Current);
            Assert.IsFalse(token.MoveNext());
        }

        [TestMethod]
        public void Function()
        {
            Assert.AreEqual(Token.Function("funky"), Tokener.Tokenize("funky(").First());
        }

        [TestMethod]
        public void FunctionWithEnclosedIdent()
        {
            var token = Tokener.Tokenize("foo(bar)").GetEnumerator();
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Function("foo"), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Ident("bar"), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.RightParenthesis(), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Eoi(), token.Current);
            Assert.IsFalse(token.MoveNext());
        }

        [TestMethod]
        public void Integer()
        {
            Assert.AreEqual(Token.Integer("42"), Tokener.Tokenize("42").First());
        }

        [TestMethod]
        public void IntegerEnclosed()
        {
            var token = Tokener.Tokenize("[42]").GetEnumerator();
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.LeftBracket(), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Integer("42"), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.RightBracket(), token.Current);
            Assert.IsTrue(token.MoveNext()); Assert.AreEqual(Token.Eoi(), token.Current);
            Assert.IsFalse(token.MoveNext());
        }
    }
}