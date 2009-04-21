using System;
using System.Collections.Generic;
using System.Linq;
using Fizzler.Parser.ChunkHandling;
using Fizzler.Parser.Document;
using Fizzler.Parser.Extensions;

namespace Fizzler.Parser.Matchers
{
	///<summary>
	/// Matches a Node against a Chunk.
	///</summary>
	public class NodeMatcher
	{
		/// <summary>
		/// Does this node match the chunk?
		/// </summary>
		/// <remarks>Referred to as "downward" match to differentiate from an upward match but it's really just a straightforward match between node and chunk.</remarks>
		/// <param name="node"></param>
		/// <param name="chunks"></param>
		/// <param name="currentChunk"></param>
		/// <returns></returns>
		public bool IsDownwardMatch(IDocumentNode node, IList<Chunk> chunks, int currentChunk)
		{
			Chunk chunk = chunks[currentChunk];

			bool match = false;

			if(!node.IsElement)
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

		/// <summary>
		/// Is there a match upward anywhere in the selector chain?
		/// </summary>
		/// <param name="chunks"></param>
		/// <param name="currentChunk"></param>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool IsUpwardMatch(IList<Chunk> chunks, int currentChunk, IDocumentNode node)
		{
			bool match = false;

			// are any parent nodes affected by the previous chunk?
			var parent = node.ParentNode;

			while(parent != null)
			{
				Chunk previousChunk = currentChunk > 0 ? chunks[currentChunk - 1] : null;

				if(previousChunk != null)
					match = IsDownwardMatch(parent, chunks, currentChunk - 1);

				if(match)
				{
					break;
				}

				parent = parent.ParentNode;
			}
			return match;
		}

		/// <summary>
		/// Is the immediate parent node affected by the previous chunk?
		/// </summary>
		/// <param name="chunks"></param>
		/// <param name="currentChunk"></param>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool IsImmediateUpwardMatch(IList<Chunk> chunks, int currentChunk, IDocumentNode node)
		{
			bool match = false;

			// are any parent nodes affected by the previous chunk?
			var parent = node.ParentNode;

			while(parent != null)
			{
				Chunk previousChunk = currentChunk > 0 ? chunks[currentChunk - 1] : null;

				if(previousChunk != null)
					match = IsDownwardMatch(parent, chunks, currentChunk - 1);

				break;
			}
			return match;
		}

		private bool MatchId(IDocumentNode node, IList<Chunk> chunks, int currentChunk)
		{
			bool match = false;
			Chunk chunk = chunks[currentChunk];
			Chunk previousChunk = currentChunk > 0 ? chunks[currentChunk - 1] : null;

			if(node.Id != null)
			{
				string idValue = node.Id;
				string[] chunkParts = chunk.Body.Split("#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

				// if length is greater than one, we could have an id selector with element
				if(chunkParts.Length > 1)
				{
					if(node.Name == chunkParts[0] && chunkParts[1] == idValue)
					{
						match = previousChunk == null || IsDownwardMatch(node.ParentNode, chunks, currentChunk - 1);
					}
				}
				else
				{
					if(chunkParts[0] == idValue)
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

		private bool MatchTag(IDocumentNode node, IList<Chunk> chunks, int currentChunk)
		{
			bool match = false;

			Chunk chunk = chunks[currentChunk];
			Chunk previousChunk = currentChunk > 0 ? chunks[currentChunk - 1] : null;

			if(!new PseudoclassMatcher().Match(chunk.PseudoclassData, node))
				return false;

			if(!new AttributeMatcher().Match(chunk.AttributeSelectorData, node))
				return false;

			if(node.Name == chunk.Body)
			{
				if(previousChunk != null)
				{
					if(previousChunk.DescendantSelectionType == DescendantSelectionType.Children)
					{
						match = true;
					}
					else if(previousChunk.DescendantSelectionType == DescendantSelectionType.Adjacent)
					{
						return IsDownwardMatch(node.PreviousSibling, chunks, currentChunk - 1);
					}
					else
					{
						// Descendant
						match = IsUpwardMatch(chunks, currentChunk, node);
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
					if(node.PreviousSibling.Name == chunk.Body && chunk.DescendantSelectionType == DescendantSelectionType.Adjacent)
					{
						match = true;
					}
				}
			}
			return match;
		}

		private bool MatchStar(IDocumentNode node, IList<Chunk> chunks, int currentChunk)
		{
			Chunk chunk = chunks[currentChunk];
			Chunk previousChunk = currentChunk > 0 ? chunks[currentChunk - 1] : null;
			bool match = true;

			if(!new PseudoclassMatcher().Match(chunk.PseudoclassData, node))
				return false;

			if(previousChunk != null)
			{
				if(previousChunk.DescendantSelectionType == DescendantSelectionType.Children)
				{
					// return true
					match = IsImmediateUpwardMatch(chunks, currentChunk, node);
				}
				else
				{
					if(chunks.Any(c => c.DescendantSelectionType == DescendantSelectionType.Children))
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

		private bool MatchClass(IDocumentNode node, IList<Chunk> chunks, int currentChunk)
		{
			bool match = false;
			Chunk chunk = chunks[currentChunk];
			Chunk previousChunk = currentChunk > 0 ? chunks[currentChunk - 1] : null;

			if(node.Class != null)
			{
				List<string> idValues = new List<string>(node.Class.Split(" ".ToCharArray()));
				List<string> chunkParts = new List<string>(chunk.Body.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

				// if length is greater than one, we could have an id selector with element
				if(chunkParts.Count > 1)
				{
					if(chunkParts.All(idValues.Contains))
					{
						match = true;
					}
					else if(node.Name == chunkParts[0] && idValues.Contains(chunkParts[1]))
					{
						match = previousChunk == null || IsDownwardMatch(node.ParentNode, chunks, currentChunk - 1);
					}
				}
				else
				{
					if(idValues.Contains(chunkParts[0]))
					{
						match = previousChunk == null || IsUpwardMatch(chunks, currentChunk, node);
					}
				}
			}

			return match;
		}
	}
}