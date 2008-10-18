using System.Collections.Generic;
using Fizzler.Parser;
using Fizzler.Parser.ChunkHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests.ChunkParsing
{
	[TestClass]
	public class AttributeTests
	{
		private readonly ChunkParser _chunkParser = new ChunkParser();

		[TestMethod]
		public void Element_Attr_Exists()
		{
			var result = _chunkParser.GetChunks("a[id]");
			
			Assert.AreEqual(DescendantSelectionType.LastSelector,result[0].DescendantSelectionType);
			Assert.AreEqual("a", result[0].Body);
			Assert.AreEqual(ChunkType.TagName, result[0].ChunkType);
			Assert.AreEqual("id", result[0].AttributeSelectorData.Attribute);
		}

		[TestMethod]
		public void Element_Attr_Equals()
		{
			Assert_Element_Attr_Comparison(_chunkParser.GetChunks("div[id=\"someOtherDiv\"]"), AttributeComparator.Exact);
			Assert_Element_Attr_Comparison(_chunkParser.GetChunks("div[id='someOtherDiv']"), AttributeComparator.Exact);
			Assert_Element_Attr_Comparison(_chunkParser.GetChunks("div[id=someOtherDiv]"), AttributeComparator.Exact);
		}

		[TestMethod]
		public void Element_Attr_Comma_Separated()
		{
			Assert_Element_Attr_Comparison(_chunkParser.GetChunks("div[id|=\"someOtherDiv\"]"), AttributeComparator.CommaSeparated);
			Assert_Element_Attr_Comparison(_chunkParser.GetChunks("div[id|='someOtherDiv']"), AttributeComparator.CommaSeparated);
			Assert_Element_Attr_Comparison(_chunkParser.GetChunks("div[id|=someOtherDiv]"), AttributeComparator.CommaSeparated);
		}

		[TestMethod]
		public void Element_Attr_Space_Separated()
		{
			Assert_Element_Attr_Comparison(_chunkParser.GetChunks("div[id~=\"someOtherDiv\"]"), AttributeComparator.SpaceSeparated);
			Assert_Element_Attr_Comparison(_chunkParser.GetChunks("div[id~='someOtherDiv']"), AttributeComparator.SpaceSeparated);
			Assert_Element_Attr_Comparison(_chunkParser.GetChunks("div[id~=someOtherDiv]"), AttributeComparator.SpaceSeparated);
		}

		private static void Assert_Element_Attr_Comparison(IList<Chunk> result, AttributeComparator comparison)
		{
			Assert.AreEqual(DescendantSelectionType.LastSelector, result[0].DescendantSelectionType);
			Assert.AreEqual("div", result[0].Body);
			Assert.AreEqual("id", result[0].AttributeSelectorData.Attribute);
			Assert.AreEqual("someOtherDiv", result[0].AttributeSelectorData.Value);
			Assert.AreEqual(comparison, result[0].AttributeSelectorData.Comparison);
		}
	}
}