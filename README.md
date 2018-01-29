# Fizzler: .NET CSS Selector Engine

A .NET library to select items from a node tree based on a CSS selector.
The [default implementation][fizzhap] is based on [HTMLAgilityPack][hap] and
selects from HTML documents. The unit tests are based on the jQuery
selector engine tests.

Fizzler supports .NET Standard 1.0 and later versions.

Contributions are welcome in forms of:

  * Increased selector support
  * Implementation over an HTML-like hierarchical document model
  * Re-factorings
  * Improved tests

## Examples

```c#
// Load the document using HTMLAgilityPack as normal
var html = new HtmlDocument();
html.LoadHtml(@"
  <html>
      <head></head>
      <body>
        <div>
          <p class='content'>Fizzler</p>
          <p>CSS Selector Engine</p></div>
      </body>
  </html>");

// Fizzler for HtmlAgilityPack is implemented as the
// QuerySelectorAll extension method on HtmlNode

var document = html.DocumentNode;

// yields: [<p class="content">Fizzler</p>]
document.QuerySelectorAll(".content");

// yields: [<p class="content">Fizzler</p>,<p>CSS Selector Engine</p>]
document.QuerySelectorAll("p");

// yields empty sequence
document.QuerySelectorAll("body>p");

// yields [<p class="content">Fizzler</p>,<p>CSS Selector Engine</p>]
document.QuerySelectorAll("body p");

// yields [<p class="content">Fizzler</p>]
document.QuerySelectorAll("p:first-child");
```


  [fizzhap]: http://www.nuget.org/packages/Fizzler.Systems.HtmlAgilityPack/
  [hap]: http://html-agility-pack.net/
