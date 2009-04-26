using HtmlAgilityPack;

namespace Fizzler.Systems.HtmlAgilityPack
{
    public class HtmlNodeSelectorGenerator : SelectorGenerator<HtmlNode>
    {
        public HtmlNodeSelectorGenerator() : 
            base(new HtmlNodeOps()) {}
    }
}