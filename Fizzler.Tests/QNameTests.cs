using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Fizzler.Tests
{
    [TestFixture]
    public class QNameTests
    {
        [Test]
        public void Empty()
        {
            Assert.That(QName.Empty.Namespace, Is.Null);
            Assert.That(QName.Empty.Name, Is.Null);
        }

        [Test,ExpectedException(typeof(ArgumentNullException))]
        public void InitializationWithNullName()
        {
            new QName(null, null);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void InitializationWithEmptyName()
        {
            new QName(null, string.Empty);
        }
        
        [Test]
        public void Namespaceless()
        {
            var name = QName.Namespaceless("foo");
            Assert.That(name.Namespace, Is.Null);
            Assert.That(name.Name, Is.EqualTo("foo"));
            Assert.That(name.IsEmpty, Is.False, "IsEmpty");
            Assert.That(name.IsNamespaceless, Is.True, "IsNamespaceless");
            Assert.That(name.IsAnyNamespace, Is.False, "IsAnyNamespace");
            Assert.That(name.IsEmptyNamespace, Is.False, "IsEmptyNamespace");
        }

        [Test]
        public void AnyNamespace()
        {
            var name = QName.AnyNamespace("foo");
            Assert.That(name.Namespace, Is.EqualTo("*"));
            Assert.That(name.Name, Is.EqualTo("foo"));
            Assert.That(name.IsEmpty, Is.False, "IsEmpty");
            Assert.That(name.IsNamespaceless, Is.False, "IsNamespaceless");
            Assert.That(name.IsAnyNamespace, Is.True, "IsAnyNamespace");
            Assert.That(name.IsEmptyNamespace, Is.False, "IsEmptyNamespace");
        }

        [Test]
        public void EmpyNamespace()
        {
            var name = QName.EmptyNamespace("foo");
            Assert.That(name.Namespace, Is.EqualTo(string.Empty));
            Assert.That(name.Name, Is.EqualTo("foo"));
            Assert.That(name.IsEmpty, Is.False, "IsEmpty");
            Assert.That(name.IsNamespaceless, Is.False, "IsNamespaceless");
            Assert.That(name.IsAnyNamespace, Is.False, "IsAnyNamespace");
            Assert.That(name.IsEmptyNamespace, Is.True, "IsEmptyNamespace");
        }
        
        [Test]
        public void IsEmpty()
        {
            Assert.That(QName.Empty.IsEmpty, Is.True);
        }
        
        [Test]
        public void NoneEmpty()
        {
            Assert.That(QName.AnyNamespace("foo").IsEmpty, Is.Not.True);
        }

        [Test,ExpectedException(typeof(InvalidOperationException))]
        public void EmptyIsNamespaceless()
        {
            var unused = QName.Empty.IsNamespaceless;
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void EmptyIsAnyNamespace()
        {
            var unused = QName.Empty.IsAnyNamespace;
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void EmptyIsEmptyNamespace()
        {
            var unused = QName.Empty.IsEmptyNamespace;
        }

        [Test]
        public void Equality()
        {
            Assert.That(QName.Empty.Equals(QName.Empty), Is.True);
            Assert.That(QName.Empty, Is.EqualTo(QName.Empty));
            Assert.That(new QName("ns", "name").Equals(new QName("ns", "name")), Is.True);
            Assert.That(new QName("ns", "name"), Is.EqualTo(new QName("ns", "name")));
        }

        [Test]
        public void TypeEquality()
        {
            Assert.That(new QName("ns", "name"), Is.Not.EqualTo(null));
            Assert.That(new QName("ns", "name"), Is.Not.EqualTo(123));
        }

        [Test]
        public void EmptyHashCode()
        {
            Assert.That(0, Is.EqualTo(QName.Empty.GetHashCode()));
        }

        [Test]
        public void HashCode()
        {
            var name1 = new QName("ns1", "name1");
            var name2 = new QName("ns1", "name2");
            Assert.That(name1.GetHashCode() == name2.GetHashCode(), Is.False);
        }

        [Test]
        public void NamespacelessHashCode()
        {
            var name1 = QName.Namespaceless("name1");
            var name2 = QName.Namespaceless("name2");
            Assert.That(name1.GetHashCode() == name2.GetHashCode(), Is.False);
        }

        [Test]
        public void EmptyStringRepresentation()
        {
            Assert.That(QName.Empty.ToString(), Is.EqualTo(string.Empty));
        }

        [Test]
        public void StringRepresentation()
        {
            Assert.That(new QName("ns", "name").ToString(), Is.EqualTo("ns|name"));
        }

        [Test]
        public void NamespacelessNameStringRepresentation()
        {
            Assert.That(QName.Namespaceless("name").ToString(), Is.EqualTo("name"));
        }

        [Test]
        public void AnyNamespaceNameStringRepresentation()
        {
            Assert.That(QName.AnyNamespace("name").ToString(), Is.EqualTo("*|name"));
        }

        [Test]
        public void EmptyNamespaceNameStringRepresentation()
        {
            Assert.That(QName.EmptyNamespace("name").ToString(), Is.EqualTo("|name"));
        }

        [Test]
        public void EmptyMatches()
        {
            
        }
    }
}
