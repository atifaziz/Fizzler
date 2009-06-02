namespace Fizzler
{
    /// <summary>
    /// Represents the classification of a token.
    /// </summary>
    public enum TokenKind
    {
        /// <summary>
        /// Represents end of input/file/stream
        /// </summary>
        Eoi,

        /// <summary>
        /// Represents {ident}
        /// </summary>
        Ident,

        /// <summary>
        /// Represents "#" {name}
        /// </summary>
        Hash,

        /// <summary>
        /// Represents "~="
        /// </summary>
        Includes,

        /// <summary>
        /// Represents "|="
        /// </summary>
        DashMatch,

        /// <summary>
        /// Represents "^="
        /// </summary>
        PrefixMatch,

        /// <summary>
        /// Represents "$="
        /// </summary>
        SuffixMatch,

        /// <summary>
        /// Represents "*="
        /// </summary>
        SubstringMatch,
        
        /// <summary>
        /// Represents {string}
        /// </summary>
        String,

        /// <summary>
        /// Represents "*"
        /// </summary>
        Star,

        /// <summary>
        /// Represents "."
        /// </summary>
        Dot,

        /// <summary>
        /// Represents ":"
        /// </summary>
        Colon,

        /// <summary>
        /// Represents ","
        /// </summary>
        Comma,

        /// <summary>
        /// Represents "="
        /// </summary>
        Equals,

        /// <summary>
        /// Represents "["
        /// </summary>
        LeftBracket,

        /// <summary>
        /// Represents "]"
        /// </summary>
        RightBracket,

        /// <summary>
        /// Represents "|"
        /// </summary>
        Pipe,

        /// <summary>
        /// Represents S* "+"
        /// </summary>
        Plus,

        /// <summary>
        /// Represents S* ">"
        /// </summary>
        Greater,

        /// <summary>
        /// Represents [ \t\r\n\f]+
        /// </summary>
        WhiteSpace,

        /// <summary>
        /// Represents {ident} ")"
        /// </summary>
        Function,

        /// <summary>
        /// Represents )
        /// </summary>
        RightParenthesis,

        /// <summary>
        /// Represents [0-9]+
        /// </summary>
        Integer,

		/// <summary>
		/// Represents S* "~"
		/// </summary>
		Tilde,
    }
}