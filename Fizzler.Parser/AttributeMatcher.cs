using System;
using System.Collections.Generic;
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

				if (attributeSelectorData.Comparison == AttributeComparator.SpaceSeparated)
				{
					HtmlAttribute attribute = node.Attributes[attributeSelectorData.Attribute];

					if (attribute == null)
						return false;

					List<string> strings = new List<string>(attribute.Value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

					return strings.Contains(attributeSelectorData.Value);
				}

				if (attributeSelectorData.Comparison == AttributeComparator.HyphenSeparated)
				{
					HtmlAttribute attribute = node.Attributes[attributeSelectorData.Attribute];

					if (attribute == null)
						return false;

					List<string> strings = new List<string>(attribute.Value.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

					return strings.Contains(attributeSelectorData.Value);
				}
			}
		
			return true;
		}
	}
}