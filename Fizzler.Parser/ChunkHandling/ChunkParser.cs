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
			rawSelector = rawSelector.Replace(" >", ">").Replace("> ", ">");
		
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
						chunks.Add(new Chunk(GetChunkType(childSeparated[j]), childSeparated[j], j == childSeparated.Length - 1 ? DescendantSelectionType.LastSelector : DescendantSelectionType.Children));
					}
				}
				else
				{
					chunks.Add(new Chunk(GetChunkType(space), space, i == spaceSeparated.Length - 1 ? DescendantSelectionType.LastSelector : DescendantSelectionType.Descendant));
				}
			}

			return chunks;
		}

		private ChunkType GetChunkType(string chunk)
		{
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