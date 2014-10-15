#region Copyright and License
// 
// Fizzler - CSS Selector Engine for Microsoft .NET Framework
// Copyright (c) 2009 Atif Aziz, Colin Ramsay. All rights reserved.
// 
// This library is free software; you can redistribute it and/or modify it under 
// the terms of the GNU Lesser General Public License as published by the Free 
// Software Foundation; either version 3 of the License, or (at your option) 
// any later version.
// 
// This library is distributed in the hope that it will be useful, but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more 
// details.
// 
// You should have received a copy of the GNU Lesser General Public License 
// along with this library; if not, write to the Free Software Foundation, Inc., 
// 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
// 
#endregion

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