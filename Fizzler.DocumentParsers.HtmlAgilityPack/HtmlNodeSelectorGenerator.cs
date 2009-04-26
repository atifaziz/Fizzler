using Fizzler;
using HtmlAgilityPack;

namespace Fizzler.DocumentParsers.HtmlAgilityPack
{
    public class HtmlNodeSelectorGenerator : SelectorGenerator<HtmlNode>
    {
        public HtmlNodeSelectorGenerator() : 
                base(new HtmlNodeOps()) {}
    }
}