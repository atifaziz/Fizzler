using System;
using System.Linq;
using NUnit.Framework;

namespace Fizzler.Tests
{
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

        [Test]
        public void WhiteSpace()
        {
            Assert.AreEqual(Token.WhiteSpace(" \r \n \f \t "), Tokener.Tokenize(" \r \n \f \t etc").First());
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

        [Test, ExpectedException(typeof(FormatException))]
        public void IdentifierUsingVendorExtensionSyntaxCannotBeginWithDigit()
        {
            Tokener.Tokenize("-42").ToArray();
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

        [Test, ExpectedException(typeof(FormatException))]
        public void BadIncludes()
        {
            Tokener.Tokenize("~~").ToArray();
        }

        [Test]
        public void DashMatch()
        {
            Assert.AreEqual(TokenKind.DashMatch, Tokener.Tokenize("|=").First().Kind);
        }

        [Test, ExpectedException(typeof(FormatException))]
        public void BadDashMatch()
        {
            Tokener.Tokenize("||").ToArray();
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
        
        [Test, ExpectedException(typeof(FormatException))]
        public void BadHash()
        {
            Tokener.Tokenize("#").ToArray();
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
            Assert.AreEqual(TokenKind.Star, Tokener.Tokenize("*").First().Kind);
        }

        [Test]
        public void StarStar()
        {
            Assert.AreEqual(new[] { TokenKind.Star, TokenKind.Star }, Tokener.Tokenize("**").Take(2).Select(t => t.Kind).ToArray());
        }
    }
}