using System;
using System.Collections.Generic;
using Fizzle.Parser.Extensions;
using Fizzler.Parser.ChunkHandling;
using Fizzler.Parser.Document;
using Fizzler.Parser.Matchers;

namespace Fizzler.Parser
{
	public class SelectorEngine : ISelectorEngine
	{
		private readonly ChunkParser _chunkParser = new ChunkParser();
		private readonly NodeMatcher _nodeMatcher = new NodeMatcher();
        private readonly IDocument _document;

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
		public SelectorEngine(IDocument document)
		{
			_document = document;
		}

		/// <summary>
		/// Select from the IDocument which was used to initialise the engine.
		/// </summary>
		/// <param name="selectorChain"></param>
		/// <returns></returns>
		public IList<IDocumentNode> Select(string selectorChain)
		{
			if(_document == null)
				throw new NullReferenceException("The engine IDocument was null. Either pass it in via the SelectorEngine(IDocument) constructor or use Select(IDocument, string).");

			return Select(_document, selectorChain);
		}

		/// <summary>
		/// Select from the passed IDocument.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="selectorChain"></param>
		/// <returns></returns>
		public IList<IDocumentNode> Select(IDocument document, string selectorChain)
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