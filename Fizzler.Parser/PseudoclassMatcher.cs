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
				if (pseudoclassData.Contains("first-child"))
					return FirstChild(node);
				if (pseudoclassData.Contains("last-child"))
					return LastChild(node);
				if (pseudoclassData.Contains("only-child"))
					return OnlyChild(node);
				if (pseudoclassData.Contains("empty"))
					return Empty(node);
			}

			return true;
		}

		private bool Empty(HtmlNode node)
		{
			return node.ChildNodes.Count == 0;
		}

		private bool OnlyChild(HtmlNode node)
		{
			List<HtmlNode> siblings = new List<HtmlNode>();

			foreach (var sibling in node.ParentNode.ChildNodes)
			{
				if (sibling.NodeType == HtmlNodeType.Element)
				{
					siblings.Add(sibling);
				}
			}


			if (siblings.Count == 1 && node.ParentNode.Name != "#document")
				return true;

			return false;
		}

		private bool FirstChild(HtmlNode node)
		{
			List<HtmlNode> siblings = new List<HtmlNode>();

			foreach (var sibling in node.ParentNode.ChildNodes)
			{
				if (sibling.NodeType == HtmlNodeType.Element)
				{
					siblings.Add(sibling);
				}
			}
			
			
			if(siblings[0] == node)
				return true;

			return false;
		}

		private bool LastChild(HtmlNode node)
		{
			List<HtmlNode> siblings = new List<HtmlNode>();

			foreach (var sibling in node.ParentNode.ChildNodes)
			{
				if (sibling.NodeType == HtmlNodeType.Element)
				{
					siblings.Add(sibling);
				}
			}


			if (siblings[siblings.Count - 1] == node && node.ParentNode.Name != "#document")
				return true;

			return false;
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