using System.Collections.Generic;
using Fizzler.Parser.Document;
using HtmlAgilityPack;

namespace Fizzler.DocumentParsers.HtmlAgilityPack
{
	/// <summary>
	/// Wraps an instance of HtmlDocument in order to implement the IDocument interface
	/// </summary>
	public class HtmlDocumentWrapper : IDocument
	{
		private readonly HtmlDocument _htmlDocument;

		public HtmlDocumentWrapper(HtmlDocument htmlDocument)
		{
			_htmlDocument = htmlDocument;
		}

		public List<IDocumentNode> ChildNodes
		{
			get { return new HtmlNodeWrapper(_htmlDocument.DocumentNode).ChildNodes; }
		}
	}
}