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
    public class PsuedoSelectors : SelectorBaseTest
    {
        [Test]
        public void First_Child()
        {
            Assert.AreEqual(8, SelectList("*:first-child").Count);
            Assert.AreEqual(1, SelectList("p:first-child").Count);
        }

        [Test]
        public void Last_Child()
        {
            Assert.AreEqual(7, SelectList("*:last-child").Count);
            Assert.AreEqual(2, SelectList("p:last-child").Count);
        }

        [Test]
        public void Only_Child()
        {
            Assert.AreEqual(3, SelectList("*:only-child").Count);
            Assert.AreEqual(1, SelectList("p:only-child").Count);
        }

        [Test]
        public void Empty()
        {
            var results = SelectList("*:empty");
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("head", results[0].Name);
            Assert.AreEqual("input", results[1].Name);
        }
    }
}