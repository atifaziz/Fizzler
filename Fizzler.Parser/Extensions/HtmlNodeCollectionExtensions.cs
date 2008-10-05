using System.Collections.Generic;
using HtmlAgilityPack;

namespace Fizzle.Parser.Extensions
{
	public static class HtmlNodeCollectionExtensions
	{
		public static List<HtmlNode> ToList(this HtmlNodeCollection t)
		{
			List<HtmlNode> list = new List<HtmlNode>();

			foreach (var node in t)
			{
				list.Add(node);
			}

			return list;
		}
	}
}