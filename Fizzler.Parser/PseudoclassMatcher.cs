using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace Fizzler.Parser
{
	public class PseudoclassMatcher
	{
		public bool Match(string pseudoclassData, HtmlNode node)
		{
			if (!string.IsNullOrEmpty(pseudoclassData))
			{
				if (pseudoclassData.Contains("nth-child"))
					return NthChild(pseudoclassData, node);

			}

			return true;
		}

		private bool NthChild(string pseudoclassData, HtmlNode node)
		{
			int digit = Convert.ToInt32(pseudoclassData.Replace("nth-child(", string.Empty).Replace(")", string.Empty)) - 1;
		
			List<HtmlNode> nodes = new List<HtmlNode>();

			foreach (var sibling in node.ParentNode.ChildNodes)
			{
				if(sibling.NodeType == HtmlNodeType.Element)
				{
					nodes.Add(sibling);
				}
			}

			int foundAt = -1;

			for (int i = 0; i < nodes.Count; i++)
			{
				if(nodes[i] == node)
				{
					foundAt = i;
				}
			}
			
			if(foundAt == digit)
			{
				return true;
			}

			return false;
		}
	}
}