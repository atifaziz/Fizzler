namespace Fizzler.Tests
{
    using NUnit.Framework;

    [TestFixture]
	public class ChildAndAdjacentSelectors : SelectorBaseTest
	{	
		[Test]
		public void Child_With_Pre_And_Post_Space()
		{
			Assert.AreEqual(2, SelectList("div > p").Count);
		}

		[Test]
		public void Child_With_Post_Space()
		{
			Assert.AreEqual(2, SelectList("div> p").Count);
		}

		[Test]
		public void Child_With_Pre_Space()
		{
			Assert.AreEqual(2, SelectList("div >p").Count);
		}

		[Test]
		public void Child_With_No_Space()
		{
			Assert.AreEqual(2, SelectList("div>p").Count);
		}

		[Test]
		public void Child_With_Class()
		{
			Assert.AreEqual(1, SelectList("div > p.ohyeah").Count);
		}

		[Test]
		public void All_Children()
		{
			// match <a href="">hi</a><span>test</span> so that's 3
			Assert.AreEqual(3, SelectList("p > *").Count);
		}

		[Test]
		public void All_GrandChildren()
		{
			// match <a href="">hi</a><span>test</span> so that's 3
			// *any* second level children under any div
			Assert.AreEqual(3, SelectList("div > * > *").Count);
		}

		[Test]
		public void Adjacent_With_Pre_And_Post_Space()
		{
			Assert.AreEqual(1, SelectList("a + span").Count);
		}

		[Test]
		public void Adjacent_With_Post_Space()
		{
			Assert.AreEqual(1, SelectList("a+ span").Count);
		}

		[Test]
		public void Adjacent_With_Pre_Space()
		{
			Assert.AreEqual(1, SelectList("a +span").Count);
		}

		[Test]
		public void Adjacent_With_No_Space()
		{
			Assert.AreEqual(1, SelectList("a+span").Count);
		}

		[Test]
		public void Comma_Child_And_Adjacent()
		{
			Assert.AreEqual(3, SelectList("a + span, div > p").Count);
		}

		[Test]
		public void General_Sibling_Combinator()
		{
			Assert.AreEqual(1, SelectList("div ~ form").Count);
			Assert.AreEqual("form", SelectList("div ~ form")[0].Name);
		}
	}	
}