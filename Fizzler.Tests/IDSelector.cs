using Fizzler.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
	[TestClass]
	public class IDSelector : SelectorBaseTest
	{
		[TestMethod]
		public void Basic_Selector()
		{
			var result = Parser.Select("#myDiv");
			
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("div", result[0].Name);
		}

		[TestMethod]
		public void With_Element()
		{
			var result = Parser.Select("div#myDiv");

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("div", result[0].Name);
		}
		
		[TestMethod]
		public void With_Existing_ID_Descendant()
		{
			var result = Parser.Select("#theBody #myDiv");

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("div", result[0].Name);
		}

		[TestMethod]
		public void With_Non_Existant_ID_Descendant()
		{
			var result = Parser.Select("#theBody #whatwhatwhat");

			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void With_Non_Existant_ID_Ancestor()
		{
			var result = Parser.Select("#whatwhatwhat #someOtherDiv");

			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void All_Descendants_Of_ID()
		{
			var result = Parser.Select("#myDiv *");

			Assert.AreEqual(5, result.Count);
			Assert.AreEqual("div", result[0].Name);
			Assert.AreEqual("p", result[1].Name);
		}
		
		[TestMethod]
		public void Child_ID()
		{
			var result = Parser.Select("#theBody>#myDiv");

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("div", result[0].Name);
		}

		[TestMethod]
		public void Not_A_Child_ID()
		{
			var result = Parser.Select("#theBody>#someOtherDiv");

			Assert.AreEqual(0, result.Count);
		}
		
		[TestMethod]
		public void All_Children_Of_ID()
		{
			var result = Parser.Select("#myDiv>*");

			Assert.AreEqual(2, result.Count);
			Assert.AreEqual("div", result[0].Name);
			Assert.AreEqual("p", result[1].Name);
		}
		
		[TestMethod]
		public void All_Children_of_ID_with_no_children()
		{
			var result = Parser.Select("#someOtherDiv>*");

			Assert.AreEqual(0, result.Count);
		}
	}
}