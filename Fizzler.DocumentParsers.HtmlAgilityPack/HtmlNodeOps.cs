using System.Linq;
using Fizzler.Parser;
using HtmlAgilityPack;

namespace Fizzler.DocumentParsers.HtmlAgilityPack
{
    /// <summary>FIXDOC</summary>
    public class HtmlNodeOps : INodeOps<HtmlNode>
    {
        /// <summary>FIXDOC</summary>
        public Selector<HtmlNode> Distinct()
        {
            return nodes => nodes.Distinct();
        }

        /// <summary>FIXDOC</summary>
        public Selector<HtmlNode> Type(string type)
        {
            return nodes => nodes.Elements().Where(n => n.Name == type);
        }

        /// <summary>FIXDOC</summary>
        public Selector<HtmlNode> Universal()
        {
            return nodes => nodes.Elements();
        }

        /// <summary>FIXDOC</summary>
        public Selector<HtmlNode> Id(string id)
        {
            return nodes => nodes.Elements().Where(n => n.Id == id);
        }

        /// <summary>FIXDOC</summary>
        public Selector<HtmlNode> Class(string clazz)
        {
            return nodes => nodes.Elements().Where(n => n.GetAttributeValue("class", string.Empty)
                                                         .Split(' ')
                                                         .Contains(clazz));
        }

        /// <summary>FIXDOC</summary>
        public Selector<HtmlNode> AttributeExists(string name)
        {
            return nodes => nodes.Elements().Where(n => n.Attributes[name] != null);
        }

        /// <summary>FIXDOC</summary>
        public Selector<HtmlNode> AttributeExact(string name, string value)
        {
            return nodes => from n in nodes.Elements()
                            let a = n.Attributes[name]
                            where a != null && a.Value == value
                            select n;
        }

        /// <summary>FIXDOC</summary>
        public Selector<HtmlNode> AttributeIncludes(string name, string value)
        {
            return nodes => from n in nodes.Elements()
                            let a = n.Attributes[name]
                            where a != null && a.Value.Split(' ').Contains(value)
                            select n;
        }

        /// <summary>FIXDOC</summary>
        public Selector<HtmlNode> AttributeDashMatch(string name, string value)
        {
            return nodes => from n in nodes.Elements()                            
                            let a = n.Attributes[name]
                            where a != null && a.Value.Split('-').Contains(value)
                            select n;
        }

        /// <summary>FIXDOC</summary>
        public Selector<HtmlNode> FirstChild()
        {
            return nodes => nodes.Where(n => !n.ElementsBeforeSelf().Any());
        }

        /// <summary>FIXDOC</summary>
        public Selector<HtmlNode> LastChild()
        {
            return nodes => nodes.Where(n => n.ParentNode.NodeType != HtmlNodeType.Document 
                                          && !n.ElementsAfterSelf().Any());
        }

        /// <summary>FIXDOC</summary>
        public Selector<HtmlNode> NthChild(int position)
        {
            return nodes => from n in nodes
                            let elements = n.ParentNode.Elements().Take(position).ToArray()
                            where elements.Length == position && elements.Last().Equals(n)
                            select n;
        }

        /// <summary>FIXDOC</summary>
        public Selector<HtmlNode> OnlyChild()
        {
            return nodes => nodes.Where(n => n.ParentNode.NodeType != HtmlNodeType.Document
                                          && !n.ElementsAfterSelf().Concat(n.ElementsBeforeSelf()).Any());
        }

        /// <summary>FIXDOC</summary>
        public Selector<HtmlNode> Empty()
        {
            return nodes => nodes.Elements().Where(n => n.ChildNodes.Count == 0);
        }

        /// <summary>FIXDOC</summary>
        public Selector<HtmlNode> Child()
        {
            return nodes => nodes.SelectMany(n => n.Elements());
        }

        /// <summary>FIXDOC</summary>
        public Selector<HtmlNode> Descendant()
        {
            return nodes => nodes.SelectMany(n => n.Descendants().Elements());
        }

        /// <summary>FIXDOC</summary>
        public Selector<HtmlNode> Adjacent()
        {
            return nodes => nodes.SelectMany(n => n.ElementsAfterSelf().Take(1));
        }
    }
}
