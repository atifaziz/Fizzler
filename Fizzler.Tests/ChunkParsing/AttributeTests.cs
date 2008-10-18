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
		public void Element_Attr_Equals__With_Double_Quotes()
		{
			var result = _chunkParser.GetChunks("div[id=\"someOtherDiv\"]");

			Assert.AreEqual(DescendantSelectionType.LastSelector, result[0].DescendantSelectionType);
			Assert.AreEqual("div", result[0].Body);
			Assert.AreEqual("id", result[0].AttributeSelectorData.Attribute);
			Assert.AreEqual("someOtherDiv", result[0].AttributeSelectorData.Value);
			Assert.AreEqual(AttributeComparator.Exact, result[0].AttributeSelectorData.Comparison);
		}
	}
}