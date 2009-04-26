using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        /// <param name="node"></param>
		/// <param name="selectorChain"></param>
		/// <returns></returns>
		public IEnumerable<IDocumentNode> Select(IDocumentNode node, string selectorChain)
		{
		    if (node == null) 
                throw new ArgumentNullException("node");
		    if (!node.IsElement)
                throw new ArgumentException("Node is not is an element.", "node");

            var filters = selectorChain.Split(',')                          // tokenize
                                       .Select(s => s.Trim())               // compress
                                       .Where(s => s.Length > 0)            // none empty, please
                                       .Select(s => SelectorToFilter(s));   // compile
		    
            return SelectImpl(Enumerable.Repeat(node, 1), filters);
		}

	    private static IEnumerable<IDocumentNode> SelectImpl(IEnumerable<IDocumentNode> nodes, 
            IEnumerable<Func<IEnumerable<IDocumentNode>, IEnumerable<IDocumentNode>>> selectors)
	    {
            Debug.Assert(nodes != null);
            Debug.Assert(selectors != null);

            foreach (var selector in selectors)
	        {
	            foreach (var selection in selector(nodes))
	                yield return selection;
	        }
	    }

	    private Func<IEnumerable<IDocumentNode>, IEnumerable<IDocumentNode>> SelectorToFilter(string selector)
        {
            Debug.Assert(selector != null);
            Debug.Assert(selector.Length > 0);

            // we also need to check if a chunk contains a "." character....
            var chunks = _chunkParser.GetChunks(selector);
            var filters = chunks.Select((c, i) => ChunkToFilter(chunks, i));
            return selections =>
            {
                foreach (var filter in filters)
                {
                    selections = selections.SelectMany(s => s.DescendantsAndSelf())
                                           .Distinct()
                                           .Where(filter);
                }
                return selections;
            };
        }

	    private Func<IDocumentNode, bool> ChunkToFilter(IList<Chunk> chunks, int index)
	    {
            Debug.Assert(chunks != null);
            Debug.Assert(index >= 0);
            Debug.Assert(index < chunks.Count);
            return node => _nodeMatcher.IsDownwardMatch(node, chunks, index);
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