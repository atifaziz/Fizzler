using System.Collections.Generic;

namespace Fizzler.Parser.Extensions
{
	public static class StringListExtensions
	{
		public static bool ContainsAll(this List<string> items, List<string> others)
		{
			foreach (var s in others)
			{
				if(!items.Contains(s))
					return false;
			}

			return true;
		}
	}
}