using System;
using System.Collections.Generic;
using Fizzler.Parser.ChunkHandling;
using Fizzler.Parser.Extensions;
using HtmlAgilityPack;

namespace Fizzler.Parser
{
	public class NodeMatcher
	{
		public bool IsDownwardMatch(HtmlNode node, List<Chunk> chunks, int currentChunk)
		{
			Chunk chunk = chunks[currentChunk];
	
		
			bool match = false;

			if (node.NodeType != HtmlNodeType.Element)
				return false;

			switch(chunk.ChunkType)
			{
				case ChunkType.Star:
					match = MatchStar(node, chunks, currentChunk);
					break;
				case ChunkType.TagName:
					match = MatchTag(node, chunks, currentChunk);
					break;
				case ChunkType.Id:
					match = MatchId(node, chunks, currentChunk);
					break;
				case ChunkType.Class:
					match = MatchClass(node, chunks, currentChunk);
					break;
			}

			return match;
		}

		public bool IsUpwardMatch(List<Chunk> chunks, int currentChunk, HtmlNode node)
		{
			bool match = false;
		
			// are any parent nodes affected by the previous chunk?
			var parent = node.ParentNode;

			while (parent != null)
			{
				Chunk previousChunk = currentChunk > 0 ? chunks[currentChunk - 1] : null;
				
				if(previousChunk != null)
					match = IsDownwardMatch(parent, chunks, currentChunk - 1);

				if (match)
				{
					break;
				}

				parent = parent.ParentNode;
			}
			return match;
		}

		public bool IsImmediateUpwardMatch(List<Chunk> chunks, int currentChunk, HtmlNode node)
		{
			bool match = false;

			// are any parent nodes affected by the previous chunk?
			var parent = node.ParentNode;

			while (parent != null)
			{
				Chunk previousChunk = currentChunk > 0 ? chunks[currentChunk - 1] : null;

				if (previousChunk != null)
					match = IsDownwardMatch(parent, chunks, currentChunk - 1);

				//if (match)
				//{
					break;
				//}

				parent = parent.ParentNode;
			}
			return match;
		}

		private bool MatchId(HtmlNode node, List<Chunk> chunks, int currentChunk)
		{
			bool match = false;
			Chunk chunk = chunks[currentChunk];
			Chunk previousChunk = currentChunk > 0 ? chunks[currentChunk - 1] : null;
		
			if (node.Attributes["id"] != null)
			{
				string idValue = node.Attributes["id"].Value;
				string[] chunkParts = chunk.Body.Split("#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

				// if length is greater than one, we could have an id selector with element
				if (chunkParts.Length > 1)
				{
					if (node.Name == chunkParts[0] && chunkParts[1] == idValue)
					{
						match = previousChunk == null || IsDownwardMatch(node.ParentNode, chunks, currentChunk - 1);
					}
				}
				else
				{
					if (chunkParts[0] == idValue)
					{
						if(previousChunk == null)
						{
							match = true;
						}
						else
						{
							match = IsDownwardMatch(node.ParentNode, chunks, currentChunk - 1);
						}
					}
				}
			}
			else
			{
				match = false;
			}
			return match;
		}

		private bool MatchTag(HtmlNode node, List<Chunk> chunks, int currentChunk)
		{
			bool match = false;

			Chunk chunk = chunks[currentChunk];
			Chunk previousChunk = currentChunk > 0 ? chunks[currentChunk - 1] : null;

			if (!new PseudoclassMatcher().Match(chunk.PseudoclassData, node))
				return false;

			if (!new AttributeMatcher().Match(chunk.AttributeSelectorData, node))
				return false;
					
			if (node.Name == chunk.Body)
			{
				if(previousChunk != null)
				{
					if (previousChunk.DescendantSelectionType == DescendantSelectionType.Children)
					{
						match = true;
					}
					else if (previousChunk.DescendantSelectionType == DescendantSelectionType.Adjacent)
					{
						return IsDownwardMatch(node.PreviousSibling, chunks, currentChunk - 1);
					}
					else
					{
						// Check if the previous chunk matched the parent node
						match = IsDownwardMatch(node.ParentNode, chunks, currentChunk - 1);
					}
				}
				else
				{
					match = true;
				}
			}
			else
			{
				if(node.PreviousSibling != null)
				{
					if (node.PreviousSibling.Name == chunk.Body && chunk.DescendantSelectionType == DescendantSelectionType.Adjacent)
					{
						match = true;
					}
				}
			}
			return match;
		}

		private bool MatchStar(HtmlNode node, List<Chunk> chunks, int currentChunk)
		{
			Chunk chunk = chunks[currentChunk];
			Chunk previousChunk = currentChunk > 0 ? chunks[currentChunk - 1] : null;
			bool match = true;

			if(!new PseudoclassMatcher().Match(chunk.PseudoclassData, node))
				return false;
	
			if (previousChunk != null)
			{
				if (previousChunk.DescendantSelectionType == DescendantSelectionType.Children)
				{
					// return true
					match = IsImmediateUpwardMatch(chunks, currentChunk, node);
				}
				else
				{
					if (chunks.Exists(c => c.DescendantSelectionType == DescendantSelectionType.Children))
					{
						match = false;
					}
					else
					{
						match = IsUpwardMatch(chunks, currentChunk, node);
					}
				}
			}

			return match;
		}

		private bool MatchClass(HtmlNode node, List<Chunk> chunks, int currentChunk)
		{
			bool match = false;
			Chunk chunk = chunks[currentChunk];
			Chunk previousChunk = currentChunk > 0 ? chunks[currentChunk - 1] : null;
		
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
						match = previousChunk == null || IsDownwardMatch(node.ParentNode, chunks, currentChunk - 1);
					}
				}
				else
				{
					if (idValues.Contains(chunkParts[0]))
					{
						match = previousChunk == null || IsUpwardMatch(chunks, currentChunk, node);
					}
				}
			}

			return match;
		}
	}
}