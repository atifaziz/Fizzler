using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
	[TestClass]
	public class AttributeSelectors : SelectorBaseTest
	{
		[TestMethod]
		public void Element_Attr_Exists()
		{
			var results = Parser.Parse("div[id]");
			
			Assert.AreEqual(2, results.Count);
			Assert.AreEqual("div", results[0].Name);
			Assert.AreEqual("div", results[1].Name);
		}

		[TestMethod]
		public void Element_Attr_Equals_With_Double_Quotes()
		{
			var results = Parser.Parse("div[id=\"someOtherDiv\"]");

			Assert.AreEqual(1, results.Count);
			Assert.AreEqual("div", results[0].Name);
		}
	}
}