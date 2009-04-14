using System.Collections.Generic;
using Fizzler.Parser.Document;

namespace Fizzler.Parser.Extensions
{
	/// <summary>
	/// HtmlNodeList extension methods.
	/// </summary>
	public static class HtmlNodeListExtensions
	{
		/// <summary>
		/// Flatten a tree of nodes into a list.
		/// </summary>
		/// <param name="nodes"></param>
		/// <returns></returns>
		public static List<IDocumentNode> Flatten(this List<IDocumentNode> nodes)
		{
			var list = new List<IDocumentNode>();

			foreach(IDocumentNode node in nodes)
			{
				if(!list.Contains(node))
					list.Add(node);

				list.AddRange(Flatten(node.ChildNodes));
			}

			return list;
		}
	}
}