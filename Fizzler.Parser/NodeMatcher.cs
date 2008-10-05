using System;
using System.Collections.Generic;
using Fizzler.Parser.ChunkHandling;
using Fizzler.Parser.Extensions;
using HtmlAgilityPack;

namespace Fizzler.Parser
{
	public class NodeMatcher
	{
		public bool IsMatch(HtmlNode node, Chunk chunk, Chunk previousChunk)
		{
			bool match = false;

			if (node.NodeType != HtmlNodeType.Element)
				return false;

			switch(chunk.ChunkType)
			{
				case ChunkType.Star:
					match = MatchStar(node, previousChunk);
					break;
				case ChunkType.TagName:
					match = MatchTag(node, chunk, previousChunk);
					break;
				case ChunkType.Id:
					match = MatchId(node, chunk, previousChunk);
					break;
				case ChunkType.Class:
					match = MatchClass(node, chunk, previousChunk);
					break;
			}

			return match;
		}

		private bool MatchId(HtmlNode node, Chunk chunk, Chunk previousChunk)
		{
			bool match = false;
		
			if (node.Attributes["id"] != null)
			{
				string idValue = node.Attributes["id"].Value;
				string[] chunkParts = chunk.Body.Split("#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

				// if length is greater than one, we could have an id selector with element
				if (chunkParts.Length > 1)
				{
					if (node.Name == chunkParts[0] && chunkParts[1] == idValue)
					{
						match = previousChunk == null || IsMatch(node.ParentNode, previousChunk, null);
					}
				}
				else
				{
					if (chunkParts[0] == idValue)
					{
						match = previousChunk == null || IsMatch(node.ParentNode, previousChunk, null);
					}
				}
			}
			return match;
		}

		private bool MatchTag(HtmlNode node, Chunk chunk, Chunk previousChunk)
		{
			bool match = false;
		
			if (node.Name == chunk.Body)
			{
				match = previousChunk == null || IsMatch(node.ParentNode, previousChunk, null);
			}
			return match;
		}

		private bool MatchStar(HtmlNode node, Chunk previousChunk)
		{
			bool match = false;
		
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

			return match;
		}

		private bool MatchClass(HtmlNode node, Chunk chunk, Chunk previousChunk)
		{
			bool match = false;
		
			if (node.Attributes["class"] != null)
			{
				List<string> idValues = new List<string>(node.Attributes["class"].Value.Split(" ".ToCharArray()));
				List<string> chunkParts = new List<string>(chunk.Body.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

				// if length is greater than one, we could have an id selector with element
				if (chunkParts.Count > 1)
				{
					if(chunkParts.ContainsAll(idValues))
					{
						match = true;
					}
					else if (node.Name == chunkParts[0] && idValues.Contains(chunkParts[1]))
					{
						match = previousChunk == null || IsMatch(node.ParentNode, previousChunk, null);
					}
				}
				else
				{
					if (idValues.Contains(chunkParts[0]))
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
				}
			}

			return match;
		}
	}
}