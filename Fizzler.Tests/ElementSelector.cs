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
			Assert.AreEqual(12, Parser.Parse("*").Count);
		}
		
		[TestMethod]
		public void Single_Tag_Name()
		{
			Assert.AreEqual(1, Parser.Parse("body").Count);
			Assert.AreEqual("body", Parser.Parse("body")[0].Name);
		}
		
		[TestMethod]
		public void Single_Tag_Name_Matching_Multiple_Elements()
		{
			Assert.AreEqual(3, Parser.Parse("p").Count);
			Assert.AreEqual("p", Parser.Parse("p")[0].Name);
			Assert.AreEqual("p", Parser.Parse("p")[1].Name);
			Assert.AreEqual("p", Parser.Parse("p")[2].Name);
		}

		[TestMethod]
		public void Basic_Negative_Precedence()
		{
			Assert.AreEqual(0, Parser.Parse("head p").Count);
		}

		[TestMethod]
		public void Basic_Positive_Precedence()
		{
			Assert.AreEqual(2, Parser.Parse("div p").Count);
		}

		[TestMethod]
		public void Basic_Positive_Precedence_With_Same_Tags()
		{
			Assert.AreEqual(1, Parser.Parse("div div").Count);
		}
	}
}