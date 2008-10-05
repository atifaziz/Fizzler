using System;
using System.Collections.Generic;
using Fizzle.Parser.Extensions;
using Fizzler.Parser;
using Fizzler.Parser.ChunkHandling;
using HtmlAgilityPack;

namespace Fizzler.Parser
{
	public class SelectorEngine
	{
		private NodeMatcher _nodeMatcher = new NodeMatcher();
		private readonly string _html;

		public SelectorEngine(string html)
		{
			_html = html;
		}

		private HtmlNode GetDocumentNode()
		{
			var document = new HtmlDocument();
			document.LoadHtml(_html);

			return document.DocumentNode;
		}

		public IList<HtmlNode> Parse(string selectorChain)
		{
			HtmlNode documentNode = GetDocumentNode();
			List<HtmlNode> data = new List<HtmlNode>();

			var selectors = selectorChain.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			
			// This enables us to support "," by simply treating comma-separated parts as separate selectors
			foreach (string rawSelector in selectors)
			{
				// we also need to check if a chunk contains a "." character....
				var chunks = new ChunkParser().GetChunks(rawSelector.Trim());

				List<HtmlNode> list = documentNode.ChildNodes.ToList();

				for (int i1 = 0; i1 < chunks.Count; i1++)
				{
					var chunk = chunks[i1];
					var previousChunk = i1 > 0 ? chunks[i1 - 1] : null;

					list = list.Flatten();
					IList<HtmlNode> remove = new List<HtmlNode>();
					IList<HtmlNode> keep = new List<HtmlNode>();

					foreach (var node in list)
					{
						if (!_nodeMatcher.IsMatch(node, chunk, previousChunk))
						{
							remove.Add(node);
						}
						else
						{
							keep.Add(node);
						}
					}

					for (int i = 0; i < remove.Count; i++)
					{
						var node = remove[i];

						foreach (var htmlNode in list)
						{
							if (htmlNode.ParentNode == node && !keep.Contains(htmlNode) && !remove.Contains(htmlNode))
							{
								remove.Add(htmlNode);
							}
						}
					}

					foreach (var node in remove)
					{
						list.Remove(node);
					}

					remove.Clear();
				}

				data.AddRange(list);
			}

			return data;
		}
	}
}