using NUnit.Framework;

namespace Fizzler.Tests
{
    [TestFixture, Ignore("http://code.google.com/p/fizzler/issues/detail?id=24")]
	public class PsuedoSelectors : SelectorBaseTest
	{
		[Test]
		public void First_Child()
		{
			Assert.AreEqual(8, SelectList("*:first-child").Count);
			Assert.AreEqual(1, SelectList("p:first-child").Count);
		}

		[Test]
		public void Last_Child()
		{
			Assert.AreEqual(7, SelectList("*:last-child").Count);
			Assert.AreEqual(2, SelectList("p:last-child").Count);
		}

		[Test]
		public void Only_Child()
		{
			Assert.AreEqual(3, SelectList("*:only-child").Count);
			Assert.AreEqual(1, SelectList("p:only-child").Count);
		}

		[Test]
		public void Empty()
		{
			var results = SelectList("*:empty");
			Assert.AreEqual(2, results.Count);
			Assert.AreEqual("head", results[0].Name);
			Assert.AreEqual("input", results[1].Name);
		}
	}
}