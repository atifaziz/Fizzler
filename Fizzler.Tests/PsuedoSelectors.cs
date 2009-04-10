using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
	[TestClass]
	public class PsuedoSelectors : SelectorBaseTest
	{
		[TestMethod]public void First_Child()
		{
			Assert.AreEqual(7, Parser.Select("*:first-child").Count);
			Assert.AreEqual(1, Parser.Select("p:first-child").Count);
		}
		
		[TestMethod]public void Last_Child()
		{
			Assert.AreEqual(6, Parser.Select("*:last-child").Count);
			Assert.AreEqual(2, Parser.Select("p:last-child").Count);
		}
		
		[TestMethod]public void Only_Child()
		{
			Assert.AreEqual(2, Parser.Select("*:only-child").Count);
			Assert.AreEqual(1, Parser.Select("p:only-child").Count);
		}
		[TestMethod]public void Empty()
		{
			var results = Parser.Select("*:empty");
			Assert.AreEqual(1, results.Count);
			Assert.AreEqual("head", results[0].Name);
		}
	}
}