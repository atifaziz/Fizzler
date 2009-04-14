namespace Fizzler.Parser.Document
{
	/// <summary>
	/// An attribute
	/// </summary>
	public interface IAttribute
	{
		///<summary>
		/// Value of this attribute.
		///</summary>
		string Value { get; }
	}
}