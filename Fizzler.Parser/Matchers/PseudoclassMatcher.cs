using System;
using System.Linq;
using Fizzler.Parser.Document;
using Fizzler.Parser.Extensions;

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

		private static bool Empty(IDocumentNode node)
		{
			return node.ChildNodes.Count == 0;
		}

		private static bool OnlyChild(IDocumentNode node)
		{
		    var parent = node.ParentNode;
            return parent.Elements().Count() == 1 
                && parent.Name != "#document";
		}

	    private static bool FirstChild(IDocumentNode node)
	    {
	        return node.ParentNode.ChildNodes.Where(child => child.IsElement).First().Equals(node);
	    }

	    private static bool LastChild(IDocumentNode node)
		{
            var parent = node.ParentNode;
            return parent.Elements().Last().Equals(node)
                && parent.Name != "#document";
        }

		private static bool NthChild(string pseudoclassData, IDocumentNode node)
		{
			var sought = Convert.ToInt32(pseudoclassData.Replace("nth-child(", string.Empty).Replace(")", string.Empty));
            var found = 0;
		    var position = 1;
		    var e = node.ParentNode.Elements().GetEnumerator();
            while (e.MoveNext())
            {
				if (e.Current.Equals(node))
					found = position;
                position++;
            }
			return found == sought;
		}
	}
}