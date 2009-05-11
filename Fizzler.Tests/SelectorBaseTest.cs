using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Fizzler.Systems.XmlNodeQuery;
using Sgml;

namespace Fizzler.Tests
{
	public abstract class SelectorBaseTest
	{
		protected SelectorBaseTest()
		{
			var assembly = Assembly.GetExecutingAssembly();
			using (var stream = assembly.GetManifestResourceStream("Fizzler.Tests.SelectorTest.html"))
			using (var streamReader = new StreamReader(stream))
				Document = FromHtml(streamReader);

		}

		private XmlDocument FromHtml(TextReader reader)
		{
			SgmlReader sgmlReader = new SgmlReader();
			sgmlReader.DocType = "HTML";
			sgmlReader.WhitespaceHandling = WhitespaceHandling.All;
			sgmlReader.CaseFolding = CaseFolding.ToLower;
			sgmlReader.InputStream = reader;

			XmlDocument doc = new XmlDocument();
			doc.PreserveWhitespace = true;
			doc.XmlResolver = null;
			doc.Load(sgmlReader);
			return doc;
		}

		protected XmlDocument Document { get; private set; }

		protected IEnumerable<XmlNode> Select(string selectorChain)
		{
			return Document.QuerySelectorAll(selectorChain);
		}

		protected IList<XmlNode> SelectList(string selectorChain)
		{
			return new ReadOnlyCollection<XmlNode>(Select(selectorChain).ToArray());
		}
	}
}