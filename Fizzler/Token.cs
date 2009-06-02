using System;

namespace Fizzler
{
    /// <summary>
    /// Represent a token and optionally any text associated with it.
    /// </summary>
    public struct Token
    {
        /// <summary>
        /// Gets the kind/type/class of the token.
        /// </summary>
        public TokenKind Kind { get; private set; }

        /// <summary>
        /// Gets text, if any, associated with the token.
        /// </summary>
        public string Text { get; private set; }

        private Token(TokenKind kind) : this(kind, null) {}

        private Token(TokenKind kind, string text) : this()
        {
            Kind = kind;
            Text = text;
        }

        /// <summary>
        /// Creates an end-of-input token.
        /// </summary>
        public static Token Eoi()
        {
            return new Token(TokenKind.Eoi);
        }

        /// <summary>
        /// Creates an star token.
        /// </summary>
        public static Token Star()
        {
            return new Token(TokenKind.Star);
        }

        /// <summary>
        /// Creates an dot token.
        /// </summary>
        public static Token Dot()
        {
            return new Token(TokenKind.Dot);
        }

        /// <summary>
        /// Creates an colon token.
        /// </summary>
        public static Token Colon()
        {
            return new Token(TokenKind.Colon);
        }

        /// <summary>
        /// Creates an comma token.
        /// </summary>
        public static Token Comma()
        {
            return new Token(TokenKind.Comma);
        }

        /// <summary>
        /// Creates an right parenthesis token.
        /// </summary>
        public static Token RightParenthesis()
        {
            return new Token(TokenKind.RightParenthesis);
        }

        /// <summary>
        /// Creates an equals token.
        /// </summary>
        public static Token Equals()
        {
            return new Token(TokenKind.Equals);
        }

        /// <summary>
        /// Creates an left bracket token.
        /// </summary>
        public static Token LeftBracket()
        {
            return new Token(TokenKind.LeftBracket);
        }

        /// <summary>
        /// Creates an right bracket token.
        /// </summary>
        public static Token RightBracket()
        {
            return new Token(TokenKind.RightBracket);
        }

        /// <summary>
        /// Creates a pipe (vertical line) token.
        /// </summary>
        public static Token Pipe()
        {
            return new Token(TokenKind.Pipe);
        }

        /// <summary>
        /// Creates an left plus token.
        /// </summary>
        public static Token Plus()
        {
            return new Token(TokenKind.Plus);
        }

        /// <summary>
        /// Creates an right greater token.
        /// </summary>
        public static Token Greater()
        {
            return new Token(TokenKind.Greater);
        }

        /// <summary>
        /// Creates an includes token.
        /// </summary>
        public static Token Includes()
        {
            return new Token(TokenKind.Includes);
        }

        /// <summary>
        /// Creates a dash-match token.
        /// </summary>
        public static Token DashMatch()
        {
            return new Token(TokenKind.DashMatch);
        }

        /// <summary>
        /// Creates a prefix-match token.
        /// </summary>
        public static Token PrefixMatch()
        {
            return new Token(TokenKind.PrefixMatch);
        }

        /// <summary>
        /// Creates a suffix-match token.
        /// </summary>
        public static Token SuffixMatch()
        {
            return new Token(TokenKind.SuffixMatch);
        }

        /// <summary>
        /// Creates a substring-match token.
        /// </summary>
        public static Token SubstringMatch()
        {
            return new Token(TokenKind.SubstringMatch);
        }

		/// <summary>
		/// Creates a general sibling token.
		/// </summary>
		public static Token Tilde()
		{
			return new Token(TokenKind.Tilde);
		}

        /// <summary>
        /// Creates an identifier token.
        /// </summary>
        public static Token Ident(string text)
        {
            ValidateTextArgument(text);
            return new Token(TokenKind.Ident, text);
        }

        /// <summary>
        /// Creates an integer token.
        /// </summary>
        public static Token Integer(string text)
        {
            ValidateTextArgument(text);
            return new Token(TokenKind.Integer, text);
        }

        /// <summary>
        /// Creates a hash-name token.
        /// </summary>
        public static Token Hash(string text)
        {
            ValidateTextArgument(text);
            return new Token(TokenKind.Hash, text);
        }

        /// <summary>
        /// Creates a white-space token.
        /// </summary>
        public static Token WhiteSpace(string space)
        {
            ValidateTextArgument(space);
            return new Token(TokenKind.WhiteSpace, space);
        }

        /// <summary>
        /// Creates a string token.
        /// </summary>
        public static Token String(string text)
        {
            return new Token(TokenKind.String, text ?? string.Empty);
        }

        /// <summary>
        /// Creates a function token.
        /// </summary>
        public static Token Function(string text)
        {
            ValidateTextArgument(text);
            return new Token(TokenKind.Function, text);
        }

        private static void ValidateTextArgument(string text)
        {
            if (text == null) throw new ArgumentNullException("text");
            if (text.Length == 0) throw new ArgumentException(null, "text");
        }

        /// <summary>
        /// Gets a string representation of the token.
        /// </summary>
        public override string ToString()
        {
            return Text == null ? Kind.ToString() : Kind + ": " + Text;
        }
    }
}