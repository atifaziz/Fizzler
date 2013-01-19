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

namespace Fizzler.Systems.WinForms
{
    #region Imports

    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// Selectors API for <see cref="Control"/>.
    /// </summary>
    /// <remarks>
    /// For more information, see <a href="http://www.w3.org/TR/selectors-api/">Selectors API</a>.
    /// </remarks>
    public static class ControlSelection
    {
        /// <summary>
        /// Similar to <see cref="QuerySelectorAll" /> except it returns 
        /// only the first control matching the supplied selector strings.
        /// </summary>
        public static Control QuerySelector(this Control control, string selector)
        {
            return Enumerable.FirstOrDefault(control.QuerySelectorAll(selector));
        }

        /// <summary>
        /// Retrieves all controls from descendants of the starting control 
        /// that match any selector within the supplied selector strings. 
        /// </summary>
        public static IEnumerable<Control> QuerySelectorAll(this Control control, string selector)
        {
            var generator = new SelectorGenerator<Control>(new ControlOps());
            Parser.Parse(selector, generator);
            return generator.Selector(Enumerable.Repeat(control, 1));
        }
    }
}
