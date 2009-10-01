using System;
using NUnit.Framework;

namespace Fizzler.Tests
{
	[TestFixture]
	public class ElementSelector : SelectorBaseTest
	{
		[Test]
		public void Star()
		{
			Assert.AreEqual(16, SelectList("*").Count);
		}
		
		[Test]
		public void Single_Tag_Name()
		{
			Assert.AreEqual(1, SelectList("body").Count);
			Assert.AreEqual("body", SelectList("body")[0].Name);
		}
		
		[Test]
		public void Single_Tag_Name_Matching_Multiple_Elements()
		{
			Assert.AreEqual(3, SelectList("p").Count);
			Assert.AreEqual("p", SelectList("p")[0].Name);
			Assert.AreEqual("p", SelectList("p")[1].Name);
			Assert.AreEqual("p", SelectList("p")[2].Name);
		}
		
		[Test]
		public void Basic_Negative_Precedence()
		{
			Assert.AreEqual(0, SelectList("head p").Count);
		}

		[Test]
		public void Basic_Positive_Precedence_Two_Tags()
		{
			Assert.AreEqual(2, SelectList("div p").Count);
		}

		[Test]
		public void Basic_Positive_Precedence_Two_Tags_With_Grandchild_Descendant()
		{
			Assert.AreEqual(2, SelectList("div a").Count);
		}

		[Test]
		public void Basic_Positive_Precedence_Three_Tags()
		{
			Assert.AreEqual(1, SelectList("div p a").Count);
			Assert.AreEqual("a", SelectList("div p a")[0].Name);
		}

		[Test]
		public void Basic_Positive_Precedence_With_Same_Tags()
		{
			Assert.AreEqual(1, SelectList("div div").Count);
		}

		/// <summary>
		/// This test covers an issue with HtmlAgilityPack where form childnodes().length == 0.
		/// </summary>
        [Test]
		public void Basic_Positive_Precedence_Within_Form()
		{
			Assert.AreEqual(1, SelectList("form input").Count);
		}

        [Test,ExpectedException(typeof(FormatException))]
        public void Type_Star()
        {
            SelectList("a*");
        }
    }
}