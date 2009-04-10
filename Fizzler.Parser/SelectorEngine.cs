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
        private IDocument _document;

        public SelectorEngine()
        {
        }

		public SelectorEngine(IDocument document)
		{
			_document = document;
		}

        public IList<IDocumentNode> Parse(string selectorChain)
		{
            List<IDocumentNode> selectedNodes = new List<IDocumentNode>();

			string[] selectors = selectorChain.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			
			// This enables us to support "," by simply treating comma-separated parts as separate selectors
			foreach (string rawSelector in selectors)
			{
				// we also need to check if a chunk contains a "." character....
				var chunks = _chunkParser.GetChunks(rawSelector.Trim());

                List<IDocumentNode> list = _document.ChildNodes;

				for (int chunkCounter = 0; chunkCounter < chunks.Count; chunkCounter++)
				{

					list = list.Flatten();

					list.RemoveAll(node => !_nodeMatcher.IsDownwardMatch( node, chunks, chunkCounter));
				}

				selectedNodes.AddRange(list);
			}

			return selectedNodes;
		}
	}
}