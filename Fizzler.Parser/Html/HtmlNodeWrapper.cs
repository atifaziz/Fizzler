using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Fizzler.Parser.Html
{
    /// <summary>
    /// Wraps an instance of HtmlNode in order to implement the IDocumentNode interface
    /// </summary>
    public class HtmlNodeWrapper : IDocumentNode
    {
        private HtmlNode _htmlNode;

        public HtmlNodeWrapper(HtmlNode htmlNode)
        {
            _htmlNode = htmlNode;
        }

        public List<IDocumentNode> ChildNodes
        {
            get
            {
                HtmlNodeCollection htmlNodes = _htmlNode.ChildNodes;

                List<IDocumentNode> documentNodes = new List<IDocumentNode>();
                foreach (HtmlNode htmlNode in htmlNodes)
                {
                    documentNodes.Add(new HtmlNodeWrapper(htmlNode));
                }

                return documentNodes;
            }
        }

        public HtmlNode HtmlNode
        {
            get
            {
                return _htmlNode;
            }
        }

        public string Name
        {
            get
            {
                return _htmlNode.Name;
            }
        }

        public string Id
        {
            get
            {
                return _htmlNode.Attributes["id"]!=null ? _htmlNode.Attributes["id"].Value : null;
            }
        }

        public string Class
        {
            get
            {
                return _htmlNode.Attributes["class"] != null ? _htmlNode.Attributes["class"].Value : null;
            }
        }

        public IDocumentNode ParentNode
        {
            get
            {
                return _htmlNode.ParentNode!=null ? new HtmlNodeWrapper(_htmlNode.ParentNode) : null;
            }
        }

        public IDocumentNode PreviousSibling
        {
            get
            {
                return _htmlNode.PreviousSibling!=null ? new HtmlNodeWrapper(_htmlNode.PreviousSibling) : null;
            }
        }

        public bool IsElement
        {
            get
            {
                return _htmlNode.NodeType == HtmlNodeType.Element;
            }
        }

        public string InnerText
        {
            get
            {
                return _htmlNode.InnerText;
            }
        }

        public override bool Equals(object obj)
        {
            HtmlNodeWrapper otherNode = obj as HtmlNodeWrapper;
            return otherNode!=null ? otherNode._htmlNode.Equals(_htmlNode) : false;
        }

        public override int GetHashCode()
        {
            return _htmlNode.GetHashCode();
        }

        public IAttributeCollection Attributes
        {
            get { return new HtmlAttributeWrapperCollection(_htmlNode.Attributes); }
        }

        public class HtmlAttributeWrapperCollection : IAttributeCollection
        {
            HtmlAttributeCollection _htmlAttributeCollection;

            public HtmlAttributeWrapperCollection(HtmlAttributeCollection htmlAttributeCollection)
            {
                _htmlAttributeCollection = htmlAttributeCollection;
            }
        

            public IAttribute  this[string name]
            {
	            get
                {
                    HtmlAttribute attribute = _htmlAttributeCollection[name];
                    return attribute != null ? new HtmlAttributeWrapper(attribute) : null;
                }
            }

        }

        public class HtmlAttributeWrapper : IAttribute
        {
            HtmlAttribute _attribute;

            public HtmlAttributeWrapper(HtmlAttribute attribute)
            {
                _attribute = attribute;
            }

            public string Value
            {
                get { return _attribute.Value; }
            }
        }
    }
}
