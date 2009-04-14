namespace Fizzler.Parser.ChunkHandling
{
	/// <summary>
	/// Information regarding an attribute selector.
	/// </summary>
	public class AttributeSelectorData
	{
		/// <summary>
		/// The attribute being matched.
		/// </summary>
		public string Attribute { get; set; }

		/// <summary>
		/// The comparison method.
		/// </summary>
		public AttributeComparator Comparison { get; set; }

		/// <summary>
		/// Value to match against.
		/// </summary>
		public string Value { get; set; }
	}
}