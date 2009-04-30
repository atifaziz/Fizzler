using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace ConsoleFizzler
{
    internal sealed class SelectCommand : Command
    {
        private string _separator;

        public bool Colorful { get; set; }
        public ConsoleColor? TagNameColor { get; set; }
        public ConsoleColor? TagColor { get; set; }
        public ConsoleColor? AttributeNameColor { get; set; }
        public ConsoleColor? AttributeValueColor { get; set; }
        public ConsoleColor? AttributeColor { get; set; }
        public ConsoleColor? CommentColor { get; set; }        
        
        public string Separator
        {
            get { return _separator ?? string.Empty; }
            set { _separator = value; }
        }

        public bool LineInfo { get; set; }

        public NodeOutputFormat OutputFormat { get; set; }

        protected override int OnRun(string[] args)
        {
            if (OutputFormat != NodeOutputFormat.Default
                && OutputFormat != NodeOutputFormat.Full)
                Console.Error.WriteLine("WARNING! The output format option is not yet supported.");

            var arg = ((IEnumerable<string>)args).GetEnumerator();
            
            if (!arg.MoveNext())
                throw new ApplicationException("Missing CSS selector.");

            var selector = arg.Current;

            var document = new HtmlDocument();
            if(!arg.MoveNext() || arg.Current == "-")
                document.LoadHtml(Console.In.ReadToEnd());
            else
                document.Load(arg.Current);

            var i = 0;
            foreach (var node in document.DocumentNode.QuerySelectorAll(selector))
            {
                if (i > 0 && Separator.Length > 0) 
                    Console.WriteLine(Separator);
                
                if (LineInfo)
                    Console.Write("@{0},{1}: ", node.Line, node.LinePosition);

                Output(node);
                Console.WriteLine();
                i++;
            }

            //
            // Exit code = 0 (found) or 1 (not found)
            //

            return i > 0 ? 0 : 1;
        }
        
        private void Output(HtmlNode node)
        {
            var color = Console.ForegroundColor;

            try
            {
                OutputImpl(node, color);
            }
            finally
            {
                if (Colorful) 
                    Console.ForegroundColor = color;
            }    
        }

        private void OutputImpl(HtmlNode node, ConsoleColor color)
        {
            Debug.Assert(node.NodeType == HtmlNodeType.Element);

            Write(TagColor ?? color, "<");
            Write(TagNameColor ?? TagColor ?? color, node.Name);

            foreach (var attribute in node.Attributes)
            {
                Write(color, " ");
                Write(AttributeNameColor ?? AttributeColor ?? color, attribute.Name);
                Write(AttributeColor ?? color, "=\"");
                Write(AttributeValueColor ?? AttributeColor ?? color, HtmlDocument.HtmlEncode(attribute.Value));
                Write(AttributeColor ?? color, "\"");
            }

            var descendants = node.Descendants();
            if (!descendants.Any())
            {
                Write(color, " ");
                Write(TagColor ?? color, "/>");
            }
            else
            {
                Write(TagColor ?? color, ">");

                foreach (var descendant in descendants)
                {
                    var type = descendant.NodeType;
                    switch(type)
                    {
                        case HtmlNodeType.Element:
                            Output(descendant);
                            break;
                        case HtmlNodeType.Comment:
                            Write(CommentColor ?? color, descendant.OuterHtml);
                            break;
                        default:
                            Write(color, descendant.OuterHtml);
                            break;
                    }
                }

                Write(TagColor ?? color, "</");
                Write(TagNameColor ?? TagColor ?? color, node.Name);
                Write(TagColor ?? color, ">");
            }
        }

        private void Write(ConsoleColor color, params string[] strings)
        {
            if (Colorful)
                Console.ForegroundColor = color;

            Array.ForEach(strings, Console.Write);
        }
    }
}