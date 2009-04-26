using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
	[TestClass]
	public class AttributeSelectors : SelectorBaseTest
	{
		[TestMethod]
		public void Element_Attr_Exists()
		{
			var results = SelectList("div[id]");
			
			Assert.AreEqual(2, results.Count);
			Assert.AreEqual("div", results[0].Name);
			Assert.AreEqual("div", results[1].Name);
		}

		[TestMethod]
		public void Element_Attr_Equals_With_Double_Quotes()
		{
			var results = SelectList("div[id=\"someOtherDiv\"]");

			Assert.AreEqual(1, results.Count);
			Assert.AreEqual("div", results[0].Name);
		}

		[TestMethod]
		public void Element_Attr_Space_Separated_With_Double_Quotes()
		{
			var results = SelectList("p[class~=\"ohyeah\"]");

			Assert.AreEqual(1, results.Count);
			Assert.AreEqual("p", results[0].Name);
            Assert.AreEqual("eeeee", results[0].InnerText);
		}

		[TestMethod]
		public void Element_Attr_Hyphen_Separated_With_Double_Quotes()
		{
			var results = SelectList("span[class|=\"separated\"]");

			Assert.AreEqual(1, results.Count);
			Assert.AreEqual("span", results[0].Name);
            Assert.AreEqual("test", results[0].InnerText);
		}

        [TestMethod]
        public void Implicit_Star_Attr_Exact_With_Double_Quotes()
        {
            var results = SelectList("[class=\"checkit\"]");

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("div", results[0].Name);
            Assert.AreEqual("woooeeeee", results[0].InnerText);
            Assert.AreEqual("div", results[1].Name);
            Assert.AreEqual("woootooowe", results[1].InnerText);
        }

        [TestMethod]
        public void Star_Attr_Exact_With_Double_Quotes()
        {
            var results = SelectList("*[class=\"checkit\"]");

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("div", results[0].Name);
            Assert.AreEqual("woooeeeee", results[0].InnerText);
            Assert.AreEqual("div", results[1].Name);
            Assert.AreEqual("woootooowe", results[1].InnerText);
        }
    }
}