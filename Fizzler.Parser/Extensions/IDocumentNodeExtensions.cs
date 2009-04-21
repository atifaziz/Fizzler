using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fizzler.Parser.Document;

namespace Fizzler.Parser.Extensions
{
	/// <summary>
	/// HtmlNodeList extension methods.
	/// </summary>
    public static class IDocumentNodeExtensions
	{
        /// <summary>
        /// Returns a collections of child elements of this node.
        /// </summary>
        public static IEnumerable<IDocumentNode> Elements(this IDocumentNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return node.ChildNodes.Where(child => child.IsElement);
        }

        /// <summary>
        /// Returns a collection of nodes that contains this element 
        /// followed by all descendant nodes of this element.
        /// </summary>
        public static IEnumerable<IDocumentNode> DescendantsAndSelf(this IDocumentNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return DescendantsAndSelfImpl(node);
        }

	    private static IEnumerable<IDocumentNode> DescendantsAndSelfImpl(IDocumentNode node)
	    {
            Debug.Assert(node != null);
	        return Enumerable.Repeat(node, 1).Concat(node.Descendants());
	    }

        /// <summary>
        /// Returns a collection of all descendant nodes of this element.
        /// </summary>
        public static IEnumerable<IDocumentNode> Descendants(this IDocumentNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            return DescendantsImpl(node);
        }

	    private static IEnumerable<IDocumentNode> DescendantsImpl(IDocumentNode node)
	    {
            Debug.Assert(node != null);
            foreach (var child in node.ChildNodes)
	        {
	            yield return child;
	            foreach (var descendant in child.Descendants())
	                yield return descendant;
	        }
	    }
	}
}