namespace Fizzler.Parser.Document
{
	/// <summary>
	/// A collection of attributes
	/// </summary>
	public interface IAttributeCollection
	{
		///<summary>
		/// Get an attribute by name.
		///</summary>
		///<param name="name"></param>
		IAttribute this[string name] { get; }
	}
}