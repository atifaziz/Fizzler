using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
	[TestClass]
	public class ClassSelector : SelectorBaseTest
	{
		[TestMethod]
		public void Basic()
		{
			var result = SelectList(".checkit");

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
		    var result = SelectList(".omg.ohyeah");

		    Assert.AreEqual(1, result.Count);
		    Assert.AreEqual("p", result[0].Name);
		    Assert.AreEqual("eeeee", result[0].InnerText);
		}

		[TestMethod]
		public void With_Element()
		{
			var result = SelectList("p.ohyeah");

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("p", result[0].Name);
			Assert.AreEqual("eeeee", result[0].InnerText);
		}

		[TestMethod]
		public void Parent_Class_Selector()
		{
			var result = SelectList("div .ohyeah");

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("p", result[0].Name);
			Assert.AreEqual("eeeee", result[0].InnerText);
		}
	}
}