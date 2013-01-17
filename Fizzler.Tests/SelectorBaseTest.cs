namespace Fizzler.Tests
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Fizzler.Systems.HtmlAgilityPack;
    using HtmlAgilityPack;

    #endregion
    
    public abstract class SelectorBaseTest
    {
        protected SelectorBaseTest()
        {
            string html;
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "Fizzler.Tests.SelectorTest.html";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new Exception(string.Format("Resource, named {0}, not found.", resourceName));
                using(var reader = new StreamReader(stream))
                    html = reader.ReadToEnd();
            }
            var document = new HtmlDocument();
            document.LoadHtml2(html);
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