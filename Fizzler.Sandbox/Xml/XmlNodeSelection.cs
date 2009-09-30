using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Fizzler.Systems.XmlNodeQuery
{
	public static class XmlNodeSelection
	{
		/// <summary>
		/// Similar to <see cref="QuerySelectorAll" /> except it returns 
		/// only the first element matching the supplied selector strings.
		/// </summary>
		public static XmlNode QuerySelector(this XmlNode node, string selector)
		{
			return node.QuerySelectorAll(selector).FirstOrDefault();
		}

		/// <summary>
		/// Retrieves all element nodes from descendants of the starting 
		/// element node that match any selector within the supplied 
		/// selector strings. 
		/// </summary>
		public static IEnumerable<XmlNode> QuerySelectorAll(this XmlNode node, string selector)
		{
			var generator = new SelectorGenerator<XmlNode>(new XmlNodeOps());
			Parser.Parse(selector, generator);
			return generator.Selector(Enumerable.Repeat(node, 1));
		}
	}
}