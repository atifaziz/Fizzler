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
				if (attributeSelectorData.Comparison == AttributeComparator.Unknown)
				{
					return node.Attributes[attributeSelectorData.Attribute] == null;
				}
				if(attributeSelectorData.Comparison == AttributeComparator.Exact)
				{
					HtmlAttribute attribute = node.Attributes[attributeSelectorData.Attribute];
				
					if(attribute == null)
						return false;

					return attribute.Value == attributeSelectorData.Value;
					
				}
			}
		
			return true;
		}
	}
}