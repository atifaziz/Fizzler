using System;
using System.Collections.Generic;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using NUnit.Framework;

namespace Fizzler.Tests
{
	/// <summary>
	/// Some test to catch edge-cases in the Systems.XmlNodeQuery namespace
	/// </summary>
	[TestFixture]
	public class XmlNodeQueryTests : SelectorBaseTest
	{
		[Test]
		public void QuerySelectorTest()
		{
			Assert.AreEqual(Document.QuerySelectorAll("*").First(), Document.QuerySelector("*"));
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void IsElementExtensionNullCheck()
		{
			((XmlNode)null).IsElement();
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ChildrenNodeExtensionNullCheck()
		{
			((XmlNode)null).Children();
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ElementsNodeListExtensionNullCheck()
		{
			((IList<XmlNode>)null).Elements();
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ElementsNodeExtensionNullCheck()
		{
			((XmlNode)null).Elements();
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ElementsAfterSelfNodeExtensionNullCheck()
		{
			((XmlNode)null).ElementsAfterSelf();
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ElementsBeforeSelfNodeExtensionNullCheck()
		{
			((XmlNode)null).ElementsBeforeSelf();
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void NodesAfterSelfNodeExtensionNullCheck()
		{
			((XmlNode)null).NodesAfterSelf();
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void NodesBeforeSelfNodeExtensionNullCheck()
		{
			((XmlNode)null).NodesBeforeSelf();
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void DescendantsNodeExtensionNullCheck()
		{
			((XmlNode)null).Descendants();
		}
	}
}