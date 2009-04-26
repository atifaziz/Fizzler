using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Fizzler.DocumentParsers.HtmlAgilityPack
{
    /// <summary>
    /// HtmlNode extension methods.
    /// </summary>
    public static class HtmlNodeExtensions
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
            var generator = new HtmlNodeSelectorGenerator();
            Parser.Parse(selector, generator);
            return generator.Selector(Enumerable.Repeat(node, 1));
        }

        /// <summary>
        /// Determines whether this node is an element or not.
        /// </summary>
        public static bool IsElement(this HtmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return node.NodeType == HtmlNodeType.Element;
        }

        /// <summary>
        /// Returns a collection of elements from this collection.
        /// </summary>
        public static IEnumerable<HtmlNode> Elements(this IEnumerable<HtmlNode> nodes)
        {
            if (nodes == null) throw new ArgumentNullException("nodes");
            return nodes.Where(n => n.IsElement());
        }

        /// <summary>
        /// Returns a collection of child nodes of this node.
        /// </summary>
        public static IEnumerable<HtmlNode> Children(this HtmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return node.ChildNodes.Cast<HtmlNode>();
        }

        /// <summary>
        /// Returns a collection of child elements of this node.
        /// </summary>
        public static IEnumerable<HtmlNode> Elements(this HtmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return node.Children().Elements();
        }

        /// <summary>
        /// Returns a collection of the sibling elements after this node.
        /// </summary>
        public static IEnumerable<HtmlNode> ElementsAfterSelf(this HtmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return node.NodesAfterSelf().Elements();
        }

        /// <summary>
        /// Returns a collection of the sibling nodes after this node.
        /// </summary>
        public static IEnumerable<HtmlNode> NodesAfterSelf(this HtmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return NodesAfterSelfImpl(node);
        }

        private static IEnumerable<HtmlNode> NodesAfterSelfImpl(HtmlNode node)
        {
            while ((node = node.NextSibling) != null)
                yield return node;
        }

        /// <summary>
        /// Returns a collection of the sibling elements before this node.
        /// </summary>
        public static IEnumerable<HtmlNode> ElementsBeforeSelf(this HtmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return node.NodesBeforeSelf().Elements();
        }

        /// <summary>
        /// Returns a collection of the sibling nodes before this node.
        /// </summary>
        public static IEnumerable<HtmlNode> NodesBeforeSelf(this HtmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return NodesBeforeSelfImpl(node);
        }

        private static IEnumerable<HtmlNode> NodesBeforeSelfImpl(HtmlNode node)
        {
            while ((node = node.PreviousSibling) != null)
                yield return node;
        }

        /// <summary>
        /// Returns a collection of nodes that contains this element 
        /// followed by all descendant nodes of this element.
        /// </summary>
        public static IEnumerable<HtmlNode> DescendantsAndSelf(this HtmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return DescendantsAndSelfImpl(node);
        }

        private static IEnumerable<HtmlNode> DescendantsAndSelfImpl(HtmlNode node)
        {
            Debug.Assert(node != null);
            return Enumerable.Repeat(node, 1).Concat(node.Descendants());
        }

        /// <summary>
        /// Returns a collection of all descendant nodes of this element.
        /// </summary>
        public static IEnumerable<HtmlNode> Descendants(this HtmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return DescendantsImpl(node);
        }

        private static IEnumerable<HtmlNode> DescendantsImpl(HtmlNode node)
        {
            Debug.Assert(node != null);
            foreach (var child in node.ChildNodes)
            {
                yield return child;
                foreach (var descendant in child.Descendants())
                    yield return descendant;
            }
        }

        /// <summary>
        /// Returns a begin tag, including attributes, string 
        /// representation of this element.
        /// </summary>
        public static string GetBeginTagString(this HtmlNode node)
        {
            if(node == null) throw new ArgumentNullException("node");

            if (!node.IsElement())
                return string.Empty;

            var sb = new StringBuilder().Append('<').Append(node.Name);

            foreach (var attribute in node.Attributes)
            {
                sb.Append(' ')
                  .Append(attribute.Name)
                  .Append("=\"")
                  .Append(HtmlDocument.HtmlEncode(attribute.Value))
                  .Append('\"');
            }

            return sb.Append('>').ToString();
        }
    }
}