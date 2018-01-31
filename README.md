# Fizzler: .NET CSS Selector Engine

[![Build Status][win-build-badge]][win-builds]
[![Build Status][nix-build-badge]][nix-builds]
[![NuGet][nuget-badge]][nuget-pkg]
[![MyGet][myget-badge]][edge-pkgs]

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

The following example uses [Fizzler.Systems.HtmlAgilityPack][fizzhap]:

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


[win-build-badge]: https://img.shields.io/appveyor/ci/raboof/fizzler/master.svg?label=windows
[win-builds]: https://ci.appveyor.com/project/raboof/fizzler
[nix-build-badge]: https://img.shields.io/travis/atifaziz/Fizzler/master.svg?label=linux
[nix-builds]: https://travis-ci.org/atifaziz/Fizzler
[myget-badge]: https://img.shields.io/myget/raboof/vpre/Fizzler.svg?label=myget
[edge-pkgs]: https://www.myget.org/feed/raboof/package/nuget/Fizzler
[nuget-badge]: https://img.shields.io/nuget/v/Fizzler.svg
[nuget-pkg]: https://www.nuget.org/packages/Fizzler

[fizzhap]: http://www.nuget.org/packages/Fizzler.Systems.HtmlAgilityPack/
[hap]: http://html-agility-pack.net/
