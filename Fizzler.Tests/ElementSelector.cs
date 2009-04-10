using Fizzler.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
	[TestClass]
	public class ElementSelector : SelectorBaseTest
	{
		[TestMethod]
		public void Star()
		{
			Assert.AreEqual(14, Parser.Select("*").Count);
		}
		
		[TestMethod]
		public void Single_Tag_Name()
		{
			Assert.AreEqual(1, Parser.Select("body").Count);
			Assert.AreEqual("body", Parser.Select("body")[0].Name);
		}
		
		[TestMethod]
		public void Single_Tag_Name_Matching_Multiple_Elements()
		{
			Assert.AreEqual(3, Parser.Select("p").Count);
			Assert.AreEqual("p", Parser.Select("p")[0].Name);
			Assert.AreEqual("p", Parser.Select("p")[1].Name);
			Assert.AreEqual("p", Parser.Select("p")[2].Name);
		}
		
		[TestMethod]
		public void Basic_Negative_Precedence()
		{
			Assert.AreEqual(0, Parser.Select("head p").Count);
		}

		[TestMethod]
		public void Basic_Positive_Precedence_Two_Tags()
		{
			Assert.AreEqual(2, Parser.Select("div p").Count);
		}

		[TestMethod]
		public void Basic_Positive_Precedence_Two_Tags_With_Grandchild_Descendant()
		{
			Assert.AreEqual(2, Parser.Select("div a").Count);
		}

		[TestMethod]
		public void Basic_Positive_Precedence_Three_Tags()
		{
			Assert.AreEqual(1, Parser.Select("div p a").Count);
			Assert.AreEqual("a", Parser.Select("div p a")[0].Name);
		}

		[TestMethod]
		public void Basic_Positive_Precedence_With_Same_Tags()
		{
			Assert.AreEqual(1, Parser.Select("div div").Count);
		}
	}
}