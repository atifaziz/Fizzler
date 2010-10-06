namespace Fizzler.Tests
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;

    #endregion

    [TestFixture]
    public class ParserTests
    {
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

        private static void Test<T1>(string input, Func<TestSelectorGenerator, T1> actual1, T1 expected1)
        {
            Test(input,
                new Func<TestSelectorGenerator, object>[] { g => actual1(g) },
                new object[] { expected1 });
        }

        private static void Test<T1, T2>(string input, 
            Func<TestSelectorGenerator, T1> actual1, T1 expected1, 
            Func<TestSelectorGenerator, T2> actual2, T2 expected2)
        {
            Test(input, 
                new Func<TestSelectorGenerator, object>[] {g => actual1(g), g => actual2(g)}, 
                new object[] {expected1, expected2});
        }

        private static void Test<T1, T2, T3>(string input,
            Func<TestSelectorGenerator, T1> actual1, T1 expected1,
            Func<TestSelectorGenerator, T2> actual2, T2 expected2,
            Func<TestSelectorGenerator, T3> actual3, T2 expected3)
        {
            Test(input,
                new Func<TestSelectorGenerator, object>[] { g => actual1(g), g => actual2(g), g => actual3(g) },
                new object[] { expected1, expected2, expected3 });
        }

        private static void Test(string input, 
            IEnumerable<Func<TestSelectorGenerator, object>> actuals, 
            IEnumerable<object> expectations)
        {
            var generator = new TestSelectorGenerator();
            Parser.Parse(Tokener.Tokenize(input), generator);
            using (var actual = actuals.GetEnumerator())
            using (var expected = expectations.GetEnumerator())
            {
                while(actual.MoveNext())
                {
                    Assert.That(expected.MoveNext(), Is.True, "Missing expectation");
                    Assert.That(actual.Current(generator), Is.EqualTo(expected.Current));
                }
                Assert.That(expected.MoveNext(), Is.False, "Too many expectations");
            }
        }

        public class TestSelectorGenerator : ISelectorGenerator
        {
            public NamespacePrefix TypePrefix;
            public string TypeName;

            public NamespacePrefix UniversalPrefix;

            public NamespacePrefix AttributeExistsPrefix;
            public string AttributeExistsName;
            public NamespacePrefix AttributeExactPrefix;
            public string AttributeExactName;
            public string AttributeExactValue;
            public NamespacePrefix AttributeIncludesPrefix;
            public string AttributeIncludesName;
            public string AttributeIncludesValue;
            public NamespacePrefix AttributeDashMatchPrefix;
            public string AttributeDashMatchName;
            public string AttributeDashMatchValue;
            public NamespacePrefix AttributePrefixMatchPrefix;
            public string AttributePrefixMatchName;
            public string AttributePrefixMatchValue;
            public NamespacePrefix AttributeSuffixMatchPrefix;
            public string AttributeSuffixMatchName;
            public string AttributeSuffixMatchValue;
            public NamespacePrefix AttributeSubstringPrefix;
            public string AttributeSubstringName;
            public string AttributeSubstringValue;

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

             public void NthLastChild(int a, int b)
             {
                 throw new NotImplementedException();
             }

            #endregion
        }
    }
}