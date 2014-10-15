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