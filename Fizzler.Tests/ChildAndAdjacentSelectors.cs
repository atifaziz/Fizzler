using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
	[TestClass]
	public class ChildAndAdjacentSelectors : SelectorBaseTest
	{	
		[TestMethod]
		public void Child_With_Pre_And_Post_Space()
		{
			Assert.AreEqual(2, Parser.Select("div > p").Count);
		}

		[TestMethod]
		public void Child_With_Post_Space()
		{
			Assert.AreEqual(2, Parser.Select("div> p").Count);
		}

		[TestMethod]
		public void Child_With_Pre_Space()
		{
			Assert.AreEqual(2, Parser.Select("div >p").Count);
		}

		[TestMethod]
		public void Child_With_No_Space()
		{
			Assert.AreEqual(2, Parser.Select("div>p").Count);
		}

		[TestMethod]
		public void Child_With_Class()
		{
			Assert.AreEqual(1, Parser.Select("div > p.ohyeah").Count);
		}

		[TestMethod]
		public void All_Children()
		{
			// match <a href="">hi</a><span>test</span> so that's 3
			Assert.AreEqual(3, Parser.Select("p > *").Count);
		}

		[TestMethod]
		public void All_GrandChildren()
		{
			// match <a href="">hi</a><span>test</span> so that's 3
			// *any* second level children under any div
			Assert.AreEqual(3, Parser.Select("div > * > *").Count);
		}

		[TestMethod]
		public void Adjacent_With_Pre_And_Post_Space()
		{
			Assert.AreEqual(1, Parser.Select("a + span").Count);
		}

		[TestMethod]
		public void Adjacent_With_Post_Space()
		{
			Assert.AreEqual(1, Parser.Select("a+ span").Count);
		}

		[TestMethod]
		public void Adjacent_With_Pre_Space()
		{
			Assert.AreEqual(1, Parser.Select("a +span").Count);
		}

		[TestMethod]
		public void Adjacent_With_No_Space()
		{
			Assert.AreEqual(1, Parser.Select("a+span").Count);
		}

		[TestMethod]
		public void Comma_Child_And_Adjacent()
		{
			Assert.AreEqual(3, Parser.Select("a + span, div > p").Count);
		}
	}	
}