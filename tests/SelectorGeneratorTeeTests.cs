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
    using System.Linq;
    using System.Reflection;
    using NUnit.Framework;

    #endregion

    /// <summary>
    /// Ensure that all actions on SelectorGeneratorTee are passed
    /// to the internal Primary and Secondary SelectorGenerators.
    /// </summary>
    [TestFixture]
    public class SelectorGeneratorTeeTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable. (Assigned during setup)
        SelectorGeneratorTee tee;
        FakeSelectorGenerator primary;
        FakeSelectorGenerator secondary;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [SetUp]
        public void Setup()
        {
            this.primary = new FakeSelectorGenerator();
            this.secondary = new FakeSelectorGenerator();
            this.tee = new SelectorGeneratorTee(this.primary, this.secondary);
        }

        [Test]
        public void NullPrimary()
        {
            Assert.That(() => new SelectorGeneratorTee(null!, new FakeSelectorGenerator()),
                        Throws.ArgumentNullException("primary"));
        }

        [Test]
        public void NullSecondary()
        {
            Assert.That(() => new SelectorGeneratorTee(new FakeSelectorGenerator(), null!),
                        Throws.ArgumentNullException("secondary"));
        }

        [Test]
        public void OnInitTest()
        {
            Run(this.tee.OnInit);
        }

        [Test]
        public void OnCloseTest()
        {
            Run(this.tee.OnClose);
        }

        [Test]
        public void OnSelectorTest()
        {
            Run(this.tee.OnSelector);
        }

        [Test]
        public void TypeTest()
        {
            Run(this.tee.Type, NamespacePrefix.None, "go");
        }

        [Test]
        public void UniversalTest()
        {
            Run(this.tee.Universal, NamespacePrefix.None);
        }

        [Test]
        public void IdTest()
        {
            Run(this.tee.Id, "hello");
        }

        [Test]
        public void ClassTest()
        {
            Run(this.tee.Class, "hello");
        }

        [Test]
        public void AttrExistsTest()
        {
            Run(this.tee.AttributeExists, NamespacePrefix.None, "hello");
        }

        [Test]
        public void AttExactTest()
        {
            Run(this.tee.AttributeExact, NamespacePrefix.None, "hello", "there");
        }

        [Test]
        public void AttrIncludesTest()
        {
            Run(this.tee.AttributeIncludes, NamespacePrefix.None, "hello", "there");
        }

        [Test]
        public void AttrDashMatchTest()
        {
            Run(this.tee.AttributeDashMatch, NamespacePrefix.None, "hello", "there");
        }

        [Test]
        public void AttrPrefixMatchTest()
        {
            Run(this.tee.AttributePrefixMatch,NamespacePrefix.None, "hello", "there");
        }

        [Test]
        public void AttrSuffixMatchTest()
        {
            Run(this.tee.AttributeSuffixMatch, NamespacePrefix.None, "hello", "there");
        }

        [Test]
        public void AttrSubstringTest()
        {
            Run(this.tee.AttributeSubstring, NamespacePrefix.None, "hello", "there");
        }

        [Test]
        public void FirstChildTest()
        {
            Run(this.tee.FirstChild);
        }

        [Test]
        public void LastChildTest()
        {
            Run(this.tee.LastChild);
        }

        [Test]
        public void NthChildTest()
        {
            Run(this.tee.NthChild, 1, 2);
        }

        [Test]
        public void OnlyChildTest()
        {
            Run(this.tee.OnlyChild);
        }

        [Test]
        public void EmptyTest()
        {
            Run(this.tee.Empty);
        }

        [Test]
        public void ChildTest()
        {
            Run(this.tee.Child);
        }

        [Test]
        public void DescendantTest()
        {
            Run(this.tee.Descendant);
        }

        [Test]
        public void AdjacentTest()
        {
            Run(this.tee.Adjacent);
        }

        [Test]
        public void GeneralSiblingTest()
        {
            Run(this.tee.GeneralSibling);
        }

        void Run(Action action)
        {
            RunImpl(action.Method);
        }

        void Run<T>(Action<T> action, T arg)
        {
            RunImpl(action.Method, arg);
        }

        void Run<T1, T2>(Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            RunImpl(action.Method, arg1, arg2);
        }

        void Run<T1, T2, T3>(Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            RunImpl(action.Method, arg1, arg2, arg3);
        }

        /// <summary>
        /// Take the passed action, run it, and then check that the last method
        /// and last args are the same for pri and sec.
        /// </summary>
        void RunImpl(MethodBase action, params object?[] args)
        {
            var recordings = new Queue<CallRecording<ISelectorGenerator>>(2);
            this.primary.Recorder = recordings.Enqueue;
            this.secondary.Recorder = recordings.Enqueue;

            _ = action.Invoke(this.tee, args);

            // Assert the fact that the primary and secondary methods were
            // both called with the same arguments and in the right order!

            var recording = recordings.Dequeue();
            Assert.That(recording.Target, Is.SameAs(this.primary));
            Assert.That(MapMethod<ISelectorGenerator>(recording.Method).Name, Is.EqualTo(action.Name));
            Assert.That(recording.Arguments, Is.EqualTo(args));

            recording = recordings.Dequeue();
            Assert.That(recording.Target, Is.SameAs(this.secondary));
            Assert.That(MapMethod<ISelectorGenerator>(recording.Method).Name, Is.EqualTo(action.Name));
            Assert.That(recording.Arguments, Is.EqualTo(args));
        }

        static MethodInfo MapMethod<T>(MethodInfo method) where T : class
        {
#pragma warning disable CA2201 // Do not raise reserved exception types (test code)
            var type = method.ReflectedType ?? throw new NullReferenceException();
#pragma warning restore CA2201 // Do not raise reserved exception types
            var mapping = type.GetInterfaceMap(typeof(T));
            return mapping.InterfaceMethods
                          .Select((m, i) => new { Source = m, Target = mapping.TargetMethods[i] })
                          .Single(m => m.Target == method).Source;
        }

        sealed class CallRecording<T>(T target, MethodInfo method, object[] arguments)
        {
            public T Target { get; } = target;
            public MethodInfo Method { get; } = method;
            public object[] Arguments { get; } = arguments;
        }

        sealed class FakeSelectorGenerator : ISelectorGenerator
        {
            public Action<CallRecording<ISelectorGenerator>>? Recorder { get; set; }

            public void OnInit() =>
                OnInvoked(MethodBase.GetCurrentMethod());

            public void OnClose() =>
                OnInvoked(MethodBase.GetCurrentMethod());

            public void OnSelector() =>
                OnInvoked(MethodBase.GetCurrentMethod());

            public void Type(NamespacePrefix prefix, string type) =>
                OnInvoked(MethodBase.GetCurrentMethod(), prefix, type);

            public void Universal(NamespacePrefix prefix) =>
                OnInvoked(MethodBase.GetCurrentMethod(), prefix);

            public void Id(string id) =>
                OnInvoked(MethodBase.GetCurrentMethod(), id);

            public void Class(string clazz) =>
                OnInvoked(MethodBase.GetCurrentMethod(), clazz);

            public void AttributeExists(NamespacePrefix prefix, string name) =>
                OnInvoked(MethodBase.GetCurrentMethod(), prefix, name);

            public void AttributeExact(NamespacePrefix prefix, string name, string value) =>
                OnInvoked(MethodBase.GetCurrentMethod(), prefix, name, value);

            public void AttributeIncludes(NamespacePrefix prefix, string name, string value) =>
                OnInvoked(MethodBase.GetCurrentMethod(), prefix, name, value);

            public void AttributeDashMatch(NamespacePrefix prefix, string name, string value) =>
                OnInvoked(MethodBase.GetCurrentMethod(), prefix, name, value);

            public void AttributePrefixMatch(NamespacePrefix prefix, string name, string value) =>
                OnInvoked(MethodBase.GetCurrentMethod(), prefix, name, value);

            public void AttributeSuffixMatch(NamespacePrefix prefix, string name, string value) =>
                OnInvoked(MethodBase.GetCurrentMethod(), prefix, name, value);

            public void AttributeSubstring(NamespacePrefix prefix, string name, string value) =>
                OnInvoked(MethodBase.GetCurrentMethod(), prefix, name, value);

            public void FirstChild() =>
                OnInvoked(MethodBase.GetCurrentMethod());

            public void LastChild() =>
                OnInvoked(MethodBase.GetCurrentMethod());

            public void NthChild(int a, int b) =>
                OnInvoked(MethodBase.GetCurrentMethod(), a, b);

            public void OnlyChild() =>
                OnInvoked(MethodBase.GetCurrentMethod());

            public void Empty() =>
                OnInvoked(MethodBase.GetCurrentMethod());

            public void Child() =>
                OnInvoked(MethodBase.GetCurrentMethod());

            public void Descendant() =>
                OnInvoked(MethodBase.GetCurrentMethod());

            public void Adjacent() =>
                OnInvoked(MethodBase.GetCurrentMethod());

            public void GeneralSibling() =>
                OnInvoked(MethodBase.GetCurrentMethod());

            public void NthLastChild(int a, int b) =>
                OnInvoked(MethodBase.GetCurrentMethod(), a, b);

            void OnInvoked(MethodBase? method, params object[] args)
            {
                switch (method)
                {
                    case null: throw new ArgumentNullException(nameof(method));
                    case MethodInfo info: Recorder?.Invoke(new CallRecording<ISelectorGenerator>(this, info, args)); break;
                    default: throw new ArgumentException(null, nameof(method));
                }
            }
        }
    }
}
