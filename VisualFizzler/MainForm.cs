using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.VisualBasic;
using HtmlDocument=HtmlAgilityPack.HtmlDocument;

namespace VisualFizzler
{
    public partial class MainForm : Form
    {
        private static readonly Regex _tagExpression = new Regex(@"\<(?:(?<t>[a-z]+)(?:""[^""]*""|'[^']*'|[^""'>])*|/(?<t>[a-z]+))\>",
            RegexOptions.IgnoreCase
            | RegexOptions.Singleline
            | RegexOptions.Compiled
            | RegexOptions.CultureInvariant);

        private HtmlDocument _document;
        private Match[] _selectorMatches;
        private Uri _lastKnownGoodImportedUrl;

        public MainForm()
        {
            InitializeComponent();
        }

        private void FileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FileOpen_Click(object sender, EventArgs e)
        {
            if (_openFileDialog.ShowDialog(this) != DialogResult.OK)
                return;

            var document = new HtmlDocument();
            document.Load(_openFileDialog.FileName);
            Open(document);
        }

        private void ImportFromWebMenu_Click(object sender, EventArgs args)
        {
            Uri url = null;

            var input = _lastKnownGoodImportedUrl != null 
                      ? _lastKnownGoodImportedUrl.ToString() 
                      : string.Empty;

            do
            {
                input = Interaction.InputBox("Enter URL:", "Import From Web", input,
                    (int)(Location.X + (Size.Height / 10f)),
                    (int)(Location.Y + (Size.Height / 10f))).Trim();

                if (string.IsNullOrEmpty(input))
                    return;

                // http://www.shauninman.com/archive/2006/05/08/validating_domain_names

                if (Regex.IsMatch(input, @"^([a-z0-9] ([-a-z0-9]*[a-z0-9])? \.)+ 
                                            ( (a[cdefgilmnoqrstuwxz]|aero|arpa)
                                              |(b[abdefghijmnorstvwyz]|biz)
                                              |(c[acdfghiklmnorsuvxyz]|cat|com|coop)
                                              |d[ejkmoz]
                                              |(e[ceghrstu]|edu)
                                              |f[ijkmor]
                                              |(g[abdefghilmnpqrstuwy]|gov)
                                              |h[kmnrtu]
                                              |(i[delmnoqrst]|info|int)
                                              |(j[emop]|jobs)
                                              |k[eghimnprwyz]
                                              |l[abcikrstuvy]
                                              |(m[acdghklmnopqrstuvwxyz]|mil|mobi|museum)
                                              |(n[acefgilopruz]|name|net)
                                              |(om|org)
                                              |(p[aefghklmnrstwy]|pro)
                                              |qa
                                              |r[eouw]
                                              |s[abcdeghijklmnortvyz]
                                              |(t[cdfghjklmnoprtvwz]|travel)
                                              |u[agkmsyz]
                                              |v[aceginu]
                                              |w[fs]
                                              |y[etu]
                                              |z[amw])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture))
                    input = "http://" + input;

                if (!Uri.IsWellFormedUriString(input, UriKind.Absolute))
                    MessageBox.Show(this, "The entered URL does not appear to be correctly formatted.", "Invalid URL", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    url = new Uri(input, UriKind.Absolute);
            }
            while (url == null);

            try
            {
                using (CurrentCursorScope.EnterWait())
                {
                    var document = new HtmlDocument();
                    document.LoadHtml(new WebClient().DownloadString(url));
                    Open(document);
                    _lastKnownGoodImportedUrl = url;
                }
            }
            catch (WebException e)
            {
                Program.ShowExceptionDialog(e, "Import Error", this);
            }
        }

        private void SelectorBox_TextChanged(object sender, EventArgs e)
        {
            Evaluate();
        }

        private void HelpContents_Click(object sender, EventArgs e)
        {
            Process.Start("http://fizzler.googlecode.com/");
        }

        private void Open(HtmlDocument document)
        {
            _document = document;
            _documentBox.Clear();
            _documentBox.Text = document.DocumentNode.OuterHtml;
            _selectorMatches = null;
            HighlightMarkup(_documentBox, Color.Blue, Color.FromArgb(163, 21, 21), Color.Red);
            Evaluate();
        }

        private static void HighlightMarkup(RichTextBox rtb, Color tagColor, Color tagNameColor, Color attributeNameColor)
        {
            Debug.Assert(rtb != null);

            foreach (Match tag in _tagExpression.Matches(rtb.Text))
            {
                Highlight(rtb, tag.Index, tag.Length, tagColor, null, null);

                var name = tag.Groups["t"];
                Highlight(rtb, name.Index, name.Length, tagNameColor, null, null);

                var attributes = Regex.Matches(tag.Value, 
                    @"\b([a-z]+)\s*=\s*(?:""[^""]*""|'[^']*'|[^'"">\s]+)", 
                    RegexOptions.IgnoreCase
                    | RegexOptions.Singleline
                    | RegexOptions.CultureInvariant);

                foreach (var attribute in attributes.Cast<Match>().Select(m => m.Groups[1]))
                    Highlight(rtb, tag.Index + attribute.Index, attribute.Length, attributeNameColor, null, null);
            }
        }

        private static void Highlight(RichTextBox rtb, IEnumerable<Match> matches, Color? color, Color? backColor, Font font)
        {
            foreach (var match in matches)
                Highlight(rtb, match.Index, match.Length, color, backColor, font);
        }

        private static void Highlight(RichTextBox rtb, int start, int length, Color? color, Color? backColor, Font font)
        {
            rtb.SelectionStart = start;
            rtb.SelectionLength = length;
            if (color != null) rtb.SelectionColor = color.Value;
            if (backColor != null) rtb.SelectionBackColor = backColor.Value;
            if (font != null) rtb.SelectionFont = font;
        }

        private void Evaluate()
        {
            _selectorMatches = Evaluate(_document, _selectorBox, _matchBox, _helpBox, _statusLabel, _selectorMatches, _documentBox);
        }

        private static Match[] Evaluate(HtmlDocument document, Control tb, ListBox lb, Control hb, ToolStripItem status, IEnumerable<Match> oldMatches, RichTextBox rtb)
        {
            var input = tb.Text.Trim();
            tb.ForeColor = SystemColors.WindowText;
            
            var nodes = new HtmlNode[0];
            
            if (string.IsNullOrEmpty(input))
            {
                status.Text = "Ready";
            }
            else
            {
                try
                {
                    //
                    // Simple way to query for nodes:
                    //
                    // nodes = document.DocumentNode.QuerySelectorAll(input).ToArray();
                    //
                    // However, we want to generate the human readable text and
                    // the node selector in a single pass so go the bare metal way 
                    // here to make all the parties to talk to each other.
                    //
                    
                    var generator = new SelectorGenerator<HtmlNode>(new HtmlNodeOps());
                    var helper = new HumanReadableSelectorGenerator();
                    Parser.Parse(input, new SelectorGeneratorTee(generator, helper));
                    if (document != null)
                        nodes = generator.Selector(Enumerable.Repeat(document.DocumentNode, 1)).ToArray();
                    hb.Text = helper.Selector;

                    status.Text = "Matches: " + nodes.Length.ToString("N0");
                }
                catch (FormatException e)
                {
                    tb.ForeColor = Color.FromKnownColor(KnownColor.Red);
                    status.Text = "Error: " + e.Message;
                }
            }
            
            if (oldMatches != null)
                Highlight(rtb, oldMatches, null, SystemColors.Info, null);
        
            lb.BeginUpdate();
            try
            {
                lb.Items.Clear();
                if (!nodes.Any())
                    return new Match[0];

                var html = rtb.Text;
                var matches  = new List<Match>(nodes.Length);
                foreach (var node in nodes)
                {
                    var index = rtb.GetFirstCharIndexFromLine(node.Line - 1) + node.LinePosition - 1;
                    var match = _tagExpression.Match(html, index);
                    if (match.Success)
                        matches.Add(match);
                }
                
                Highlight(rtb, matches, null, Color.Yellow, null);
                
                lb.Items.AddRange(nodes.Select(n => n.GetBeginTagString()).ToArray());
                
                return matches.ToArray();
            }
            finally
            {
                lb.EndUpdate();
            }
        }
    }
}
