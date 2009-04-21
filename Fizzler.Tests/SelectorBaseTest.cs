using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Fizzler.DocumentParsers.HtmlAgilityPack;
using Fizzler.Parser;
using Fizzler.Parser.Document;
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

	    protected SelectorEngine Parser
		{
			get { return _parser; }
		}

	    protected string Html
		{
			get { return _html; }
		}

        protected IEnumerable<IDocumentNode> Select(string selectorChain)
        {
            return Parser.Select(selectorChain);
        }

        protected IList<IDocumentNode> SelectList(string selectorChain)
        {
            return new ReadOnlyCollection<IDocumentNode>(Parser.Select(selectorChain).ToArray());
        }
	}
}