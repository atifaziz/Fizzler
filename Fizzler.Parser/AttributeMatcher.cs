using Fizzler.Parser.ChunkHandling;
using HtmlAgilityPack;

namespace Fizzler.Parser
{
	public class AttributeMatcher
	{
		public bool Match(AttributeSelectorData attributeSelectorData, HtmlNode node)
		{
			if(attributeSelectorData != null)
			{
				return node.Attributes[attributeSelectorData.Attribute] == null;
			}
		
			return true;
		}
	}
}