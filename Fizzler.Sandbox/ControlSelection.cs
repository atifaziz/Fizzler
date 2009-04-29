using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Fizzler.Systems.WinForms
{
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
