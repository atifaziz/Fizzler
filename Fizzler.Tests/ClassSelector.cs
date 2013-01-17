namespace Fizzler.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ClassSelector : SelectorBaseTest
    {
        [Test]
        public void Basic()
        {
            var result = SelectList(".checkit");

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("div", result[0].Name);
            Assert.AreEqual("div", result[1].Name);
        }
        
        /// <summary>
        /// Should match class="omg ohyeah"
        /// </summary>
        [Test]
        public void Chained()
        {
            var result = SelectList(".omg.ohyeah");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("p", result[0].Name);
            Assert.AreEqual("eeeee", result[0].InnerText);
        }

        [Test]
        public void With_Element()
        {
            var result = SelectList("p.ohyeah");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("p", result[0].Name);
            Assert.AreEqual("eeeee", result[0].InnerText);
        }

        [Test]
        public void Parent_Class_Selector()
        {
            var result = SelectList("div .ohyeah");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("p", result[0].Name);
            Assert.AreEqual("eeeee", result[0].InnerText);
        }
    }
}