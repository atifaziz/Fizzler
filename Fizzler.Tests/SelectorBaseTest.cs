using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Fizzler.DocumentParsers.HtmlAgilityPack;
using Fizzler.Parser.Document;
using HtmlAgilityPack;

namespace Fizzler.Tests
{
	public abstract class SelectorBaseTest
	{
	    protected SelectorBaseTest()
		{
            string html;
			var assembly = Assembly.GetExecutingAssembly();
			using (var stream = assembly.GetManifestResourceStream("Fizzler.Tests.Data.SelectorTest.html"))
			using (var streamReader = new StreamReader(stream))
			    html = streamReader.ReadToEnd();
            var document = new HtmlDocument();
            document.LoadHtml(html);
            Document = document;
        }

	    protected HtmlDocument Document { get; private set; }

	    protected IEnumerable<IDocumentNode> Select(string selectorChain)
        {
            return Document.DocumentNode
                           .QuerySelectorAll(selectorChain)
                           .Select(n => new HtmlNodeWrapper(n)).Cast<IDocumentNode>();
        }

        protected IList<IDocumentNode> SelectList(string selectorChain)
        {
            return new ReadOnlyCollection<IDocumentNode>(Select(selectorChain).ToArray());
        }
	}
}