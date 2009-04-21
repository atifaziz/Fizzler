using System.Collections.Generic;

namespace Fizzler.Parser.Document
{
	/// <summary>
	/// A node within the tree which the SelectorEngine operates upon
	/// </summary>
	public interface IDocumentNode
	{
		/// <summary>
		/// The attributes of this node
		/// </summary>
		IAttributeCollection Attributes { get; }

		/// <summary>
		/// This nodes children
		/// </summary>
		IList<IDocumentNode> ChildNodes { get; }

		/// <summary>
		/// This nodes parent
		/// </summary>
		IDocumentNode ParentNode { get; }

		/// <summary>
		/// This nodes previous sibling
		/// </summary>
		IDocumentNode PreviousSibling { get; }

		/// <summary>
		/// The ID of this node
		/// </summary>
		string Id { get; }

		/// <summary>
		/// The class of this node
		/// </summary>
		string Class { get; }

		/// <summary>
		/// The name of this node
		/// </summary>
		string Name { get; }

		/// <summary>
		/// The inner text of this node
		/// </summary>
		string InnerText { get; }

		/// <summary>
		/// Determines whether this node is an element
		/// </summary>
		bool IsElement { get; }
	}
}