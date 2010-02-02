namespace Fizzler.Tests
{
    using NUnit.Framework;

    [TestFixture]
	public class AttributeSelectors : SelectorBaseTest
	{
		[Test]
		public void Element_Attr_Exists()
		{
			var results = SelectList("div[id]");
			
			Assert.AreEqual(2, results.Count);
			Assert.AreEqual("div", results[0].Name);
			Assert.AreEqual("div", results[1].Name);
		}

		[Test]
		public void Element_Attr_Equals_With_Double_Quotes()
		{
			var results = SelectList("div[id=\"someOtherDiv\"]");

			Assert.AreEqual(1, results.Count);
			Assert.AreEqual("div", results[0].Name);
		}

		[Test]
		public void Element_Attr_Space_Separated_With_Double_Quotes()
		{
			var results = SelectList("p[class~=\"ohyeah\"]");

			Assert.AreEqual(1, results.Count);
			Assert.AreEqual("p", results[0].Name);
            Assert.AreEqual("eeeee", results[0].InnerText);
		}

        [Test]
        public void Element_Attr_Space_Separated_With_Empty_Value()
        {
            Assert.AreEqual(0, SelectList("p[class~='']").Count);
        }
        
        [Test]
		public void Element_Attr_Hyphen_Separated_With_Double_Quotes()
		{
			var results = SelectList("span[class|=\"separated\"]");

			Assert.AreEqual(1, results.Count);
			Assert.AreEqual("span", results[0].Name);
            Assert.AreEqual("test", results[0].InnerText);
		}

        [Test]
        public void Implicit_Star_Attr_Exact_With_Double_Quotes()
        {
            var results = SelectList("[class=\"checkit\"]");

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("div", results[0].Name);
            Assert.AreEqual("woooeeeee", results[0].InnerText);
            Assert.AreEqual("div", results[1].Name);
            Assert.AreEqual("woootooowe", results[1].InnerText);
        }

        [Test]
        public void Star_Attr_Exact_With_Double_Quotes()
        {
            var results = SelectList("*[class=\"checkit\"]");

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("div", results[0].Name);
            Assert.AreEqual("woooeeeee", results[0].InnerText);
            Assert.AreEqual("div", results[1].Name);
            Assert.AreEqual("woootooowe", results[1].InnerText);
        }

        [Test]
        public void Star_Attr_Prefix()
        {
            var results = SelectList("*[class^=check]");

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("div", results[0].Name);
            Assert.AreEqual("woooeeeee", results[0].InnerText);
            Assert.AreEqual("div", results[1].Name);
            Assert.AreEqual("woootooowe", results[1].InnerText);
        }

        [Test]
        public void Star_Attr_Prefix_With_Empty_Value()
        {
            Assert.AreEqual(0, SelectList("*[class^='']").Count);
        }

        [Test]
        public void Star_Attr_Suffix()
        {
            var results = SelectList("*[class$=it]");

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("div", results[0].Name);
            Assert.AreEqual("woooeeeee", results[0].InnerText);
            Assert.AreEqual("div", results[1].Name);
            Assert.AreEqual("woootooowe", results[1].InnerText);
        }

        [Test]
        public void Star_Attr_Suffix_With_Empty_Value()
        {
            Assert.AreEqual(0, SelectList("*[class$='']").Count);
        }

        [Test]
        public void Star_Attr_Substring()
        {
            var results = SelectList("*[class*=heck]");

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("div", results[0].Name);
            Assert.AreEqual("woooeeeee", results[0].InnerText);
            Assert.AreEqual("div", results[1].Name);
            Assert.AreEqual("woootooowe", results[1].InnerText);
        }

        [Test]
        public void Star_Attr_Substring_With_Empty_Value()
        {
            Assert.AreEqual(0, SelectList("*[class*='']").Count);
        }
    }
}