using NUnit.Framework;

namespace Fizzler.Tests
{
	[TestFixture]
	public class HumanReadableSelectorGeneratorTests
	{
		[Test]
		public void All_Elements()
		{
			Run("*", "Select any element.");
		}

		[Test]
		public void Tag()
		{
			Run("p", "Select <p>.");
		}

		[Test]
		public void Descendant()
		{
			Run("p a", "Select <p> whose descendant is <a>.");
		}

		[Test]
		public void Three_Levels_Of_Descendant()
		{
			Run("p a img", "Select <p> whose descendant is <a>, that in turn has a descendant that is <img>.");
		}

		[Test]
		public void Attribute()
		{
			Run("a[href]", "Select <a> which has attribute href defined.");
		}

		[Test]
		public void Adjacent()
		{
			Run("a + span", "Select <a> whose next sibling is <span>.");
		}

		[Test]
		public void Id()
		{
			Run("#nodeId", "Select any element with an ID of 'nodeId'.");
		}

		[Test]
		public void SelectorGroup()
		{
			Run("a, span", "Select <a>. Combined with previous, select <span>.");
		}

		private static void Run(string selector, string message)
		{
		    var generator = new HumanReadableSelectorGenerator();
			Parser.Parse(selector, generator);
			Assert.AreEqual(message, generator.Text);
		}
	}
}