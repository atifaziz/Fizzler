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
    public class NthChild : SelectorBaseTest
    {
        /// <summary>
        /// Behaves the same as *:nth-child(2)
        /// </summary>
        [Test]
        public void No_Prefix_With_Digit()
        {
            var result = SelectList(":nth-child(2)");

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("body", result[0].Name);
            Assert.AreEqual("p", result[1].Name);
            Assert.AreEqual("span", result[2].Name);
            Assert.AreEqual("p", result[3].Name);
        }
    
        [Test]
        public void Star_Prefix_With_Digit()
        {
            var result = SelectList("*:nth-child(2)");

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("body", result[0].Name);
            Assert.AreEqual("p", result[1].Name);
            Assert.AreEqual("span", result[2].Name);
            Assert.AreEqual("p", result[3].Name);
        }

        [Test]
        public void Element_Prefix_With_Digit()
        {
            var result = SelectList("p:nth-child(2)");

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("p", result[0].Name);
            Assert.AreEqual("p", result[1].Name);
        }
    }
}