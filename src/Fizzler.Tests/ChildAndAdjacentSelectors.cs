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
    using NUnit.Framework;

    [TestFixture]
    public class ChildAndAdjacentSelectors : SelectorBaseTest
    {
        [Test]
        public void Child_With_Pre_And_Post_Space()
        {
            Assert.AreEqual(2, SelectList("div > p").Count);
        }

        [Test]
        public void Child_With_Post_Space()
        {
            Assert.AreEqual(2, SelectList("div> p").Count);
        }

        [Test]
        public void Child_With_Pre_Space()
        {
            Assert.AreEqual(2, SelectList("div >p").Count);
        }

        [Test]
        public void Child_With_No_Space()
        {
            Assert.AreEqual(2, SelectList("div>p").Count);
        }

        [Test]
        public void Child_With_Class()
        {
            Assert.AreEqual(1, SelectList("div > p.ohyeah").Count);
        }

        [Test]
        public void All_Children()
        {
            // match <a href="">hi</a><span>test</span> so that's 3
            Assert.AreEqual(3, SelectList("p > *").Count);
        }

        [Test]
        public void All_GrandChildren()
        {
            // match <a href="">hi</a><span>test</span> so that's 3
            // *any* second level children under any div
            Assert.AreEqual(3, SelectList("div > * > *").Count);
        }

        [Test]
        public void Adjacent_With_Pre_And_Post_Space()
        {
            Assert.AreEqual(1, SelectList("a + span").Count);
        }

        [Test]
        public void Adjacent_With_Post_Space()
        {
            Assert.AreEqual(1, SelectList("a+ span").Count);
        }

        [Test]
        public void Adjacent_With_Pre_Space()
        {
            Assert.AreEqual(1, SelectList("a +span").Count);
        }

        [Test]
        public void Adjacent_With_No_Space()
        {
            Assert.AreEqual(1, SelectList("a+span").Count);
        }

        [Test]
        public void Comma_Child_And_Adjacent()
        {
            Assert.AreEqual(3, SelectList("a + span, div > p").Count);
        }

        [Test]
        public void General_Sibling_Combinator()
        {
            Assert.AreEqual(1, SelectList("div ~ form").Count);
            Assert.AreEqual("form", SelectList("div ~ form")[0].Name);
        }
    }
}