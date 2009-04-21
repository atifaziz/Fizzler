using System.Collections.Generic;
using Fizzler.Parser.Document;

namespace Fizzler.Parser
{
    /// <summary>
	/// SelectorEngine.
	/// </summary>
	public interface ISelectorEngine
    {
        /// <summary>
		/// Select from the passed IDocument.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="selectorChain"></param>
		/// <returns></returns>
		IEnumerable<IDocumentNode> Select(IDocumentNode node, string selectorChain);

        /// <summary>
        /// Create an object that can be selected using this engine given a document node.
        /// </summary>
        /// <param name="node"></param>
        ISelectable ToSelectable(IDocumentNode node);
    }
}