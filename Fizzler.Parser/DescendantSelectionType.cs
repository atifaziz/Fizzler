namespace Fizzler.Parser
{
	public enum DescendantSelectionType
	{
		/// <summary>
		/// Any descendant, i.e. the " " selector.
		/// </summary>
		Descendant,
		
		/// <summary>
		/// Direct children, i.e. the ">" selector.
		/// </summary>
		Children,
		
		/// <summary>
		/// Only elements immediately next to this one, i.e. the + selector.
		/// </summary>
		Adjacent,
		
		/// <summary>
		/// Tip means the last selector in the sequence, therefore no descendant type is applicable.
		/// </summary>
		LastSelector
	}
}