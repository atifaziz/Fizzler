#region Copyright and License
//
// Fizzler - CSS Selector Engine for Microsoft .NET Framework
// Copyright (c) 2009 Atif Aziz, Colin Ramsay. All rights reserved.
//
// This library is free software; you can redistribute it and/or modify it under
// the terms of the GNU Lesser General Public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
// details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this library; if not, write to the Free Software Foundation, Inc.,
// 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
#endregion

namespace Fizzler.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class HumanReadableSelectorGeneratorTests
    {
        public class TestHumanReadableSelectorGenerator : HumanReadableSelectorGenerator
        {
            public new void Add(string selector)
            {
                base.Add(selector);
            }
        }

        [Test]
        public void Null_Selector()
        {
            var generator = new TestHumanReadableSelectorGenerator();
            var e = Assert.Throws<ArgumentNullException>(() => generator.Add(null));
            Assert.That(e.ParamName, Is.EqualTo("selector"));
        }

        [Test]
        public void All_Elements()
        {
            Run("*", "Take all elements and select them.");
        }

        [Test]
        public void Tag()
        {
            Run("p", "Take all <p> elements and select them.");
        }

        [Test]
        public void Descendant()
        {
            Run("p a", "Take all <p> elements, then take their descendants which are <a> elements and select them.");
        }

        [Test]
        public void Three_Levels_Of_Descendant()
        {
            Run("p a img", "Take all <p> elements, then take their descendants which are <a> elements. With those, take only their descendants which are <img> elements and select them.");
        }

        [Test]
        public void Attribute()
        {
            Run("a[href]", "Take all <a> elements which have attribute href defined and select them.");
        }

        [Test]
        public void Adjacent()
        {
            Run("a + span", "Take all <a> elements, then take their immediate siblings which are <span> elements and select them.");
        }

        [Test]
        public void Id()
        {
            Run("#nodeId", "Take all elements with an ID of 'nodeId' and select them.");
        }

        [Test]
        public void SelectorGroup()
        {
            Run("a, span", "Take all <a> elements and select them. Combined with previous, take all <span> elements and select them.");
        }

        [Test]
        public void GeneralSibling()
        {
            Run("div ~ p", "Take all <div> elements, then take their siblings which are <p> elements and select them.");
        }

        [Test]
        public void Empty()
        {
            Run("*:empty", "Take all elements where the element is empty and select them.");
        }

        [Test]
        public void FirstChild()
        {
            Run("*:first-child", "Take all elements which are the first child of their parent and select them.");
        }

        [Test]
        public void Child()
        {
            Run("* > p", "Take all elements, then take their immediate children which are <p> elements and select them.");
        }

        [Test] public void Class()
        {
            Run(".myclass", "Take all elements with a class of 'myclass' and select them.");
        }

        [Test] public void LastChild()
        {
            Run("*:last-child", "Take all elements which are the last child of their parent and select them.");
        }

        [Test]
        public void NthChild()
        {
            Run("*:nth-child(2)", "Take all elements where the element has 1n+2-1 sibling before it and select them.");
        }

        [Test]
        public void OnlyChild()
        {
            Run("*:only-child", "Take all elements where the element is the only child and select them.");
        }

        [Test] public void AttributeDashMatch()
        {
            Run("*[lang|='en']", "Take all elements which have attribute lang with a hyphen separated value matching 'en' and select them.");
        }
        [Test] public void AttributeExact()
        {
            Run("*[title='hithere']", "Take all elements which have attribute title with a value of 'hithere' and select them.");
        }
        [Test] public void AttributeIncludes()
        {
            Run("*[title~='hithere']", "Take all elements which have attribute title that includes the word 'hithere' and select them.");
        }
        [Test] public void AttributeSubstring()
        {
            Run("*[title*='hithere']", "Take all elements which have attribute title whose value contains 'hithere' and select them.");
        }
        [Test] public void AttributeSuffixMatch()
        {
            Run("*[title$='hithere']", "Take all elements which have attribute title whose value ends with 'hithere' and select them.");
        }

        [Test]
        public void AttributePrefixMatch()
        {
            Run("*[title^='hithere']", "Take all elements which have attribute title whose value begins with 'hithere' and select them.");
        }

        private static void Run(string selector, string message)
        {
            var generator = new HumanReadableSelectorGenerator();
            Parser.Parse(selector, generator);
            Assert.AreEqual(message, generator.Text);
        }
    }
}
