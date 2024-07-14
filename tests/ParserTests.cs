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
    #region Imports

    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class ParserTests
    {
        [TestCase(":not(.foo.bar)")]
        [TestCase(":not(p.foo)")]
        [TestCase(":not(p div)")]
        [TestCase(":not(p, div)")]
        [TestCase(":not(#foo.bar)")]
        [TestCase(":not([foo][bar])")]
        public void Invalid(string selector)
        {
            Assert.That(() => Parser.Parse(Tokener.Tokenize(selector), new NoSelectorGenerator()),
                        Throws.TypeOf<FormatException>());
        }

        [Test]
        public void TypeNoNamespace()
        {
            Test("x", g => g.TypePrefix, NamespacePrefix.None,
                      g => g.TypeName, "x");
        }

        [Test]
        public void TypeEmptyNamespace()
        {
            Test("|x", g => g.TypePrefix, NamespacePrefix.Empty,
                       g => g.TypeName, "x");
        }

        [Test]
        public void TypeAnyNamespace()
        {
            Test("*|x", g => g.TypePrefix, NamespacePrefix.Any,
                        g => g.TypeName, "x");
        }

        [Test]
        public void NamespacedType()
        {
            Test("foo|bar", g => g.TypePrefix, new NamespacePrefix("foo"),
                            g => g.TypeName, "bar");
        }

        [Test]
        public void UniversalNoNamespace()
        {
            Test("*", g => g.UniversalPrefix, NamespacePrefix.None);
        }

        [Test]
        public void UniversalEmptyNamespace()
        {
            Test("|*", g => g.UniversalPrefix, NamespacePrefix.Empty);
        }

        [Test]
        public void UniversalAnyNamespace()
        {
            Test("*|*", g => g.UniversalPrefix, NamespacePrefix.Any);
        }

        [Test]
        public void NamespacedUniversal()
        {
            Test("foo|*", g => g.UniversalPrefix, new NamespacePrefix("foo"));
        }

        [Test]
        public void NoNamespaceAttribueExists()
        {
            Test("[foo]", g => g.AttributeExistsPrefix, NamespacePrefix.None,
                          g => g.AttributeExistsName, "foo");
        }

        [Test]
        public void EmptyNamespaceAttribueExists()
        {
            Test("[|a]", g => g.AttributeExistsPrefix, NamespacePrefix.Empty,
                         g => g.AttributeExistsName, "a");
        }

        [Test]
        public void AnyNamespaceAttribueExists()
        {
            Test("[*|a]", g => g.AttributeExistsPrefix, NamespacePrefix.Any,
                          g => g.AttributeExistsName, "a");
        }

        [Test]
        public void NamespacedAttribueExists()
        {
            Test("[ns|a]", g => g.AttributeExistsPrefix, new NamespacePrefix("ns"),
                           g => g.AttributeExistsName, "a");
        }

        [Test]
        public void NoNamespaceAttribueExact()
        {
            Test("[a=v]", g => g.AttributeExactPrefix, NamespacePrefix.None,
                          g => g.AttributeExactName, "a",
                          g => g.AttributeExactValue, "v");
        }

        [Test]
        public void EmptyNamespaceAttribueExact()
        {
            Test("[|a=v]", g => g.AttributeExactPrefix, NamespacePrefix.Empty,
                           g => g.AttributeExactName, "a",
                           g => g.AttributeExactValue, "v");
        }

        [Test]
        public void AnyNamespaceAttribueExact()
        {
            Test("[*|a=v]", g => g.AttributeExactPrefix, NamespacePrefix.Any,
                            g => g.AttributeExactName, "a",
                            g => g.AttributeExactValue, "v");
        }

        [Test]
        public void NamespacedAttribueExact()
        {
            Test("[ns|a=v]", g => g.AttributeExactPrefix, new NamespacePrefix("ns"),
                             g => g.AttributeExactName, "a",
                             g => g.AttributeExactValue, "v");
        }

        [Test]
        public void NoNamespaceAttribueIncludes()
        {
            Test("[a~=v]", g => g.AttributeIncludesPrefix, NamespacePrefix.None,
                           g => g.AttributeIncludesName, "a",
                           g => g.AttributeIncludesValue, "v");
        }

        [Test]
        public void EmptyNamespaceAttribueIncludes()
        {
            Test("[|a~=v]", g => g.AttributeIncludesPrefix, NamespacePrefix.Empty,
                            g => g.AttributeIncludesName, "a",
                            g => g.AttributeIncludesValue, "v");
        }

        [Test]
        public void AnyNamespaceAttribueIncludes()
        {
            Test("[*|a~=v]", g => g.AttributeIncludesPrefix, NamespacePrefix.Any,
                             g => g.AttributeIncludesName, "a",
                             g => g.AttributeIncludesValue, "v");
        }

        [Test]
        public void NamespacedAttribueIncludes()
        {
            Test("[ns|a~=v]", g => g.AttributeIncludesPrefix, new NamespacePrefix("ns"),
                              g => g.AttributeIncludesName, "a",
                              g => g.AttributeIncludesValue, "v");
        }

        [Test]
        public void NoNamespaceAttribueDashMatch()
        {
            Test("[a|=v]", g => g.AttributeDashMatchPrefix, NamespacePrefix.None,
                           g => g.AttributeDashMatchName, "a",
                           g => g.AttributeDashMatchValue, "v");
        }

        [Test]
        public void EmptyNamespaceAttribueDashMatch()
        {
            Test("[|a|=v]", g => g.AttributeDashMatchPrefix, NamespacePrefix.Empty,
                            g => g.AttributeDashMatchName, "a",
                            g => g.AttributeDashMatchValue, "v");
        }

        [Test]
        public void AnyNamespaceAttribueDashMatch()
        {
            Test("[*|a|=v]", g => g.AttributeDashMatchPrefix, NamespacePrefix.Any,
                             g => g.AttributeDashMatchName, "a",
                             g => g.AttributeDashMatchValue, "v");
        }

        [Test]
        public void NamespacedAttribueDashMatch()
        {
            Test("[ns|a|=v]", g => g.AttributeDashMatchPrefix, new NamespacePrefix("ns"),
                              g => g.AttributeDashMatchName, "a",
                              g => g.AttributeDashMatchValue, "v");
        }

        [Test]
        public void NoNamespaceAttribuePrefixMatch()
        {
            Test("[a^=v]", g => g.AttributePrefixMatchPrefix, NamespacePrefix.None,
                           g => g.AttributePrefixMatchName, "a",
                           g => g.AttributePrefixMatchValue, "v");
        }

        [Test]
        public void EmptyNamespaceAttribuePrefixMatch()
        {
            Test("[|a^=v]", g => g.AttributePrefixMatchPrefix, NamespacePrefix.Empty,
                            g => g.AttributePrefixMatchName, "a",
                            g => g.AttributePrefixMatchValue, "v");
        }

        [Test]
        public void AnyNamespaceAttribuePrefixMatch()
        {
            Test("[*|a^=v]", g => g.AttributePrefixMatchPrefix, NamespacePrefix.Any,
                             g => g.AttributePrefixMatchName, "a",
                             g => g.AttributePrefixMatchValue, "v");
        }

        [Test]
        public void NamespacedAttribuePrefixMatch()
        {
            Test("[ns|a^=v]", g => g.AttributePrefixMatchPrefix, new NamespacePrefix("ns"),
                              g => g.AttributePrefixMatchName, "a",
                              g => g.AttributePrefixMatchValue, "v");
        }

        [Test]
        public void NoNamespaceAttribueSuffixMatch()
        {
            Test("[a$=v]", g => g.AttributeSuffixMatchPrefix, NamespacePrefix.None,
                           g => g.AttributeSuffixMatchName, "a",
                           g => g.AttributeSuffixMatchValue, "v");
        }

        [Test]
        public void EmptyNamespaceAttribueSuffixMatch()
        {
            Test("[|a$=v]", g => g.AttributeSuffixMatchPrefix, NamespacePrefix.Empty,
                            g => g.AttributeSuffixMatchName, "a",
                            g => g.AttributeSuffixMatchValue, "v");
        }

        [Test]
        public void AnyNamespaceAttribueSuffixMatch()
        {
            Test("[*|a$=v]", g => g.AttributeSuffixMatchPrefix, NamespacePrefix.Any,
                             g => g.AttributeSuffixMatchName, "a",
                             g => g.AttributeSuffixMatchValue, "v");
        }

        [Test]
        public void NamespacedAttribueSuffixMatch()
        {
            Test("[ns|a$=v]", g => g.AttributeSuffixMatchPrefix, new NamespacePrefix("ns"),
                              g => g.AttributeSuffixMatchName, "a",
                              g => g.AttributeSuffixMatchValue, "v");
        }

        [Test]
        public void NoNamespaceAttribueSubstring()
        {
            Test("[a*=v]", g => g.AttributeSubstringPrefix, NamespacePrefix.None,
                           g => g.AttributeSubstringName, "a",
                           g => g.AttributeSubstringValue, "v");
        }

        [Test]
        public void EmptyNamespaceAttribueSubstring()
        {
            Test("[|a*=v]", g => g.AttributeSubstringPrefix, NamespacePrefix.Empty,
                            g => g.AttributeSubstringName, "a",
                            g => g.AttributeSubstringValue, "v");
        }

        [Test]
        public void AnyNamespaceAttribueSubstring()
        {
            Test("[*|a*=v]", g => g.AttributeSubstringPrefix, NamespacePrefix.Any,
                             g => g.AttributeSubstringName, "a",
                             g => g.AttributeSubstringValue, "v");
        }

        [Test]
        public void NamespacedAttribueSubstring()
        {
            Test("[ns|a*=v]", g => g.AttributeSubstringPrefix, new NamespacePrefix("ns"),
                              g => g.AttributeSubstringName, "a",
                              g => g.AttributeSubstringValue, "v");
        }

        static void Test<T1>(string input, Func<TestSelectorGenerator, T1> actual1, T1 expected1)
        {
            Test(input, [g => actual1(g)], [expected1]);
        }

        static void Test<T1, T2>(string input,
            Func<TestSelectorGenerator, T1> actual1, T1 expected1,
            Func<TestSelectorGenerator, T2> actual2, T2 expected2)
        {
            Test(input,
                 [g => actual1(g), g => actual2(g)],
                 [expected1, expected2]);
        }

        static void Test<T1, T2, T3>(string input,
            Func<TestSelectorGenerator, T1> actual1, T1 expected1,
            Func<TestSelectorGenerator, T2> actual2, T2 expected2,
            Func<TestSelectorGenerator, T3> actual3, T2 expected3)
        {
            Test(input,
                 [g => actual1(g), g => actual2(g), g => actual3(g)],
                 [expected1, expected2, expected3]);
        }

        static void Test(string input,
            IEnumerable<Func<TestSelectorGenerator, object?>> actuals,
            IEnumerable<object?> expectations)
        {
            var generator = Parser.Parse(Tokener.Tokenize(input), new TestSelectorGenerator());
            using var actual = actuals.GetEnumerator();
            using var expected = expectations.GetEnumerator();
            while(actual.MoveNext())
            {
                Assert.That(expected.MoveNext(), Is.True, "Missing expectation");
                Assert.That(actual.Current(generator), Is.EqualTo(expected.Current));
            }
            Assert.That(expected.MoveNext(), Is.False, "Too many expectations");
        }

        public class TestSelectorGenerator : ISelectorGenerator
        {
            public NamespacePrefix TypePrefix { get; private set; }
            public string? TypeName { get; private set; }

            public NamespacePrefix UniversalPrefix { get; private set; }

            public NamespacePrefix AttributeExistsPrefix { get; private set; }
            public string? AttributeExistsName { get; private set; }
            public NamespacePrefix AttributeExactPrefix { get; private set; }
            public string? AttributeExactName { get; private set; }
            public string? AttributeExactValue { get; private set; }
            public NamespacePrefix AttributeIncludesPrefix { get; private set; }
            public string? AttributeIncludesName { get; private set; }
            public string? AttributeIncludesValue { get; private set; }
            public NamespacePrefix AttributeDashMatchPrefix { get; private set; }
            public string? AttributeDashMatchName { get; private set; }
            public string? AttributeDashMatchValue { get; private set; }
            public NamespacePrefix AttributePrefixMatchPrefix { get; private set; }
            public string? AttributePrefixMatchName { get; private set; }
            public string? AttributePrefixMatchValue { get; private set; }
            public NamespacePrefix AttributeSuffixMatchPrefix { get; private set; }
            public string? AttributeSuffixMatchName { get; private set; }
            public string? AttributeSuffixMatchValue { get; private set; }
            public NamespacePrefix AttributeSubstringPrefix { get; private set; }
            public string? AttributeSubstringName { get; private set; }
            public string? AttributeSubstringValue { get; private set; }

            public void Type(NamespacePrefix prefix, string name)
            {
                TypePrefix = prefix;
                TypeName = name;
            }

            public void Universal(NamespacePrefix prefix)
            {
                UniversalPrefix = prefix;
            }

            public void AttributeExists(NamespacePrefix prefix, string name)
            {
                AttributeExistsPrefix = prefix;
                AttributeExistsName = name;
            }

            public void AttributeExact(NamespacePrefix prefix, string name, string value)
            {
                AttributeExactPrefix = prefix;
                AttributeExactName = name;
                AttributeExactValue = value;
            }

            public void AttributeIncludes(NamespacePrefix prefix, string name, string value)
            {
                AttributeIncludesPrefix = prefix;
                AttributeIncludesName = name;
                AttributeIncludesValue = value;
            }

            public void AttributeDashMatch(NamespacePrefix prefix, string name, string value)
            {
                AttributeDashMatchPrefix = prefix;
                AttributeDashMatchName = name;
                AttributeDashMatchValue = value;
            }

            public void AttributePrefixMatch(NamespacePrefix prefix, string name, string value)
            {
                AttributePrefixMatchPrefix = prefix;
                AttributePrefixMatchName = name;
                AttributePrefixMatchValue = value;
            }

            public void AttributeSuffixMatch(NamespacePrefix prefix, string name, string value)
            {
                AttributeSuffixMatchPrefix = prefix;
                AttributeSuffixMatchName = name;
                AttributeSuffixMatchValue = value;
            }

            public void AttributeSubstring(NamespacePrefix prefix, string name, string value)
            {
                AttributeSubstringPrefix = prefix;
                AttributeSubstringName = name;
                AttributeSubstringValue = value;
            }

            #region Unimplemented memebers

            public void OnInit() {}
            public void OnClose() {}
            public void OnSelector() {}
            public void Id(string id) => throw new NotImplementedException();
            public void Class(string clazz) => throw new NotImplementedException();
            public void FirstChild() => throw new NotImplementedException();
            public void LastChild() => throw new NotImplementedException();
            public void NthChild(int a, int b) => throw new NotImplementedException();
            public void OnlyChild() => throw new NotImplementedException();
            public void Empty() => throw new NotImplementedException();
            public void Child() => throw new NotImplementedException();
            public void Descendant() => throw new NotImplementedException();
            public void Adjacent() => throw new NotImplementedException();
            public void GeneralSibling() => throw new NotImplementedException();
            public void NthLastChild(int a, int b) => throw new NotImplementedException();

            #endregion
        }

        sealed class NoSelectorGenerator : INegationSelectorGenerator
        {
            public void OnInit() {}
            public void OnClose() {}
            public void OnSelector() {}
            public void Type(NamespacePrefix prefix, string name) {}
            public void Universal(NamespacePrefix prefix) {}
            public void Id(string id) {}
            public void Class(string clazz) {}
            public void AttributeExists(NamespacePrefix prefix, string name) {}
            public void AttributeExact(NamespacePrefix prefix, string name, string value) {}
            public void AttributeIncludes(NamespacePrefix prefix, string name, string value) {}
            public void AttributeDashMatch(NamespacePrefix prefix, string name, string value) {}
            public void AttributePrefixMatch(NamespacePrefix prefix, string name, string value) {}
            public void AttributeSuffixMatch(NamespacePrefix prefix, string name, string value) {}
            public void AttributeSubstring(NamespacePrefix prefix, string name, string value) {}
            public void FirstChild() {}
            public void LastChild() {}
            public void NthChild(int a, int b) {}
            public void OnlyChild() {}
            public void Empty() {}
            public void Child() {}
            public void Descendant() {}
            public void Adjacent() {}
            public void GeneralSibling() {}
            public void NthLastChild(int a, int b) {}
            public void BeginNegation() {}
            public void EndNegation() {}
        }
    }
}
