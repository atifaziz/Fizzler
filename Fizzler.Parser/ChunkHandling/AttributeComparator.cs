namespace Fizzler.Parser.ChunkHandling
{
	/// <summary>
	/// Method by which an attribute is compared.
	/// </summary>
	/// <remarks>See 5.8.1 of the CSS2.1 spec.</remarks>
	public enum AttributeComparator
	{
		/// <summary>
		/// Unknown comparison method.
		/// </summary>
		Unknown,

		/// <summary>
		/// Aka "=" comparison.
		/// </summary>
		Exact,

		/// <summary>
		/// Aka "~" comparison
		/// </summary>
		SpaceSeparated,

		/// <summary>
		/// Aka "|" comparison.
		/// </summary>
		HyphenSeparated
	}
}