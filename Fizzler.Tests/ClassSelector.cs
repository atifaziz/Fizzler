using Fizzler.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
	[TestClass]
	public class ClassSelector : SelectorBaseTest
	{
		[TestMethod]
		public void Basic()
		{
			var result = Parser.Parse(".checkit");

			Assert.AreEqual(2, result.Count);
			Assert.AreEqual("div", result[0].Name);
			Assert.AreEqual("div", result[1].Name);
		}
		
		[TestMethod]
		public void Chained()
		{
			var result = Parser.Parse(".checkit.ohyeah");

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("p", result[0].Name);
		}
	}
}