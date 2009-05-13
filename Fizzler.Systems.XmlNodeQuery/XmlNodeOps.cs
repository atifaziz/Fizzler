using System;
using System.Linq;
using System.Xml;

namespace Fizzler.Systems.XmlNodeQuery
{
	public class XmlNodeOps : IElementOps<XmlNode>
	{
		public virtual Selector<XmlNode> Type(string type)
		{
			return nodes => nodes.Where(n => n.Name == type);
		}

		public virtual Selector<XmlNode> Universal()
		{
			return nodes => nodes.Elements();
		}

		public virtual Selector<XmlNode> Id(string id)
		{
			return AttributeExact("id", id);
		}

		public virtual Selector<XmlNode> Class(string clazz)
		{
			return AttributeIncludes("class", clazz);
		}

		public virtual Selector<XmlNode> AttributeExists(string name)
		{
			return nodes => nodes.Elements().Where(n => n.Attributes[name] != null);
		}

		public virtual Selector<XmlNode> AttributeExact(string name, string value)
		{
			return nodes => from n in nodes.Elements()
			                let a = n.Attributes[name]
			                where a != null && a.Value == value
			                select n;
		}

		public virtual Selector<XmlNode> AttributeIncludes(string name, string value)
		{
			return nodes => from n in nodes.Elements()
			                let a = n.Attributes[name]
			                where a != null && a.Value.Split(' ').Contains(value)
			                select n;
		}

		public virtual Selector<XmlNode> AttributeDashMatch(string name, string value)
		{
			return string.IsNullOrEmpty(value)
			     ? (Selector<XmlNode>)(nodes => Enumerable.Empty<XmlNode>())
			     : (nodes => from n in nodes.Elements()
			                 let a = n.Attributes[name]
			                 where a != null && a.Value.Split('-').Contains(value)
			       		            select n);
		}

		public virtual Selector<XmlNode> AttributePrefixMatch(string name, string value)
		{
			return string.IsNullOrEmpty(value)
			     ? (Selector<XmlNode>)(nodes => Enumerable.Empty<XmlNode>())
			     : (nodes => from n in nodes.Elements()
			                 let a = n.Attributes[name]
			                 where a != null && a.Value.StartsWith(value)
			                 select n);
		}

		public virtual Selector<XmlNode> AttributeSuffixMatch(string name, string value)
		{
			return string.IsNullOrEmpty(value)
			     ? (Selector<XmlNode>)(nodes => Enumerable.Empty<XmlNode>())
			     : (nodes => from n in nodes.Elements()
			                 let a = n.Attributes[name]
			                 where a != null && a.Value.EndsWith(value)
			                 select n);
		}

		public virtual Selector<XmlNode> AttributeSubstring(string name, string value)
		{
			return string.IsNullOrEmpty(value)
			     ? (Selector<XmlNode>)(nodes => Enumerable.Empty<XmlNode>())
			     : (nodes => from n in nodes.Elements()
			                 let a = n.Attributes[name]
			                 where a != null && a.Value.Contains(value)
			                 select n);
		}

		public virtual Selector<XmlNode> FirstChild()
		{
			return nodes => nodes.Where(n => !n.ElementsBeforeSelf().Any());
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
		/// which represents an element that is the last child of some other element.
		/// </summary>
		public virtual Selector<XmlNode> LastChild()
		{
			return nodes => nodes.Where(n => n.ParentNode.NodeType != XmlNodeType.Document
			                              && !n.ElementsAfterSelf().Any());
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
		/// which represents an element that is the N-th child of some other element.
		/// </summary>
		public virtual Selector<XmlNode> NthChild(int a, int b)
		{
			if (a != 1)
				throw new NotSupportedException("The nth-child(an+b) selector where a in is not 1 are not supported.");

			return nodes => from n in nodes
			                let elements = n.ParentNode.Elements().Take(b).ToArray()
			                where elements.Length == b && elements.Last().Equals(n)
			                select n;
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
		/// which represents an element that has a parent element and whose parent 
		/// element has no other element children.
		/// </summary>
		public virtual Selector<XmlNode> OnlyChild()
		{
			return nodes => nodes.Where(n => n.ParentNode.NodeType != XmlNodeType.Document
			                              && !n.ElementsAfterSelf().Concat(n.ElementsBeforeSelf()).Any());
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
		/// which represents an element that has no children at all.
		/// </summary>
		public virtual Selector<XmlNode> Empty()
		{
			return nodes => nodes.Elements().Where(n => n.ChildNodes.Count == 0);
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
		/// which represents a childhood relationship between two elements.
		/// </summary>
		public virtual Selector<XmlNode> Child()
		{
			return nodes => nodes.SelectMany(n => n.Elements());
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
		/// which represents a relationship between two elements where one element is an 
		/// arbitrary descendant of some ancestor element.
		/// </summary>
		public virtual Selector<XmlNode> Descendant()
		{
			return nodes => nodes.SelectMany(n => n.Descendants().Elements());
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
		/// which represents elements that share the same parent in the document tree and 
		/// where the first element immediately precedes the second element.
		/// </summary>
		public virtual Selector<XmlNode> Adjacent()
		{
			return nodes => nodes.SelectMany(n => n.ElementsAfterSelf().Take(1));
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
		/// which separates two sequences of simple selectors. The elements represented
		/// by the two sequences share the same parent in the document tree and the
		/// element represented by the first sequence precedes (not necessarily
		/// immediately) the element represented by the second one.
		/// </summary>
		public virtual Selector<XmlNode> GeneralSibling()
		{
			return nodes => nodes.SelectMany(n => n.ElementsAfterSelf());
		}
	}
}