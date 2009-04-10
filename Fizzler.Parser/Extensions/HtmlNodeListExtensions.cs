using System.Collections.Generic;
using HtmlAgilityPack;
using Fizzler.Parser;

namespace Fizzle.Parser.Extensions
{
	public static class HtmlNodeListExtensions
	{
        public static List<IDocumentNode> Flatten(this List<IDocumentNode> nodes)
		{
            var list = new List<IDocumentNode>();

			foreach (IDocumentNode node in nodes)
			{
                if (!list.Contains(node))
                    list.Add(node);

				list.AddRange(Flatten(node.ChildNodes));
			}

			return list;
		}
	}
}