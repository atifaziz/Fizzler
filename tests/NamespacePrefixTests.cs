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
    public class NamespacePrefixTests
    {
        [Test]
        public void Initialization()
        {
            Assert.That(new NamespacePrefix("foo").Text, Is.EqualTo("foo"));
        }

        [Test]
        public void NoneText()
        {
            Assert.That(NamespacePrefix.None.Text, Is.Null);
        }

        [Test]
        public void NoneIsNone()
        {
            Assert.That(NamespacePrefix.None.IsNone, Is.True);
        }

        [Test]
        public void NoneIsNotAny()
        {
            Assert.That(NamespacePrefix.None.IsAny, Is.False);
        }

        [Test]
        public void NoneIsNotEmpty()
        {
            Assert.That(NamespacePrefix.None.IsEmpty, Is.False);
        }

        [Test]
        public void NoneIsNotSpecific()
        {
            Assert.That(NamespacePrefix.None.IsSpecific, Is.False);
        }

        [Test]
        public void AnyText()
        {
            Assert.That(NamespacePrefix.Any.Text, Is.EqualTo("*"));
        }

        [Test]
        public void AnyIsNotNone()
        {
            Assert.That(NamespacePrefix.Any.IsNone, Is.False);
        }

        [Test]
        public void AnyIsAny()
        {
            Assert.That(NamespacePrefix.Any.IsAny, Is.True);
        }

        [Test]
        public void AnyIsNotEmpty()
        {
            Assert.That(NamespacePrefix.Any.IsEmpty, Is.False);
        }

        [Test]
        public void AnyIsNotSpecific()
        {
            Assert.That(NamespacePrefix.Any.IsSpecific, Is.False);
        }

        [Test]
        public void EmptyText()
        {
            Assert.That(NamespacePrefix.Empty.Text, Is.EqualTo(string.Empty));
        }

        [Test]
        public void EmptyIsNotNone()
        {
            Assert.That(NamespacePrefix.Empty.IsNone, Is.False);
        }

        [Test]
        public void EmptyIsNotAny()
        {
            Assert.That(NamespacePrefix.Empty.IsAny, Is.False);
        }

        [Test]
        public void EmptyIsEmpty()
        {
            Assert.That(NamespacePrefix.Empty.IsEmpty, Is.True);
        }

        [Test]
        public void EmptyIsSpecific()
        {
            Assert.That(NamespacePrefix.Empty.IsSpecific, Is.True);
        }

        [Test]
        public void Equality()
        {
            Assert.That(NamespacePrefix.None.Equals(NamespacePrefix.None), Is.True);
            Assert.That(NamespacePrefix.Any.Equals(NamespacePrefix.Any), Is.True);
            Assert.That(NamespacePrefix.Empty.Equals(NamespacePrefix.Empty), Is.True);
            var foo = new NamespacePrefix("foo");
            Assert.That(foo.Equals(foo), Is.True);
            Assert.That(foo.Equals((object)foo), Is.True);
        }

        [Test]
        public void Inequality()
        {
            var foo = new NamespacePrefix("foo");
            var bar = new NamespacePrefix("bar");
            Assert.That(foo.Equals(bar), Is.False);
            Assert.That(foo.Equals((object)bar), Is.False);
        }

        [Test]
        public void TypeEquality()
        {
            var foo = new NamespacePrefix("foo");
            Assert.That(foo, Is.Not.EqualTo(null));
            Assert.That(foo, Is.Not.EqualTo(123));
        }

        [Test]
        public void NoneHashCode()
        {
            Assert.That(0, Is.EqualTo(NamespacePrefix.None.GetHashCode()));
        }

        [Test]
        public void HashCode()
        {
            var foo = new NamespacePrefix("foo");
            var bar = new NamespacePrefix("bar");
            Assert.That(foo.GetHashCode() == bar.GetHashCode(), Is.False);
        }

        [Test]
        public void NoneStringRepresentation()
        {
            Assert.That(NamespacePrefix.None.ToString(), Is.EqualTo("(none)"));
        }

        [Test]
        public void EmptyStringRepresentation()
        {
            Assert.That(NamespacePrefix.Empty.ToString(), Is.EqualTo(string.Empty));
        }

        [Test]
        public void AnyStringRepresentation()
        {
            Assert.That(NamespacePrefix.Any.ToString(), Is.EqualTo("*"));
        }

        [Test]
        public void StringRepresentation()
        {
            Assert.That(new NamespacePrefix("foo").ToString(), Is.EqualTo("foo"));
        }

        [Test]
        public void FormatNone()
        {
            Assert.That(NamespacePrefix.None.Format("name"), Is.EqualTo("name"));
        }

        [Test]
        public void FormatAny()
        {
            Assert.That(NamespacePrefix.Any.Format("name"), Is.EqualTo("*|name"));
        }

        [Test]
        public void FormatEmpty()
        {
            Assert.That(NamespacePrefix.Empty.Format("name"), Is.EqualTo("|name"));
        }

        [Test]
        public void Format()
        {
            Assert.That(new NamespacePrefix("foo").Format("bar"), Is.EqualTo("foo|bar"));
        }
    }
}
