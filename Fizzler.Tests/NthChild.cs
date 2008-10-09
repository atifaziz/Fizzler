using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
	[TestClass]
	public class NthChild : SelectorBaseTest
	{
		[TestMethod]
		public void No_Prefix_With_Digit()
		{
			var result = Parser.Parse("*:nth-child(2)");

			Assert.AreEqual(2, result.Count);
			Assert.AreEqual("p", result[0].Name);
			Assert.AreEqual("span", result[1].Name);
		}
	
		[TestMethod]
		public void Star_Prefix_With_Digit()
		{
			var result = Parser.Parse("*:nth-child(2)");

			Assert.AreEqual(2, result.Count);
			Assert.AreEqual("p", result[0].Name);
			Assert.AreEqual("span", result[1].Name);
		}

		[TestMethod]
		public void Element_Prefix_With_Digit()
		{
			var result = Parser.Parse("p:nth-child(2)");

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("span", result[1].Name);
		}
	}
}