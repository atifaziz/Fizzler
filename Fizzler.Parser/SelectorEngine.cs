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
		private readonly ChunkParser _chunkParser = new ChunkParser();
		private readonly NodeMatcher _nodeMatcher = new NodeMatcher();
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
			List<HtmlNode> selectedNodes = new List<HtmlNode>();

			string[] selectors = selectorChain.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			
			// This enables us to support "," by simply treating comma-separated parts as separate selectors
			foreach (string rawSelector in selectors)
			{
				// we also need to check if a chunk contains a "." character....
				var chunks = _chunkParser.GetChunks(rawSelector.Trim());

				List<HtmlNode> list = documentNode.ChildNodes.ToList();

				for (int chunkCounter = 0; chunkCounter < chunks.Count; chunkCounter++)
				{
					Chunk chunk = chunks[chunkCounter];
					Chunk previousChunk = chunkCounter > 0 ? chunks[chunkCounter - 1] : null;

					list = list.Flatten();
					
					list.RemoveAll(node => !_nodeMatcher.IsDownwardMatch(node, chunk, previousChunk));
				}

				selectedNodes.AddRange(list);
			}

			return selectedNodes;
		}
	}
}