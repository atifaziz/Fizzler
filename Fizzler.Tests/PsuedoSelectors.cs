using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
	[TestClass]
	public class PsuedoSelectors : SelectorBaseTest
	{
		[TestMethod]public void First_Child()
		{
			Assert.AreEqual(7, SelectList("*:first-child").Count);
			Assert.AreEqual(1, SelectList("p:first-child").Count);
		}
		
		[TestMethod]public void Last_Child()
		{
			Assert.AreEqual(6, SelectList("*:last-child").Count);
			Assert.AreEqual(2, SelectList("p:last-child").Count);
		}
		
		[TestMethod]public void Only_Child()
		{
			Assert.AreEqual(2, SelectList("*:only-child").Count);
			Assert.AreEqual(1, SelectList("p:only-child").Count);
		}
		[TestMethod]public void Empty()
		{
			var results = SelectList("*:empty");
			Assert.AreEqual(1, results.Count);
			Assert.AreEqual("head", results[0].Name);
		}
	}
}