using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
	[TestClass]
	public class PsuedoSelectors : SelectorBaseTest
	{
		[TestMethod]public void First_Child()
		{
			Assert.AreEqual(7, Parser.Parse("*:first-child").Count);
			Assert.AreEqual(1, Parser.Parse("p:first-child").Count);
		}
		
		[TestMethod]public void Last_Child()
		{
			Assert.AreEqual(6, Parser.Parse("*:last-child").Count);
			Assert.AreEqual(2, Parser.Parse("p:last-child").Count);
		}
		[TestMethod]public void Only_Child(){}
		[TestMethod]public void Empty(){}
		[TestMethod]public void Selected(){}
		[TestMethod]public void Enabled(){}
		[TestMethod]public void Not() { }
		[TestMethod]public void Radio() { }
		[TestMethod]public void Checkbox() { }
		[TestMethod]public void Checked() { }
		[TestMethod]public void Eq() { }
		[TestMethod]public void Gt() { }
		[TestMethod]public void Lt() { }
	}
}