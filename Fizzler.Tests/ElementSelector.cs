using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
	[TestClass]
	public class ElementSelector : SelectorBaseTest
	{
		[TestMethod]
		public void Star()
		{
			Assert.AreEqual(14, SelectList("*").Count);
		}
		
		[TestMethod]
		public void Single_Tag_Name()
		{
			Assert.AreEqual(1, SelectList("body").Count);
			Assert.AreEqual("body", SelectList("body")[0].Name);
		}
		
		[TestMethod]
		public void Single_Tag_Name_Matching_Multiple_Elements()
		{
			Assert.AreEqual(3, SelectList("p").Count);
			Assert.AreEqual("p", SelectList("p")[0].Name);
			Assert.AreEqual("p", SelectList("p")[1].Name);
			Assert.AreEqual("p", SelectList("p")[2].Name);
		}
		
		[TestMethod]
		public void Basic_Negative_Precedence()
		{
			Assert.AreEqual(0, SelectList("head p").Count);
		}

		[TestMethod]
		public void Basic_Positive_Precedence_Two_Tags()
		{
			Assert.AreEqual(2, SelectList("div p").Count);
		}

		[TestMethod]
		public void Basic_Positive_Precedence_Two_Tags_With_Grandchild_Descendant()
		{
			Assert.AreEqual(2, SelectList("div a").Count);
		}

		[TestMethod]
		public void Basic_Positive_Precedence_Three_Tags()
		{
			Assert.AreEqual(1, SelectList("div p a").Count);
			Assert.AreEqual("a", SelectList("div p a")[0].Name);
		}

		[TestMethod]
		public void Basic_Positive_Precedence_With_Same_Tags()
		{
			Assert.AreEqual(1, SelectList("div div").Count);
		}
	}
}