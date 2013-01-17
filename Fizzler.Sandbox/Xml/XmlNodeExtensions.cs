namespace Fizzler.Systems.XmlNodeQuery
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml;

    #endregion
    
    public static class XmlNodeExtensions
    {
        /// <summary>
        /// Determines whether this node is an element or not.
        /// </summary>
        public static bool IsElement(this XmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return node.NodeType == XmlNodeType.Element;
        }

        /// <summary>
        /// Returns a collection of elements from this collection.
        /// </summary>
        public static IEnumerable<XmlNode> Elements(this IEnumerable<XmlNode> nodes)
        {
            if (nodes == null) throw new ArgumentNullException("nodes");
            return nodes.Where(n => n.IsElement());
        }

        /// <summary>
        /// Returns a collection of child nodes of this node.
        /// </summary>
        public static IEnumerable<XmlNode> Children(this XmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return node.ChildNodes.Cast<XmlNode>();
        }


        /// <summary>
        /// Returns a collection of child elements of this node.
        /// </summary>
        public static IEnumerable<XmlNode> Elements(this XmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return node.Children().Elements();
        }

        /// <summary>
        /// Returns a collection of the sibling elements after this node.
        /// </summary>
        public static IEnumerable<XmlNode> ElementsAfterSelf(this XmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return node.NodesAfterSelf().Elements();
        }

        /// <summary>
        /// Returns a collection of the sibling nodes after this node.
        /// </summary>
        public static IEnumerable<XmlNode> NodesAfterSelf(this XmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return NodesAfterSelfImpl(node);
        }

        private static IEnumerable<XmlNode> NodesAfterSelfImpl(XmlNode node)
        {
            while ((node = node.NextSibling) != null)
                yield return node;
        }

        /// <summary>
        /// Returns a collection of the sibling elements before this node.
        /// </summary>
        public static IEnumerable<XmlNode> ElementsBeforeSelf(this XmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return node.NodesBeforeSelf().Elements();
        }

        /// <summary>
        /// Returns a collection of the sibling nodes before this node.
        /// </summary>
        public static IEnumerable<XmlNode> NodesBeforeSelf(this XmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return NodesBeforeSelfImpl(node);
        }

        private static IEnumerable<XmlNode> NodesBeforeSelfImpl(XmlNode node)
        {
            while ((node = node.PreviousSibling) != null)
                yield return node;
        }

        /// <summary>
        /// Returns a collection of all descendant nodes of this element.
        /// </summary>
        public static IEnumerable<XmlNode> Descendants(this XmlNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return DescendantsImpl(node);
        }

        private static IEnumerable<XmlNode> DescendantsImpl(XmlNode node)
        {
            Debug.Assert(node != null);
            foreach (XmlNode child in node.ChildNodes)
            {
                yield return child;
                foreach (var descendant in child.Descendants())
                    yield return descendant;
            }
        }
    }
}