
namespace Fizzler.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class NthLastChild : SelectorBaseTest
    {
        [Test]
        public void No_Prefix_With_Digit()
        {
            var result = SelectList(":nth-last-child(2)");

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("head", result[0].Name);
            Assert.AreEqual("div", result[1].Name);
            Assert.AreEqual("span", result[2].Name);
            Assert.AreEqual("div", result[3].Name);
        }

        [Test]
        public void Id_Prefix_With_Digit()
        {
            var result = SelectList("#myDiv :nth-last-child(2)");

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("div", result[0].Name);
            Assert.AreEqual("span", result[1].Name);
        }

        [Test]
        public void Element_Prefix_With_Digit()
        {
            var result = SelectList("span:nth-last-child(3)"); 

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void Element_Prefix_With_Digit2()
        {
            var result = SelectList("span:nth-last-child(2)");
 
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("span", result[0].Name);
        }
    }
}
