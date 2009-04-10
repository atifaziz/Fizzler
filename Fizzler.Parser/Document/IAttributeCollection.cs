namespace Fizzler.Parser.Document
{
	/// <summary>
	/// A collection of attributes
	/// </summary>
	public interface IAttributeCollection
	{
		IAttribute this[string name] { get; }
	}
}