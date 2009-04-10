using System.Collections.Generic;

namespace Fizzler.Parser.Document
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