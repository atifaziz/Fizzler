using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Fizzler.Tests
{
	/// <summary>
	/// Ensure that all actions on SelectorGeneratorTee are passed
	/// to the internal Primary and Secondary SelectorGenerators.
	/// </summary>
	[TestFixture]
	public class SelectorGeneratorTeeTests
	{
		private static SelectorGeneratorTee tee;
		private static FakeSelectorGenerator primary;
		private static FakeSelectorGenerator secondary;

		[SetUp]
		public void Setup()
		{
			primary = new FakeSelectorGenerator();
			secondary = new FakeSelectorGenerator();
			tee = new SelectorGeneratorTee(primary, secondary);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void NullPrimary()
		{
			new SelectorGeneratorTee(
				null, new FakeSelectorGenerator()
			);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void NullSecondary()
		{
			new SelectorGeneratorTee(
				new FakeSelectorGenerator(), null
			);
		}

        [Test]
		public void OnInitTest()
		{
			Run(tee.OnInit);
		}

		[Test]
		public void OnCloseTest()
		{
			Run(tee.OnClose);
		}

		[Test]
		public void OnSelectorTest()
		{
			Run(tee.OnSelector);
		}

		[Test]
		public void TypeTest()
		{
            Run(tee.Type, QName.Namespaceless("go"));
		}

		[Test]
		public void UniversalTest()
		{
			Run(tee.Universal);
		}

		[Test]
		public void IdTest()
		{
			Run(tee.Id, "hello");
		}

		[Test]
		public void ClassTest()
		{
			Run(tee.Class, "hello");
		}

		[Test]
		public void AttrExistsTest()
		{
			Run(tee.AttributeExists, QName.Namespaceless("hello"));
		}

		[Test]
		public void AttExactTest()
		{
			Run(tee.AttributeExact, QName.Namespaceless("hello"), "there");
		}

		[Test]
		public void AttrIncludesTest()
		{
			Run(tee.AttributeIncludes, QName.Namespaceless("hello"), "there");
		}

		[Test]
		public void AttrDashMatchTest()
		{
			Run(tee.AttributeDashMatch, QName.Namespaceless("hello"), "there");
		}

		[Test]
		public void AttrPrefixMatchTest()
		{
			Run(tee.AttributePrefixMatch,QName.Namespaceless("hello"), "there");
		}

		[Test]
		public void AttrSuffixMatchTest()
		{
			Run(tee.AttributeSuffixMatch, QName.Namespaceless("hello"), "there");
		}

		[Test]
		public void AttrSubstringTest()
		{
			Run(tee.AttributeSubstring, QName.Namespaceless("hello"), "there");
		}

		[Test]
		public void FirstChildTest()
		{
			Run(tee.FirstChild);
		}

		[Test]
		public void LastChildTest()
		{
			Run(tee.LastChild);
		}

		[Test]
		public void NthChildTest()
		{
			RunI(tee.NthChild, 1, 2);
		}

		[Test]
		public void OnlyChildTest()
		{
			Run(tee.OnlyChild);
		}

		[Test]
		public void EmptyTest()
		{
			Run(tee.Empty);
		}

		[Test]
		public void ChildTest()
		{
			Run(tee.Child);
		}

		[Test]
		public void DescendantTest()
		{
			Run(tee.Descendant);
		}

		[Test]
		public void AdjacentTest()
		{
			Run(tee.Adjacent);
		}

		[Test]
		public void GeneralSiblingTest()
		{
			Run(tee.GeneralSibling);
		}

		private static void Run(Action action, params object[] args)
		{
			Run(action.Method, args);
		}

        private static void Run(Action<QName> action, params object[] args)
		{
			Run(action.Method, args);
		}

		private static void Run(Action<QName, string> action, params object[] args)
		{
			Run(action.Method, args);
		}

        private static void Run(Action<string> action, params object[] args)
        {
            Run(action.Method, args);
        }
        
        /// <summary>
		/// This is named differently as Mono cannot work out the difference between this and Run(FzActionS2 action, params object[] args)
		/// </summary>
		private static void RunI(Action<int, int> action, params object[] args)
		{
			Run(action.Method, args);
		}

		/// <summary>
	    /// Take the passed action, run it, and then check that the last method
	    /// and last args are the same for pri and sec.
	    /// </summary>
	    private static void Run(MethodBase action, params object[] args)
	    {
            var recordings = new Queue<CallRecording<ISelectorGenerator>>(2);
	        primary.Recorder = recordings.Enqueue;
	        secondary.Recorder = recordings.Enqueue;

	        action.Invoke(tee, args);

	        // Assert the fact that the primary and secondary methods were 
	        // both called with the same arguments and in the right order!
            
	        var recording = recordings.Dequeue();
	        Assert.AreSame(primary, recording.Target);
			Assert.AreEqual(action.Name, MapMethod<ISelectorGenerator>(recording.Method).Name);
	        Assert.AreEqual(args, recording.Arguments);

	        recording = recordings.Dequeue();
	        Assert.AreSame(secondary, recording.Target);
			Assert.AreEqual(action.Name, MapMethod<ISelectorGenerator>(recording.Method).Name);
	        Assert.AreEqual(args, recording.Arguments);
	    }

	    private static MethodInfo MapMethod<T>(MethodInfo method) where T : class
	    {
	        var mapping = method.ReflectedType.GetInterfaceMap(typeof(T));
	        return mapping.InterfaceMethods
	                      .Select((m, i) => new { Source = m, Target = mapping.TargetMethods[i] })
	                      .Single(m => m.Target == method).Source;
	    }

	    private sealed class CallRecording<T>
	    {
	        public T Target { get; private set; }
	        public MethodInfo Method { get; private set; }
	        public object[] Arguments { get; private set; }

	        public CallRecording(T target, MethodInfo method, object[] arguments)
	        {
	            Target = target;
	            Method = method;
	            Arguments = arguments;
	        }
	    }

	    private sealed class FakeSelectorGenerator : ISelectorGenerator
	    {
            public Action<CallRecording<ISelectorGenerator>> Recorder;

	        public void OnInit()
	        {
	            OnInvoked(MethodBase.GetCurrentMethod());
	        }

	        public void OnClose()
	        {
	            OnInvoked(MethodBase.GetCurrentMethod());
	        }

	        public void OnSelector()
	        {
	            OnInvoked(MethodBase.GetCurrentMethod());
	        }

	        public void Type(QName type)
	        {
	            OnInvoked(MethodBase.GetCurrentMethod(), type);
	        }

	        public void Universal()
	        {
	            OnInvoked(MethodBase.GetCurrentMethod());
	        }

	        public void Id(string id)
	        {
	            OnInvoked(MethodBase.GetCurrentMethod(), id);
	        }

	        public void Class(string clazz)
	        {
	            OnInvoked(MethodBase.GetCurrentMethod(), clazz);
	        }

	        public void AttributeExists(QName name)
	        {
	            OnInvoked(MethodBase.GetCurrentMethod(), name);
	        }

	        public void AttributeExact(QName name, string value)
	        {
	            OnInvoked(MethodBase.GetCurrentMethod(), name, value);
	        }

	        public void AttributeIncludes(QName name, string value)
	        {
	            OnInvoked(MethodBase.GetCurrentMethod(), name, value);
	        }

	        public void AttributeDashMatch(QName name, string value)
	        {
	            OnInvoked(MethodBase.GetCurrentMethod(), name, value);
	        }

	        public void AttributePrefixMatch(QName name, string value)
	        {
	            OnInvoked(MethodBase.GetCurrentMethod(), name, value);
	        }

	        public void AttributeSuffixMatch(QName name, string value)
	        {
	            OnInvoked(MethodBase.GetCurrentMethod(), name, value);
	        }

	        public void AttributeSubstring(QName name, string value)
	        {
	            OnInvoked(MethodBase.GetCurrentMethod(), name, value);
	        }

	        public void FirstChild()
	        {
	            OnInvoked(MethodBase.GetCurrentMethod());
	        }

	        public void LastChild()
	        {
	            OnInvoked(MethodBase.GetCurrentMethod());
	        }

	        public void NthChild(int a, int b)
	        {
	            OnInvoked(MethodBase.GetCurrentMethod(), a,b);
	        }

	        public void OnlyChild()
	        {
	            OnInvoked(MethodBase.GetCurrentMethod());
	        }

	        public void Empty()
	        {
	            OnInvoked(MethodBase.GetCurrentMethod());
	        }

	        public void Child()
	        {
	            OnInvoked(MethodBase.GetCurrentMethod());
	        }

	        public void Descendant()
	        {
	            OnInvoked(MethodBase.GetCurrentMethod());
	        }

	        public void Adjacent()
	        {
	            OnInvoked(MethodBase.GetCurrentMethod());
	        }

	        public void GeneralSibling()
	        {
	            OnInvoked(MethodBase.GetCurrentMethod());
	        }
			
	        private void OnInvoked(MethodBase method, params object[] args)
	        {
	            Recorder(new CallRecording<ISelectorGenerator>(this, (MethodInfo) method, args));
	        }
	    }
	}
}