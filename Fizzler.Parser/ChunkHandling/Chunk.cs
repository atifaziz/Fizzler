namespace Fizzler.Parser.ChunkHandling
{
	/// <summary>
	/// A Chunk describers the part of the selector we are working with.
	/// </summary>
	public class Chunk
	{
		/// <summary>
		/// Create a new <see cref="Chunk">Chunk</see>.
		/// </summary>
		/// <param name="chunkType"></param>
		/// <param name="body"></param>
		/// <param name="descendantSelectionType"></param>
		/// <param name="pseudoclassData"></param>
		/// <param name="attributeSelectorData"></param>
		public Chunk(ChunkType chunkType, string body, DescendantSelectionType descendantSelectionType, string pseudoclassData, AttributeSelectorData attributeSelectorData)
		{
			AttributeSelectorData = attributeSelectorData;
			ChunkType = chunkType;
			Body = body;
			DescendantSelectionType = descendantSelectionType;
			PseudoclassData = pseudoclassData;
		}

		///<summary>
		/// If this Chunk is an attribute selector, this will contain information detailing it.
		///</summary>
		public AttributeSelectorData AttributeSelectorData { get; set; }

		/// <summary>
		/// The type of this Chunk.
		/// </summary>
		public ChunkType ChunkType { get; set; }

		/// <summary>
		/// The body of the selector Chunk.
		/// </summary>
		/// <remarks>e.g. for a tag selector, this would be the tag name.</remarks>
		public string Body { get; set; }

		/// <summary>
		/// The type of descendant selection for this Chunk.
		/// </summary>
		public DescendantSelectionType DescendantSelectionType { get; set; }

		/// <summary>
		/// Data about the pseudoclass used by this Chunk, if any.
		/// </summary>
		public string PseudoclassData { get; set; }
	}
}