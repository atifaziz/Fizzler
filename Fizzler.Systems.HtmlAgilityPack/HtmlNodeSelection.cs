using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Fizzler.Systems.HtmlAgilityPack
{
    /// <summary>
    /// Selector API for <see cref="HtmlNode"/>.
    /// </summary>
    /// <remarks>
    /// For more information, see <a href="http://www.w3.org/TR/selectors-api/">Selectors API</a>.
    /// </remarks>
    public static class HtmlNodeSelection
    {
        /// <summary>
        /// Similar to <see cref="QuerySelectorAll" /> except it returns 
        /// only the first element matching the supplied selector strings.
        /// </summary>
        public static HtmlNode QuerySelector(this HtmlNode node, string selector)
        {
            return node.QuerySelectorAll(selector).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all element nodes from descendants of the starting 
        /// element node that match any selector within the supplied 
        /// selector strings. 
        /// </summary>
        public static IEnumerable<HtmlNode> QuerySelectorAll(this HtmlNode node, string selector)
        {
            var generator = new SelectorGenerator<HtmlNode>(new HtmlNodeOps());
            Parser.Parse(selector, generator);
            return generator.Selector(Enumerable.Repeat(node, 1));
        }
    }
}
