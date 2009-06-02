using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Fizzler.Tests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void TypeNoNamespace()
        {
            var generator = new TestSelectorGenerator();
            Parser.Parse(Tokener.Tokenize("x"), generator);
            Assert.That(generator.TypePrefix, Is.EqualTo(NamespacePrefix.None));
            Assert.That(generator.TypeName, Is.EqualTo("x"));
        }

        [Test]
        public void TypeEmptyNamespace()
        {
            var generator = new TestSelectorGenerator();
            Parser.Parse(Tokener.Tokenize("|x"), generator);
            Assert.That(generator.TypePrefix, Is.EqualTo(NamespacePrefix.Empty));
            Assert.That(generator.TypeName, Is.EqualTo("x"));
        }

        [Test]
        public void TypeAnyNamespace()
        {
            var generator = new TestSelectorGenerator();
            Parser.Parse(Tokener.Tokenize("*|x"), generator);
            Assert.That(generator.TypePrefix, Is.EqualTo(NamespacePrefix.Any));
            Assert.That(generator.TypeName, Is.EqualTo("x"));
        }

        [Test]
        public void NamespacedType()
        {
            var generator = new TestSelectorGenerator();
            Parser.Parse(Tokener.Tokenize("foo|bar"), generator);
            Assert.That(generator.TypePrefix, Is.EqualTo(new NamespacePrefix("foo")));
            Assert.That(generator.TypeName, Is.EqualTo("bar"));
        }

        [Test]
        public void UniversalNoNamespace()
        {
            var generator = new TestSelectorGenerator();
            Parser.Parse(Tokener.Tokenize("*"), generator);
            Assert.That(generator.UniversalPrefix, Is.EqualTo(NamespacePrefix.None));
        }

        [Test]
        public void UniversalEmptyNamespace()
        {
            var generator = new TestSelectorGenerator();
            Parser.Parse(Tokener.Tokenize("|*"), generator);
            Assert.That(generator.UniversalPrefix, Is.EqualTo(NamespacePrefix.Empty));
        }

        [Test]
        public void UniversalAnyNamespace()
        {
            var generator = new TestSelectorGenerator();
            Parser.Parse(Tokener.Tokenize("*|*"), generator);
            Assert.That(generator.UniversalPrefix, Is.EqualTo(NamespacePrefix.Any));
        }

        [Test]
        public void NamespacedUniversal()
        {
            var generator = new TestSelectorGenerator();
            Parser.Parse(Tokener.Tokenize("foo|*"), generator);
            Assert.That(generator.UniversalPrefix, Is.EqualTo(new NamespacePrefix("foo")));
        }

        public class TestSelectorGenerator : ISelectorGenerator
        {
            public NamespacePrefix TypePrefix;
            public string TypeName;

            public NamespacePrefix UniversalPrefix;

            public void Type(NamespacePrefix prefix, string name)
            {
                TypePrefix = prefix;
                TypeName = name;
            }

            public void Universal(NamespacePrefix prefix)
            {
                UniversalPrefix = prefix;
            }

            #region Unimplemented memebers

            public void OnInit()
            {
            }

            public void OnClose()
            {
            }

            public void OnSelector()
            {
            }

            public void Id(string id)
            {
                throw new NotImplementedException();
            }

            public void Class(string clazz)
            {
                throw new NotImplementedException();
            }

            public void AttributeExists(NamespacePrefix prefix, string name)
            {
                throw new NotImplementedException();
            }

            public void AttributeExact(NamespacePrefix prefix, string name, string value)
            {
                throw new NotImplementedException();
            }

            public void AttributeIncludes(NamespacePrefix prefix, string name, string value)
            {
                throw new NotImplementedException();
            }

            public void AttributeDashMatch(NamespacePrefix prefix, string name, string value)
            {
                throw new NotImplementedException();
            }

            public void AttributePrefixMatch(NamespacePrefix prefix, string name, string value)
            {
                throw new NotImplementedException();
            }

            public void AttributeSuffixMatch(NamespacePrefix prefix, string name, string value)
            {
                throw new NotImplementedException();
            }

            public void AttributeSubstring(NamespacePrefix prefix, string name, string value)
            {
                throw new NotImplementedException();
            }

            public void FirstChild()
            {
                throw new NotImplementedException();
            }

            public void LastChild()
            {
                throw new NotImplementedException();
            }

            public void NthChild(int a, int b)
            {
                throw new NotImplementedException();
            }

            public void OnlyChild()
            {
                throw new NotImplementedException();
            }

            public void Empty()
            {
                throw new NotImplementedException();
            }

            public void Child()
            {
                throw new NotImplementedException();
            }

            public void Descendant()
            {
                throw new NotImplementedException();
            }

            public void Adjacent()
            {
                throw new NotImplementedException();
            }

            public void GeneralSibling()
            {
                throw new NotImplementedException();
            }

            #endregion
        }
    }
}