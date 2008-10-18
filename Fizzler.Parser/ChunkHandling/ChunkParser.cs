using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Fizzler.Parser.ChunkHandling
{
	/// <summary>
	/// Pulls out information about a selector chunk.
	/// </summary>
	public class ChunkParser
	{
		public List<Chunk> GetChunks(string rawSelector)
		{
			rawSelector = rawSelector.Replace(" >", ">").Replace("> ", ">").Replace(" +", "+").Replace("+ ", "+");
		
			List<Chunk> chunks = new List<Chunk>();
		
			string[] spaceSeparated = rawSelector.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

			for (int i = 0; i < spaceSeparated.Length; i++)
			{
				HandleChunk(i, spaceSeparated, chunks);
			}

			return chunks;
		}

		private static void HandleChunk(int i, string[] spaceSeparated, ICollection<Chunk> chunks)
		{
			string space = spaceSeparated[i];
			if (space.Contains(">"))
			{
				HandleChildren(space, chunks);
			}
			else if (space.Contains("+"))
			{
				HandleAdjacent(space, chunks);
			}
			else
			{
				HandleDescendant(i, spaceSeparated, space, chunks);
			}
		}

		private static void HandleDescendant(int i, ICollection<string> spaceSeparated, string space, ICollection<Chunk> chunks)
		{
			string body = GetBody(space);
			var finalDescendant = i == spaceSeparated.Count - 1 ? DescendantSelectionType.LastSelector : DescendantSelectionType.Descendant;

			chunks.Add(new Chunk(GetChunkType(body), body, finalDescendant, GetPseudoData(space), GetAttributeSelectorData(space)));
		}

		private static void HandleAdjacent(string space, ICollection<Chunk> chunks)
		{
			string[] adjSeparated = space.Split("+".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

			for (int j = 0; j < adjSeparated.Length; j++)
			{
				chunks.Add(new Chunk(GetChunkType(adjSeparated[j]), GetBody(adjSeparated[j]), j == adjSeparated.Length - 1 ? DescendantSelectionType.LastSelector : DescendantSelectionType.Adjacent, GetPseudoData(adjSeparated[j]), GetAttributeSelectorData(adjSeparated[j])));
			}
		}

		private static void HandleChildren(string space, ICollection<Chunk> chunks)
		{
			string[] childSeparated = space.Split(">".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

			for (int j = 0; j < childSeparated.Length; j++)
			{
				chunks.Add(new Chunk(GetChunkType(childSeparated[j]), GetBody(childSeparated[j]), j == childSeparated.Length - 1 ? DescendantSelectionType.LastSelector : DescendantSelectionType.Children, GetPseudoData(childSeparated[j]), GetAttributeSelectorData(childSeparated[j])));
			}
		}

		private static AttributeSelectorData GetAttributeSelectorData(string chunk)
		{
			AttributeSelectorData data = null;
		
			// does the chunk even contain an attribute selector?
			bool isAttr = chunk.Contains("[");
			
			if(isAttr)
			{
				string selector = Regex.Match(chunk, @".*?\[(.*?)\]").Groups[1].Captures[0].Value;
				
				if(selector.Contains("|="))
				{
					
				}
				else if(selector.Contains("~="))
				{
					
				}
				else if(selector.Contains("="))
				{
					string[] selectorParts = selector.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				
					data = new AttributeSelectorData
					       	{
								Attribute = selectorParts[0],
								Comparison = AttributeComparator.Exact,
								Value = selectorParts[1].Replace("\"", string.Empty)
					       	};
				}
				else
				{
					// we now have to assume we've got a selector such as a[id], i.e. an attribute existence match
					data = new AttributeSelectorData {Attribute = selector};
				}
				
			}
		
			return data;
		}

		private static string GetPseudoData(string chunk)
		{
			if(!chunk.Contains(":"))
				return null;
		
			string[] parts = chunk.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			
			if(parts.Length > 1)
				return parts[1];
			
			return parts[0];
		}

		private static string GetBody(string chunk)
		{
			if (chunk.Contains("["))
			{
				chunk = Regex.Replace(chunk, @"\[.*\]", string.Empty);
			}
		
			if(chunk.Contains(":"))
			{
				string[] parts = chunk.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

				chunk = parts.Length > 1 ? parts[0] : "*";
			}


			return chunk;
		}

		private static ChunkType GetChunkType(string chunk)
		{
			chunk = chunk.Split(":".ToCharArray())[0];
		
			if (chunk.Trim() == "*")
				return ChunkType.Star;

			if (chunk.Contains("#"))
				return ChunkType.Id;

			if (chunk.Contains("."))
				return ChunkType.Class;

			return ChunkType.TagName;
		}
	}
}