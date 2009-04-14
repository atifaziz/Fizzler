namespace Fizzler.Parser.ChunkHandling
{
	/// <summary>
	/// Type of the Chunk.
	/// </summary>
	public enum ChunkType
	{
		/// <summary>
		/// i.e. a, img, div.
		/// </summary>
		TagName,

		/// <summary>
		/// * selector.
		/// </summary>
		Star,

		/// <summary>
		/// #someId selector.
		/// </summary>
		Id,

		/// <summary>
		/// .someClass selector.
		/// </summary>
		Class
	}
}