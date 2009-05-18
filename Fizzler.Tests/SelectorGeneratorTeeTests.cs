using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
			Run(tee => tee.OnInit());
		}

		[Test]
		public void OnCloseTest()
		{
			Run(tee => tee.OnClose());
		}

		[Test]
		public void OnSelectorTest()
		{
			Run(tee => tee.OnSelector());
		}

		[Test]
		public void TypeTest()
		{
			Run(tee => tee.Type("go"));
		}

		[Test]
		public void UniversalTest()
		{
			Run(tee => tee.Universal());
		}

		[Test]
		public void IdTest()
		{
			Run(tee => tee.Id("hello"));
		}

		[Test]
		public void ClassTest()
		{
			Run(tee => tee.Class("hello"));
		}

		[Test]
		public void AttrExistsTest()
		{
			Run(tee => tee.AttributeExists("hello"));
		}

		[Test]
		public void AttExactTest()
		{
			Run(tee => tee.AttributeExact("hello", "there"));
		}

		[Test]
		public void AttrIncludesTest()
		{
			Run(tee => tee.AttributeIncludes("hello", "there"));
		}

		[Test]
		public void AttrDashMatchTest()
		{
			Run(tee => tee.AttributeDashMatch("hello", "there"));
		}

		[Test]
		public void AttrPrefixMatchTest()
		{
			Run(tee => tee.AttributePrefixMatch("hello", "there"));
		}

		[Test]
		public void AttrSuffixMatchTest()
		{
			Run(tee => tee.AttributeSuffixMatch("hello", "there"));
		}

		[Test]
		public void AttrSubstringTest()
		{
			Run(tee => tee.AttributeSubstring("hello", "there"));
		}

		[Test]
		public void FirstChildTest()
		{
			Run(tee => tee.FirstChild());
		}

		[Test]
		public void LastChildTest()
		{
			Run(tee => tee.LastChild());
		}

		[Test]
		public void NthChildTest()
		{
			Run(tee => tee.NthChild(1,2));
		}

		[Test]
		public void OnlyChildTest()
		{
			Run(tee => tee.OnlyChild());
		}

		[Test]
		public void EmptyTest()
		{
			Run(tee => tee.Empty());
		}

		[Test]
		public void ChildTest()
		{
			Run(tee => tee.Child());
		}

		[Test]
		public void DescendantTest()
		{
			Run(tee => tee.Descendant());
		}

		[Test]
		public void AdjacentTest()
		{
			Run(tee => tee.Adjacent());
		}

		[Test]
		public void GeneralSiblingTest()
		{
			Run(tee => tee.GeneralSibling());
		}

	    /// <summary>
	    /// Take the passed action, run it, and then check that the last method
	    /// and last args are the same for pri and sec.
	    /// </summary>
	    private static void Run(Expression<Action<ISelectorGenerator>> action)
	    {
	        var call = ((MethodCallExpression) action.Body);
	        var args = (from ConstantExpression arg in call.Arguments select arg.Value).ToArray();

            var primary = new FakeSelectorGenerator();
            var secondary = new FakeSelectorGenerator();
            var tee = new SelectorGeneratorTee(primary, secondary);
            
            var recordings = new Queue<CallRecording<ISelectorGenerator>>(2);
	        primary.Recorder = recordings.Enqueue;
	        secondary.Recorder = recordings.Enqueue;

	        var method = call.Method;
	        method.Invoke(tee, args);

	        // Assert the fact that the primary and secondary methods were 
	        // both called with the same arguments and in the right order!
            
	        var recording = recordings.Dequeue();
	        Assert.AreSame(primary, recording.Target);
	        Assert.AreSame(method, MapMethod<ISelectorGenerator>(recording.Method));
	        Assert.AreEqual(args, recording.Arguments);

	        recording = recordings.Dequeue();
	        Assert.AreSame(secondary, recording.Target);
	        Assert.AreSame(method, MapMethod<ISelectorGenerator>(recording.Method));
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

	        public void Type(string type)
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

	        public void AttributeExists(string name)
	        {
	            OnInvoked(MethodBase.GetCurrentMethod(), name);
	        }

	        public void AttributeExact(string name, string value)
	        {
	            OnInvoked(MethodBase.GetCurrentMethod(), name, value);
	        }

	        public void AttributeIncludes(string name, string value)
	        {
	            OnInvoked(MethodBase.GetCurrentMethod(), name, value);
	        }

	        public void AttributeDashMatch(string name, string value)
	        {
	            OnInvoked(MethodBase.GetCurrentMethod(), name, value);
	        }

	        public void AttributePrefixMatch(string name, string value)
	        {
	            OnInvoked(MethodBase.GetCurrentMethod(), name, value);
	        }

	        public void AttributeSuffixMatch(string name, string value)
	        {
	            OnInvoked(MethodBase.GetCurrentMethod(), name, value);
	        }

	        public void AttributeSubstring(string name, string value)
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