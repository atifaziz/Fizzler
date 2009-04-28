using NUnit.Framework;

namespace Fizzler.Tests
{
	[TestFixture]
	public class HumanReadableSelectorGeneratorTests
	{
		private readonly HumanReadableSelectorGenerator _humanReadableSelectorGenerator = new HumanReadableSelectorGenerator();

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

		[Test]
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

		private void Run(string selector, string message)
		{
			Parser.Parse(selector, _humanReadableSelectorGenerator);

			Assert.AreEqual(message, _humanReadableSelectorGenerator.Selector);
		}
	}
}