using System.IO;
using System.Reflection;
using Fizzler.DocumentParsers.HtmlAgilityPack;
using Fizzler.Parser;
using HtmlAgilityPack;

namespace Fizzler.Tests
{
	public abstract class SelectorBaseTest
	{
		private readonly string _html;
		private readonly SelectorEngine _parser;

		protected SelectorBaseTest()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			Stream stream = assembly.GetManifestResourceStream("Fizzler.Tests.Data.SelectorTest.html"); 
			StreamReader streamReader = new StreamReader(stream);
			_html = streamReader.ReadToEnd();

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(_html);
                        			
			_parser = new SelectorEngine(new HtmlNodeWrapper(htmlDocument.DocumentNode.SelectSingleNode("html")));
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