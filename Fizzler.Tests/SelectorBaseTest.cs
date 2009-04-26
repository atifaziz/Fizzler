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
	    private readonly ISelectable _source;

	    protected SelectorBaseTest()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			Stream stream = assembly.GetManifestResourceStream("Fizzler.Tests.Data.SelectorTest.html"); 
			StreamReader streamReader = new StreamReader(stream);
			_html = streamReader.ReadToEnd();

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(_html);
                        			
			_parser = new SelectorEngine();
		    _source = _parser.ToSelectable(new HtmlNodeWrapper(htmlDocument.DocumentNode.SelectSingleNode("html")));
		}

	    protected SelectorEngine Parser
		{
			get { return _parser; }
		}

	    public ISelectable Source
	    {
	        get { return _source; }
	    }

	    protected string Html
		{
			get { return _html; }
		}

        protected IEnumerable<IDocumentNode> Select(string selectorChain)
        {
            //return Source.Select(selectorChain);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(_html);
            return htmlDocument.DocumentNode.QuerySelectorAll(selectorChain).Select(n => new HtmlNodeWrapper(n)).Cast<IDocumentNode>();
        }

        protected IList<IDocumentNode> SelectList(string selectorChain)
        {
            return new ReadOnlyCollection<IDocumentNode>(Select(selectorChain).ToArray());
        }
	}
}