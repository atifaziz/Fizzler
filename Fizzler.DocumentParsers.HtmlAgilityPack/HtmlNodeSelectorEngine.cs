using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fizzler.Parser;
using HtmlAgilityPack;

namespace Fizzler.DocumentParsers.HtmlAgilityPack
{
    public class HtmlNodeSelectorEngine : ISelectorEngine2
    {
        private static readonly HtmlNodeSelectorEngine _engine = new HtmlNodeSelectorEngine();

        private HtmlNodeSelectorEngine() {}

        public static void Install()
        {
            SelectorEngine2.Install(typeof(HtmlNodeSelectorEngine),
                (context => context == typeof(HtmlNode) || context.GetType() == typeof(HtmlNode) ? _engine : null));
        }

        ISelectorGenerator ISelectorEngine2.CreateGenerator()
        {
            return new HtmlNodeSelectorGenerator();
        }

        object ISelectorEngine2.GetSelector(ISelectorGenerator generator)
        {
            if(generator == null) throw new ArgumentNullException("generator");
            return ((HtmlNodeSelectorGenerator) generator).Selector;
        }

        IEnumerable ISelectorEngine2.Select(object context, object selector)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (selector == null) throw new ArgumentNullException("selector");
            
            var subNode = context as HtmlNode;
            if (subNode == null)
                throw new ArgumentException(null, "context");

            var subSelector = selector as Selector<HtmlNode>;
            if (subSelector == null)
                throw new ArgumentException(null, "selector");

            return subSelector(Enumerable.Repeat(subNode, 1));
        }
    }
}
