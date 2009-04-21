using System;
using System.Collections.Generic;
using Fizzler.Parser.ChunkHandling;
using Fizzler.Parser.Document;
using Fizzler.Parser.Extensions;
using Fizzler.Parser.Matchers;

namespace Fizzler.Parser
{
	/// <summary>
	/// SelectorEngine.
	/// </summary>
	public class SelectorEngine : ISelectorEngine
	{
		private readonly ChunkParser _chunkParser = new ChunkParser();
		private readonly IDocumentNode _scopeNode;
		private readonly NodeMatcher _nodeMatcher = new NodeMatcher();

		/// <summary>
		/// Empty constructor
		/// </summary>
		public SelectorEngine()
		{
		}

		/// <summary>
		/// Allows use of the Select(string) method by initialising the engine with a document.
		/// </summary>
		/// <param name="document"></param>
		public SelectorEngine(IDocumentNode document)
		{
			_scopeNode = document;
		}

		/// <summary>
		/// Select from the IDocument which was used to initialise the engine.
		/// </summary>
		/// <param name="selectorChain"></param>
		/// <returns></returns>
		public IEnumerable<IDocumentNode> Select(string selectorChain)
		{
			if(_scopeNode == null)
				throw new NullReferenceException("The engine scope node was null. Either pass it in via the SelectorEngine(IDocumentNode) constructor or use Select(IDocumentNode, string).");

			return Select(_scopeNode, selectorChain);
		}

		/// <summary>
		/// Select from the passed IDocument.
		/// </summary>
		/// <param name="scopeNode"></param>
		/// <param name="selectorChain"></param>
		/// <returns></returns>
		public IEnumerable<IDocumentNode> Select(IDocumentNode scopeNode, string selectorChain)
		{
            if (!scopeNode.IsElement)
                throw new ArgumentException("Node is not is an element.", "scopeNode");

			List<IDocumentNode> selectedNodes = new List<IDocumentNode>();

			string[] selectors = selectorChain.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

			// This enables us to support "," by simply treating comma-separated parts as separate selectors
			foreach(string rawSelector in selectors)
			{
				// we also need to check if a chunk contains a "." character....
				var chunks = _chunkParser.GetChunks(rawSelector.Trim());

                var list = new List<IDocumentNode> { scopeNode };

			    for(int chunkCounter = 0; chunkCounter < chunks.Count; chunkCounter++)
				{
					list = list.Flatten();
					list.RemoveAll(node => !_nodeMatcher.IsDownwardMatch(node, chunks, chunkCounter));
				}

				selectedNodes.AddRange(list);
			}

			return selectedNodes;
		}
	}
}