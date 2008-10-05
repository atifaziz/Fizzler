using System.IO;
using Fizzle.Parser;

namespace Fizzle.Tests
{
	public abstract class SelectorBaseTest
	{
		private readonly string _html;
		private readonly SelectorEngine _parser;

		protected SelectorBaseTest()
		{
			_html = File.ReadAllText("../../../Fizzle.Tests/Data/SelectorTest.html");
			_parser = new SelectorEngine(Html);
		}

		public SelectorEngine Parser
		{
			get { return _parser; }
		}

		public string Html
		{
			get { return _html; }
		}
	}
}