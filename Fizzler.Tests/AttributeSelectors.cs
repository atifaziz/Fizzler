using Fizzler.DocumentParsers.HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
	[TestClass]
	public class AttributeSelectors : SelectorBaseTest
	{
		[TestMethod]
		public void Element_Attr_Exists()
		{
			var results = Parser.Select("div[id]");
			
			Assert.AreEqual(2, results.Count);
			Assert.AreEqual("div", results[0].Name);
			Assert.AreEqual("div", results[1].Name);
		}

		[TestMethod]
		public void Element_Attr_Equals_With_Double_Quotes()
		{
			var results = Parser.Select("div[id=\"someOtherDiv\"]");

			Assert.AreEqual(1, results.Count);
			Assert.AreEqual("div", results[0].Name);
		}

		[TestMethod]
		public void Element_Attr_Space_Separated_With_Double_Quotes()
		{
			var results = Parser.Select("p[class~=\"ohyeah\"]");

			Assert.AreEqual(1, results.Count);
			Assert.AreEqual("p", results[0].Name);
            Assert.AreEqual("eeeee", ((HtmlNodeWrapper)results[0]).HtmlNode.InnerText);
		}

		[TestMethod]
		public void Element_Attr_Hyphen_Separated_With_Double_Quotes()
		{
			var results = Parser.Select("span[class|=\"separated\"]");

			Assert.AreEqual(1, results.Count);
			Assert.AreEqual("span", results[0].Name);
            Assert.AreEqual("test", ((HtmlNodeWrapper)results[0]).HtmlNode.InnerText);
		}
	}
}