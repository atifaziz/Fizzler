using System.Collections.Generic;

namespace Fizzler.Parser.Extensions
{
	///<summary>
	/// Extension methods for List&gt;string&lt;.
	///</summary>
	public static class StringListExtensions
	{
		/// <summary>
		/// Does this list contain all of the items in the other list?
		/// </summary>
		/// <param name="items"></param>
		/// <param name="others"></param>
		/// <returns>true if all items match.</returns>
		public static bool ContainsAll(this List<string> items, List<string> others)
		{
			foreach(var s in others)
			{
				if(!items.Contains(s))
					return false;
			}

			return true;
		}
	}
}