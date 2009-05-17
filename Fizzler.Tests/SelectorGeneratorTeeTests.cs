using System;
using NUnit.Framework;

namespace Fizzler.Tests
{
	/// <summary>
	/// Ensure that all actions on SelectorGeneratorTee are passed
	/// to the internal Primary and Secondary SelectorGenerators.
	/// </summary>
	[TestFixture]
	public class SelectorGeneratorTeeTests : SelectorGeneratorTeeTestsBase
	{
		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void NullPrimary()
		{
			new SelectorGeneratorTee(
				null, new FakeSelectorGenerator()
			);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void NullSecondary()
		{
			new SelectorGeneratorTee(
				new FakeSelectorGenerator(), null
			);
		}

		[Test]
		public void OnInitTest()
		{
			Run(() => Tee.OnInit());
		}

		[Test]
		public void OnCloseTest()
		{
			Run(() => Tee.OnClose());
		}

		[Test]
		public void OnSelectorTest()
		{
			Run(() => Tee.OnSelector());
		}

		[Test]
		public void TypeTest()
		{
			Run(() => Tee.Type("go"));
		}

		[Test]
		public void UniversalTest()
		{
			Run(() => Tee.Universal());
		}

		[Test]
		public void IdTest()
		{
			Run(() => Tee.Id("hello"));
		}

		[Test]
		public void ClassTest()
		{
			Run(() => Tee.Class("hello"));
		}

		[Test]
		public void AttrExistsTest()
		{
			Run(() => Tee.AttributeExists("hello"));
		}

		[Test]
		public void AttExactTest()
		{
			Run(() => Tee.AttributeExact("hello", "there"));
		}

		[Test]
		public void AttrIncludesTest()
		{
			Run(() => Tee.AttributeIncludes("hello", "there"));
		}

		[Test]
		public void AttrDashMatchTest()
		{
			Run(() => Tee.AttributeDashMatch("hello", "there"));
		}

		[Test]
		public void AttrPrefixMatchTest()
		{
			Run(() => Tee.AttributePrefixMatch("hello", "there"));
		}

		[Test]
		public void AttrSuffixMatchTest()
		{
			Run(() => Tee.AttributeSuffixMatch("hello", "there"));
		}

		[Test]
		public void AttrSubstringTest()
		{
			Run(() => Tee.AttributeSubstring("hello", "there"));
		}

		[Test]
		public void FirstChildTest()
		{
			Run(() => Tee.FirstChild());
		}

		[Test]
		public void LastChildTest()
		{
			Run(() => Tee.LastChild());
		}

		[Test]
		public void NthChildTest()
		{
			Run(() => Tee.NthChild(1,2));
		}

		[Test]
		public void OnlyChildTest()
		{
			Run(() => Tee.OnlyChild());
		}

		[Test]
		public void EmptyTest()
		{
			Run(() => Tee.Empty());
		}

		[Test]
		public void ChildTest()
		{
			Run(() => Tee.Child());
		}

		[Test]
		public void DescendantTest()
		{
			Run(() => Tee.Descendant());
		}

		[Test]
		public void AdjacentTest()
		{
			Run(() => Tee.Adjacent());
		}

		[Test]
		public void GeneralSiblingTest()
		{
			Run(() => Tee.GeneralSibling());
		}
	}
}