using System;
using HtmlAgilityPack;

namespace Fizzle.Parser
{
	public class NodeMatcher
	{
		public bool IsMatch(HtmlNode node, string chunk, string previousChunk)
		{
			bool match = false;
			ChunkType chunkType = GetChunkType(chunk);

			if (node.NodeType != HtmlNodeType.Element)
				return false;

			if (chunkType == ChunkType.Star)
			{
				if (previousChunk != null)
				{
					// are any parent nodes affected by the previous chunk?
					var parent = node.ParentNode;

					while (parent != null)
					{
						match = IsMatch(parent, previousChunk, null);

						if (match)
						{
							break;
						}

						parent = parent.ParentNode;
					}
				}
				else
				{
					match = true;
				}
			}

			if (chunkType == ChunkType.TagName)
			{
				if (node.Name == chunk)
				{
					if (previousChunk != null)
					{
						match = IsMatch(node.ParentNode, previousChunk, null);
					}
					else
					{
						match = true;
					}
				}
			}

			if (chunkType == ChunkType.Id)
			{
				if (node.Attributes["id"] != null)
				{
					string idValue = node.Attributes["id"].Value;
					string[] chunkParts = chunk.Split("#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

					// if length is greater than one, we could have an id selector with element
					if (chunkParts.Length > 1)
					{
						if (node.Name == chunkParts[0] && chunkParts[1] == idValue)
						{
							if (previousChunk != null)
							{
								match = IsMatch(node.ParentNode, previousChunk, null);
							}
							else
							{
								match = true;
							}
						}
					}
					else
					{
						if (chunkParts[0] == idValue)
						{
							if (previousChunk != null)
							{
								match = IsMatch(node.ParentNode, previousChunk, null);
							}
							else
							{
								match = true;
							}
						}
					}
				}
			}

			if(chunkType == ChunkType.Class)
				match = MatchClass(node, chunk, previousChunk);

			return match;
		}

		private bool MatchClass(HtmlNode node, string chunk, string previousChunk)
		{
			bool match = false;
		
			if (node.Attributes["class"] != null)
			{
				string idValue = node.Attributes["class"].Value;
				string[] chunkParts = chunk.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

				// if length is greater than one, we could have an id selector with element
				if (chunkParts.Length > 1)
				{
					if (node.Name == chunkParts[0] && chunkParts[1] == idValue)
					{
						if (previousChunk != null)
						{
							match = IsMatch(node.ParentNode, previousChunk, null);
						}
						else
						{
							match = true;
						}
					}
				}
				else
				{
					if (chunkParts[0] == idValue)
					{
						if (previousChunk != null)
						{
							match = IsMatch(node.ParentNode, previousChunk, null);
						}
						else
						{
							match = true;
						}
					}
				}
			}

			return match;
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