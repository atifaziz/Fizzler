using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace Fizzler.Tests
{
	public abstract class SelectorBaseTest
	{
	    protected SelectorBaseTest()
		{
            string html;
			var assembly = Assembly.GetExecutingAssembly();
			using (var stream = assembly.GetManifestResourceStream("Fizzler.Tests.SelectorTest.html"))
			using (var streamReader = new StreamReader(stream))
			    html = streamReader.ReadToEnd();
            var document = new HtmlDocument();
            document.LoadHtml(html);
            Document = document;
        }

	    protected HtmlDocument Document { get; private set; }

	    protected IEnumerable<HtmlNode> Select(string selectorChain)
        {
            return Document.DocumentNode.QuerySelectorAll(selectorChain);
        }

        protected IList<HtmlNode> SelectList(string selectorChain)
        {
            return new ReadOnlyCollection<HtmlNode>(Select(selectorChain).ToArray());
        }
	}
}