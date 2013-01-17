namespace Fizzler.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class MultipleSelectors : SelectorBaseTest
    {
        [Test]
        public void CommaSupport_With_No_Space()
        {
            var result = SelectList("p.hiclass,a");

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("p", result[0].Name);
            Assert.AreEqual("a", result[1].Name);
        }

        [Test]
        public void CommaSupport_With_Post_Pended_Space()
        {
            var result = SelectList("p.hiclass, a");
            
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("p", result[0].Name);
            Assert.AreEqual("a", result[1].Name);
        }

        [Test]
        public void CommaSupport_With_Pre_Post_Pended_Space()
        {
            var result = SelectList("p.hiclass , a");

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("p", result[0].Name);
            Assert.AreEqual("a", result[1].Name);
        }

        [Test]
        public void CommaSupport_With_Pre_Pended_Space()
        {
            var result = SelectList("p.hiclass ,a");

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("p", result[0].Name);
            Assert.AreEqual("a", result[1].Name);
        }
    }
}