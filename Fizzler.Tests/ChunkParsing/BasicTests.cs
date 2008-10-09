using Fizzler.Parser;
using Fizzler.Parser.ChunkHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests.ChunkParsing
{
	[TestClass]
	public class BasicTests
	{
		private ChunkParser _chunkParser = new ChunkParser();
		
		[TestMethod]
		public void Single()
		{
			var results = _chunkParser.GetChunks("body");
			
			Assert.AreEqual(1, results.Count);
			Assert.AreEqual("body", results[0].Body);
			Assert.AreEqual(ChunkType.TagName, results[0].ChunkType);
			Assert.AreEqual(DescendantSelectionType.LastSelector, results[0].DescendantSelectionType);
		}

		[TestMethod]
		public void Descendant()
		{
			var results = _chunkParser.GetChunks("body p");

			Assert.AreEqual(2, results.Count);
			Assert.AreEqual("body", results[0].Body);
			Assert.AreEqual(ChunkType.TagName, results[0].ChunkType);
			Assert.AreEqual(DescendantSelectionType.Descendant, results[0].DescendantSelectionType);
			Assert.AreEqual("p", results[1].Body);
			Assert.AreEqual(ChunkType.TagName, results[1].ChunkType);
			Assert.AreEqual(DescendantSelectionType.LastSelector, results[1].DescendantSelectionType);
		}

		[TestMethod]
		public void Child_No_Spaces()
		{
			var results = _chunkParser.GetChunks("body>p");

			Assert.AreEqual(2, results.Count);
			Assert.AreEqual("body", results[0].Body);
			Assert.AreEqual(ChunkType.TagName, results[0].ChunkType);
			Assert.AreEqual(DescendantSelectionType.Children, results[0].DescendantSelectionType);
			Assert.AreEqual("p", results[1].Body);
			Assert.AreEqual(ChunkType.TagName, results[1].ChunkType);
			Assert.AreEqual(DescendantSelectionType.LastSelector, results[1].DescendantSelectionType);
		}

		[TestMethod]
		public void Child_With_Spaces()
		{
			var results = _chunkParser.GetChunks("body > p");

			Assert.AreEqual(2, results.Count);
			Assert.AreEqual("body", results[0].Body);
			Assert.AreEqual(ChunkType.TagName, results[0].ChunkType);
			Assert.AreEqual(DescendantSelectionType.Children, results[0].DescendantSelectionType);
			Assert.AreEqual("p", results[1].Body);
			Assert.AreEqual(ChunkType.TagName, results[1].ChunkType);
			Assert.AreEqual(DescendantSelectionType.LastSelector, results[1].DescendantSelectionType);
		}
		
		[TestMethod]
		public void Adj_No_Spaces()
		{
			var results = _chunkParser.GetChunks("body+p");

			Assert.AreEqual(2, results.Count);
			Assert.AreEqual("body", results[0].Body);
			Assert.AreEqual(ChunkType.TagName, results[0].ChunkType);
			Assert.AreEqual(DescendantSelectionType.Adjacent, results[0].DescendantSelectionType);
			Assert.AreEqual("p", results[1].Body);
			Assert.AreEqual(ChunkType.TagName, results[1].ChunkType);
			Assert.AreEqual(DescendantSelectionType.LastSelector, results[1].DescendantSelectionType);
		}

		[TestMethod]
		public void Adj_Spaces()
		{
			var results = _chunkParser.GetChunks("body + p");

			Assert.AreEqual(2, results.Count);
			Assert.AreEqual("body", results[0].Body);
			Assert.AreEqual(ChunkType.TagName, results[0].ChunkType);
			Assert.AreEqual(DescendantSelectionType.Adjacent, results[0].DescendantSelectionType);
			Assert.AreEqual("p", results[1].Body);
			Assert.AreEqual(ChunkType.TagName, results[1].ChunkType);
			Assert.AreEqual(DescendantSelectionType.LastSelector, results[1].DescendantSelectionType);
		}
	}
}