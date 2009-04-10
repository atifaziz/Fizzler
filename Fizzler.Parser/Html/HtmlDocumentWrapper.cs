using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Fizzler.Parser.Html
{
    /// <summary>
    /// Wraps an instance of HtmlDocument in order to implement the IDocument interface
    /// </summary>
    public class HtmlDocumentWrapper : IDocument
    {
        private HtmlDocument _htmlDocument;

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
