namespace ConsoleFizzler
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Globalization;

    #endregion

    static class StringExtensions
    {
        /// <summary>
        /// Splits a string into a key and a value part using a specified 
        /// character to separate the two.
        /// </summary>
        /// <remarks>
        /// The key or value of the resulting pair is never <c>null</c>.
        /// </remarks>

        public static KeyValuePair<string, string> SplitPair(this string str, char separator)
        {
            if (str == null) throw new ArgumentNullException("str");

            return SplitPair(str, str.IndexOf(separator), 1);
        }

        /// <summary>
        /// Splits a string into a key and a value part using any of a 
        /// specified set of characters to separate the two.
        /// </summary>
        /// <remarks>
        /// The key or value of the resulting pair is never <c>null</c>.
        /// </remarks>

        public static KeyValuePair<string, string> SplitPair(this string str, params char[] separators)
        {
            if (str == null) throw new ArgumentNullException("str");

            return separators == null || separators.Length == 0
                           ? new KeyValuePair<string, string>(str, string.Empty)
                           : SplitPair(str, str.IndexOfAny(separators), 1);
        }

        /// <summary>
        /// Splits a string into a key and a value part by removing a
        /// portion of the string.
        /// </summary>
        /// <remarks>
        /// The key or value of the resulting pair is never <c>null</c>.
        /// </remarks>

        public static KeyValuePair<string, string> SplitPair(this string str, int index, int count)
        {
            if (str == null) throw new ArgumentNullException("str");
            if (count <= 0) throw new ArgumentOutOfRangeException("count", count, null);

            return new KeyValuePair<string, string>(
                    /* key   */ index < 0 ? str : str.Substring(0, index),
                                /* value */ index < 0 || index + 1 >= str.Length
                                                    ? string.Empty
                                                    : str.Substring(index + count));
        }

        /// <summary>
        /// Converts the specified string to titlecase using the invariant culture. 
        /// </summary>

        public static string ToTitleCaseInvariant(this string str)
        {
            return ToTitleCase(str, CultureInfo.InvariantCulture.TextInfo);
        }

        /// <summary>
        /// Converts the specified string to titlecase. 
        /// </summary>

        public static string ToTitleCase(this string str, TextInfo info)
        {
            return (info ?? CultureInfo.CurrentCulture.TextInfo).ToTitleCase(str);
        }
    }
}