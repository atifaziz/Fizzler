using System.Collections.Generic;
using Fizzler.Parser.Document;

namespace Fizzler.Parser
{
    /// <summary>
    /// Represents an object that can be selected from.
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Select from the node.
        /// </summary>
        IEnumerable<IDocumentNode> Select(string selectorChain);
    }
}