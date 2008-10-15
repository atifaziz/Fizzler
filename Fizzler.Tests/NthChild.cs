using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
	[TestClass]
	public class NthChild : SelectorBaseTest
	{
		/// <summary>
		/// Behaves the same as *:nth-child(2)
		/// </summary>
		[TestMethod]
		public void No_Prefix_With_Digit()
		{
			var result = Parser.Parse(":nth-child(2)");

			Assert.AreEqual(4, result.Count);
			Assert.AreEqual("body", result[0].Name);
			Assert.AreEqual("p", result[1].Name);
			Assert.AreEqual("span", result[2].Name);
			Assert.AreEqual("p", result[3].Name);
		}
	
		[TestMethod]
		public void Star_Prefix_With_Digit()
		{
			var result = Parser.Parse("*:nth-child(2)");

			Assert.AreEqual(4, result.Count);
			Assert.AreEqual("body", result[0].Name);
			Assert.AreEqual("p", result[1].Name);
			Assert.AreEqual("span", result[2].Name);
			Assert.AreEqual("p", result[3].Name);
		}

		[TestMethod]
		public void Element_Prefix_With_Digit()
		{
			var result = Parser.Parse("p:nth-child(2)");

			Assert.AreEqual(2, result.Count);
			Assert.AreEqual("p", result[0].Name);
			Assert.AreEqual("p", result[1].Name);
		}
	}
}