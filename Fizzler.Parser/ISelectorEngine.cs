using System;
using System.Collections;
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

    /// <summary>FIXDOC</summary>
    public interface ISelectorEngine2
    {
        /// <summary>FIXDOC</summary>
        ISelectorGenerator CreateGenerator();
        /// <summary>FIXDOC</summary>
        object GetSelector(ISelectorGenerator generator);
        /// <summary>FIXDOC</summary>
        IEnumerable Select(object context, object selector);
    }
}