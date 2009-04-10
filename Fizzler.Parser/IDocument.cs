using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fizzler.Parser
{
    /// <summary>
    /// A document contains a number of nodes.
    /// </summary>
    public interface IDocument
    {
        /// <summary>
        /// The nodes which this document contains
        /// </summary>
        List<IDocumentNode> ChildNodes { get; }
    }
}
