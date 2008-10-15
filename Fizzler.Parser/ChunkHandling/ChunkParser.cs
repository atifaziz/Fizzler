using System;
using System.Collections.Generic;

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
				string space = spaceSeparated[i];
				if (space.Contains(">"))
				{
					string[] childSeparated = space.Split(">".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

					for (int j = 0; j < childSeparated.Length; j++)
					{
						chunks.Add(new Chunk(GetChunkType(childSeparated[j]), GetBody(childSeparated[j]), j == childSeparated.Length - 1 ? DescendantSelectionType.LastSelector : DescendantSelectionType.Children, GetPseudoData(childSeparated[j])));
					}
				}
				else if (space.Contains("+"))
				{
					string[] adjSeparated = space.Split("+".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

					for (int j = 0; j < adjSeparated.Length; j++)
					{
						chunks.Add(new Chunk(GetChunkType(adjSeparated[j]), GetBody(adjSeparated[j]), j == adjSeparated.Length - 1 ? DescendantSelectionType.LastSelector : DescendantSelectionType.Adjacent, GetPseudoData(adjSeparated[j])));
					}
				}
				else
				{
					string body = GetBody(space);
					var finalDescendant = i == spaceSeparated.Length - 1 ? DescendantSelectionType.LastSelector : DescendantSelectionType.Descendant;

					chunks.Add(new Chunk(GetChunkType(body), body, finalDescendant, GetPseudoData(space)));
				}
			}

			return chunks;
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
			if(chunk.Contains(":"))
			{
				string[] parts = chunk.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			
				if(parts.Length > 1)
					return parts[0];
				else
					return "*";
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