using System.Collections.Generic;

namespace Fizzler
{
    /// <summary>
    /// Represents a selector implementation over an arbitrary type of nodes.
    /// </summary>
    public delegate IEnumerable<TNode> Selector<TNode>(IEnumerable<TNode> nodes);
}