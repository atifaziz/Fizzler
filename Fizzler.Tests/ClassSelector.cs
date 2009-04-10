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
			var result = Parser.Select(".checkit");

			Assert.AreEqual(2, result.Count);
			Assert.AreEqual("div", result[0].Name);
			Assert.AreEqual("div", result[1].Name);
		}
		
		/// <summary>
		/// Should match class="omg ohyeah"
		/// </summary>
		[TestMethod]
		public void Chained()
		{
		    var result = Parser.Select(".omg.ohyeah");

		    Assert.AreEqual(1, result.Count);
		    Assert.AreEqual("p", result[0].Name);
		    Assert.AreEqual("eeeee", result[0].InnerText);
		}

		[TestMethod]
		public void With_Element()
		{
			var result = Parser.Select("p.ohyeah");

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("p", result[0].Name);
			Assert.AreEqual("eeeee", result[0].InnerText);
		}

		[TestMethod]
		public void Parent_Class_Selector()
		{
			var result = Parser.Select("div .ohyeah");

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("p", result[0].Name);
			Assert.AreEqual("eeeee", result[0].InnerText);
		}
	}
}