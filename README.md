# Fizzler: .NET CSS Selector Engine

A .NET library to select items from a node tree based on a CSS selector.
The [default implementation][fizzhap] is based on [HTMLAgilityPack][hap] and
selects from HTML documents. There over 140 unit tests - see below for more
information. The tests are based on the jQuery selector engine tests.

Fizzler supports .NET 3.5 and later versions.

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

## Test Overview

View them here ([Fizzler.Tests][tests]):

  * `With_Non_Existant_ID_Descendant`
  * `With_Non_Existant_ID_Ancestor`
  * `With_Existing_ID_Descendant`
  * `With_Element`
  * `With_Element`
  * `Star_Prefix_With_Digit`
  * `Star_NthChild`
  * `Star`
  * `Single_Tag_Name_Matching_Multiple_Elements`
  * `Single_Tag_Name`
  * `Single`
  * `Parent_Class_Selector`
  * `Only_Child`
  * `NthChild_No_Prefix`
  * `Not_A_Child_ID`
  * `No_Prefix_With_Digit	`
  * `Last_Child`
  * `First_Child	`
  * `Empty`
  * `Element_Prefix_With_Digit`
  * `Element_NthChild	`
  * `Element_Attr_Space_Separated_With_Double_Quotes		`
  * `Element_Attr_Space_Separated	`
  * `Element_Attr_Hyphen_Separated_With_Double_Quotes	`
  * `Element_Attr_Hyphen_Separated`
  * `Element_Attr_Exists	`
  * `Element_Attr_Exists	`
  * `Element_Attr_Equals_With_Double_Quotes	`
  * `Element_Attr_Equals	`
  * `Descendant	`
  * `CommaSupport_With_Pre_Post_Pended_Space	`
  * `CommaSupport_With_Pre_Pended_Space`
  * `CommaSupport_With_Post_Pended_Space`
  * `CommaSupport_With_No_Space`
  * `Comma_Child_And_Adjacent	`
  * `Child_With_Spaces`
  * `Child_With_Pre_Space	`
  * `Child_With_Pre_And_Post_Space`
  * `Child_With_Post_Space`
  * `Child_With_No_Space	`
  * `Child_With_Class	`
  * `Child_No_Spaces	`
  * `Child_ID	`
  * `Chained	`
  * `Basic_Selector`
  * `Basic_Positive_Precedence_With_Same_Tags	`
  * `Basic_Positive_Precedence_Two_Tags`
  * `Basic_Positive_Precedence_Three_Tags	`
  * `Basic_Negative_Precedence`
  * `Basic	`
  * `All_GrandChildren`
  * `All_Descendants_Of_ID`
  * `All_Children_of_ID_with_no_children	`
  * `All_Children_Of_ID`
  * `All_Children	`
  * `Adjacent_With_Pre_Space	`
  * `Adjacent_With_Pre_And_Post_Space	`
  * `Adjacent_With_Post_Space	`
  * `Adjacent_With_No_Space	`
  * `Adj_Spaces`
  * `Adj_No_Spaces`

  [fizzhap]: http://www.nuget.org/packages/Fizzler.Systems.HtmlAgilityPack/
  [hap]: http://www.codeplex.com/htmlagilitypack
  [tests]: https://github.com/atifaziz/Fizzler/tree/master/src/Fizzler.Tests