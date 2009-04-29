using System;
using System.Linq;
using System.Net;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace ConsoleFizzler
{
	class Program
	{
		private static readonly WebClient Webclient = new WebClient();
		private static string _selector;

		static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				Console.WriteLine("Please supply a uri as the first argument and a CSS selector, enclosed in quotes, as the second argument.");
				return;
			}

			if (args[0] == "/?")
			{
				Console.WriteLine("First argument: url. Second argument: css selector enclosed in quotes.");
				return;
			}

			if (args.Length < 2)
			{
				Console.WriteLine("Please supply a selector as the second argument.");
				return;
			}

			if (args.Length > 2)
			{
				Console.WriteLine("You only need two arguments. Perhaps you forgot to enclose the second selector argument in \"quotes\"?");
				return;
			}

			string rawuri = args[0];
			_selector = args[1];

			if (!Uri.IsWellFormedUriString(rawuri, UriKind.Absolute))
			{
				Console.WriteLine("Your url is invalid.");
				return;
			}

			RunUri(rawuri);
		}

		private static void RunUri(string rawuri)
		{
			Console.WriteLine("Please wait...");

			Uri uri = new Uri(rawuri);

			string result = Webclient.DownloadString(uri);

			var document = new HtmlDocument();
			document.LoadHtml(result);

			var generator = new SelectorGenerator<HtmlNode>(new HtmlNodeOps());
			var helper = new HumanReadableSelectorGenerator();

			Parser.Parse(_selector, new SelectorGeneratorTee(generator, helper));
			HtmlNode[] nodes = generator.Selector(Enumerable.Repeat(document.DocumentNode, 1)).ToArray();

			Console.WriteLine(helper.Selector);

			foreach (var node in nodes)
			{
				Console.WriteLine(node.OuterHtml);
			}
		}
	}
}
