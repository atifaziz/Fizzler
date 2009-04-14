using System.Collections.Generic;
using Fizzler.Parser.Document;
using HtmlAgilityPack;

namespace Fizzler.DocumentParsers.HtmlAgilityPack
{
	public static class HtmlNodeCollectionExtensions
	{
		public static List<IDocumentNode> ToList(this HtmlNodeCollection t)
		{
			List<IDocumentNode> list = new List<IDocumentNode>();

			foreach(var node in t)
			{
				list.Add((IDocumentNode) node);
			}

			return list;
		}
	}
}