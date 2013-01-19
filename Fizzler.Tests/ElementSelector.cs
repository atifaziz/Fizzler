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
    public class ElementSelector : SelectorBaseTest
    {
        [Test]
        public void Star()
        {
            Assert.AreEqual(16, SelectList("*").Count);
        }
        
        [Test]
        public void Single_Tag_Name()
        {
            Assert.AreEqual(1, SelectList("body").Count);
            Assert.AreEqual("body", SelectList("body")[0].Name);
        }
        
        [Test]
        public void Single_Tag_Name_Matching_Multiple_Elements()
        {
            Assert.AreEqual(3, SelectList("p").Count);
            Assert.AreEqual("p", SelectList("p")[0].Name);
            Assert.AreEqual("p", SelectList("p")[1].Name);
            Assert.AreEqual("p", SelectList("p")[2].Name);
        }
        
        [Test]
        public void Basic_Negative_Precedence()
        {
            Assert.AreEqual(0, SelectList("head p").Count);
        }

        [Test]
        public void Basic_Positive_Precedence_Two_Tags()
        {
            Assert.AreEqual(2, SelectList("div p").Count);
        }

        [Test]
        public void Basic_Positive_Precedence_Two_Tags_With_Grandchild_Descendant()
        {
            Assert.AreEqual(2, SelectList("div a").Count);
        }

        [Test]
        public void Basic_Positive_Precedence_Three_Tags()
        {
            Assert.AreEqual(1, SelectList("div p a").Count);
            Assert.AreEqual("a", SelectList("div p a")[0].Name);
        }

        [Test]
        public void Basic_Positive_Precedence_With_Same_Tags()
        {
            Assert.AreEqual(1, SelectList("div div").Count);
        }

        /// <summary>
        /// This test covers an issue with HtmlAgilityPack where form childnodes().length == 0.
        /// </summary>
        [Test]
        public void Basic_Positive_Precedence_Within_Form()
        {
            Assert.AreEqual(1, SelectList("form input").Count);
        }

        [Test,ExpectedException(typeof(FormatException))]
        public void Type_Star()
        {
            SelectList("a*");
        }
    }
}