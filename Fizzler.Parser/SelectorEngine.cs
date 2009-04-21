using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		private readonly NodeMatcher _nodeMatcher = new NodeMatcher();

	    /// <summary>
		/// Create an object that can be selected using this engine given a document node.
		/// </summary>
		/// <param name="node"></param>
		public ISelectable ToSelectable(IDocumentNode node)
		{
		    if (node == null) 
                throw new ArgumentNullException("node");
            if (!node.IsElement) 
                throw new ArgumentException("Node is not is an element.", "node");

		    return new SelectableDocumentNode(node, this);
		}

		/// <summary>
		/// Select from the passed node.
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

	    private sealed class SelectableDocumentNode : ISelectable
        {
            private readonly IDocumentNode _node;
            private readonly ISelectorEngine _engine;

            public SelectableDocumentNode(IDocumentNode node, ISelectorEngine engine)
            {
                Debug.Assert(node != null);
                Debug.Assert(engine != null);
                _node = node;
                _engine = engine;
            }

            public IEnumerable<IDocumentNode> Select(string selectorChain)
            {
                return _engine.Select(_node, selectorChain);
            }
        }
	}
}