using Fizzler.Parser;
using Fizzler.Parser.ChunkHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests.ChunkParsing
{
	[TestClass]
	public class PseudoclassTests
	{
		private readonly ChunkParser _chunkParser = new ChunkParser();
		
		[TestMethod]
		public void Star_NthChild()
		{
			var chunks = _chunkParser.GetChunks("*:nth-child(1)");
			
			Assert.AreEqual(1, chunks.Count);
			Assert.AreEqual(ChunkType.Star, chunks[0].ChunkType);
			Assert.AreEqual(DescendantSelectionType.LastSelector, chunks[0].DescendantSelectionType);
			Assert.AreEqual("nth-child(1)", chunks[0].PseudoclassData);
		}

		[TestMethod]
		public void Element_NthChild()
		{
			var chunks = _chunkParser.GetChunks("div:nth-child(1)");

			Assert.AreEqual(1, chunks.Count);
			Assert.AreEqual(ChunkType.TagName, chunks[0].ChunkType);
			Assert.AreEqual("div", chunks[0].Body);
			Assert.AreEqual(DescendantSelectionType.LastSelector, chunks[0].DescendantSelectionType);
			Assert.AreEqual("nth-child(1)", chunks[0].PseudoclassData);
		}

		[TestMethod]
		public void NthChild_No_Prefix()
		{
			var chunks = _chunkParser.GetChunks(":nth-child(1)");

			Assert.AreEqual(1, chunks.Count);
			Assert.AreEqual(ChunkType.Star, chunks[0].ChunkType);
			Assert.AreEqual("*", chunks[0].Body);
			Assert.AreEqual(DescendantSelectionType.LastSelector, chunks[0].DescendantSelectionType);
			Assert.AreEqual("nth-child(1)", chunks[0].PseudoclassData);
		}
	}
}