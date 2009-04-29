using NUnit.Framework;

namespace Fizzler.Tests
{
	[TestFixture]
	public class HumanReadableSelectorGeneratorTests
	{
		[Test]
		public void All_Nodes()
		{
			Run("*", "Select all nodes.");
		}

		[Test]
		public void Tag()
		{
			Run("p", "Select all nodes with the <p> tag.");
		}

		[Test]
		public void Descendant()
		{
			Run("p a", "Select all nodes with the <p> tag which have descendants with the <a> tag.");
		}

		[Test,Ignore("Pending issue #16 fix.")]
		public void Three_Levels_Of_Descendant()
		{
			Run("p a img", "Select all nodes with the <p> tag which have descendants with the <a> tag, which in turn have descendants with the <img> tag.");
		}

		[Test]
		public void Attribute()
		{
			Run("a[href]", "Select all nodes with the <a> tag which have a href attribute.");
		}

		[Test]
		public void Adjacent()
		{
			Run("a + span", "Select all nodes with the <a> tag which is immediately preceeded by a sibling node with the <span> tag.");
		}

		[Test]
		public void Id()
		{
			Run("#nodeId", "Select all nodes with an id of 'nodeId'.");
		}

		[Test]
		public void SelectorGroup()
		{
			Run("a, span", "Select all nodes with the <a> tag, then combined with previous, select all nodes with the <span> tag.");
		}

		private static void Run(string selector, string message)
		{
		    var generator = new HumanReadableSelectorGenerator();
			Parser.Parse(selector, generator);
			Assert.AreEqual(message, generator.Selector);
		}
	}
}