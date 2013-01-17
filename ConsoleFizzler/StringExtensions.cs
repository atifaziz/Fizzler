namespace ConsoleFizzler
{
    #region Imports

    using System.Globalization;

    #endregion

    static class StringExtensions
    {
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