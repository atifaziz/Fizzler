using System;
using System.Collections.Generic;
using Fizzler.Parser.Document;

namespace Fizzler.Parser.Matchers
{
	/// <summary>
	/// Matches against a pseudoclass.
	/// </summary>
	public class PseudoclassMatcher
	{
		/// <summary>
		/// Is the specified node a match against the pseudoclass?
		/// </summary>
		/// <param name="pseudoclassData"></param>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool Match(string pseudoclassData, IDocumentNode node)
		{
			if(!string.IsNullOrEmpty(pseudoclassData))
			{
				if(pseudoclassData.Contains("nth-child"))
					return NthChild(pseudoclassData, node);
				if(pseudoclassData.Contains("first-child"))
					return FirstChild(node);
				if(pseudoclassData.Contains("last-child"))
					return LastChild(node);
				if(pseudoclassData.Contains("only-child"))
					return OnlyChild(node);
				if(pseudoclassData.Contains("empty"))
					return Empty(node);
			}

			return true;
		}

		private bool Empty(IDocumentNode node)
		{
			return node.ChildNodes.Count == 0;
		}

		private bool OnlyChild(IDocumentNode node)
		{
			List<IDocumentNode> siblings = new List<IDocumentNode>();

			foreach(var sibling in node.ParentNode.ChildNodes)
			{
				if(sibling.IsElement)
				{
					siblings.Add(sibling);
				}
			}


			if(siblings.Count == 1 && node.ParentNode.Name != "#document")
				return true;

			return false;
		}

		private bool FirstChild(IDocumentNode node)
		{
			List<IDocumentNode> siblings = new List<IDocumentNode>();

			foreach(var sibling in node.ParentNode.ChildNodes)
			{
				if(sibling.IsElement)
				{
					siblings.Add(sibling);
				}
			}


			if(siblings[0].Equals(node))
				return true;

			return false;
		}

		private bool LastChild(IDocumentNode node)
		{
			List<IDocumentNode> siblings = new List<IDocumentNode>();

			foreach(var sibling in node.ParentNode.ChildNodes)
			{
				if(sibling.IsElement)
				{
					siblings.Add(sibling);
				}
			}


			if(siblings[siblings.Count - 1].Equals(node) && node.ParentNode.Name != "#document")
				return true;

			return false;
		}

		private bool NthChild(string pseudoclassData, IDocumentNode node)
		{
			int digit = Convert.ToInt32(pseudoclassData.Replace("nth-child(", string.Empty).Replace(")", string.Empty)) - 1;

			List<IDocumentNode> nodes = new List<IDocumentNode>();

			foreach(var sibling in node.ParentNode.ChildNodes)
			{
				if(sibling.IsElement)
				{
					nodes.Add(sibling);
				}
			}

			int foundAt = -1;

			for(int i = 0; i < nodes.Count; i++)
			{
				if(nodes[i].Equals(node))
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