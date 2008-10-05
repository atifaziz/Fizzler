using System.Collections.Generic;
using HtmlAgilityPack;

namespace Fizzle.Parser.Extensions
{
	public static class HtmlNodeListExtensions
	{
		public static List<HtmlNode> Flatten(this List<HtmlNode> nodes)
		{
			var list = new List<HtmlNode>();

			foreach (var htmlNode in nodes)
			{
				if (!list.Contains(htmlNode))
					list.Add(htmlNode);

				list.AddRange(Flatten(htmlNode.ChildNodes.ToList()));
			}

			return list;
		}
	}
}