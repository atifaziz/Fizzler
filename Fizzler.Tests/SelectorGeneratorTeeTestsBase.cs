using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;

namespace Fizzler.Tests
{
	public class SelectorGeneratorTeeTestsBase
	{
	    protected SelectorGeneratorTee Tee { get; private set; }
	    protected FakeSelectorGenerator Primary { get; private set; }
	    protected FakeSelectorGenerator Secondary { get; private set; }

	    [SetUp]
		public void Setup()
		{
			Primary = new FakeSelectorGenerator();
			Secondary = new FakeSelectorGenerator();

			Tee = new SelectorGeneratorTee(
					Primary, Secondary
					);
		}

		/// <summary>
		/// Take the passed action, run it, and then check that the last method
		/// and last args are the same for pri and sec.
		/// </summary>
		protected void Run(Expression<Action> action)
		{
			var methodCall = ((MethodCallExpression)action.Body);

            var args = (from ConstantExpression a in methodCall.Arguments select a.Value).ToArray();

		    var calls = new Queue<Call>(2);
		    Primary.OnCall = calls.Enqueue;
            Secondary.OnCall = calls.Enqueue;

		    var method = MapMethod<ISelectorGenerator>(methodCall.Method);
		    method.Invoke(Tee, args);

            // Assert the fact that the primary and secondary methods were 
            // both called with the same arguments and in the right order!
            
            var call = calls.Dequeue();
            Assert.AreSame(Primary, call.Target);
            Assert.AreSame(method, MapMethod<ISelectorGenerator>(call.Method));
            Assert.AreEqual(args, call.Arguments);

            call = calls.Dequeue();
            Assert.AreSame(Secondary, call.Target);
            Assert.AreSame(method, MapMethod<ISelectorGenerator>(call.Method));
            Assert.AreEqual(args, call.Arguments);
		}

        private static MethodInfo MapMethod<T>(MethodInfo method) where T : class
        {
            var mapping = method.ReflectedType.GetInterfaceMap(typeof(T));
            return mapping.InterfaceMethods
                          .Select((m, i) => new { Source = m, Target = mapping.TargetMethods[i] })
                          .Single(m => m.Target == method).Source;
        }

	    protected sealed class Call
        {
            public object Target { get; private set; }
            public MethodInfo Method { get; private set; }
            public object[] Arguments { get; private set; }

            public Call(object target, MethodInfo method, object[] arguments)
            {
                Target = target;
                Method = method;
                Arguments = arguments;
            }
        }

	    protected class FakeSelectorGenerator : ISelectorGenerator
		{
	        public Action<Call> OnCall;

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
                OnCall(new Call(this, (MethodInfo)method, args));
			}
		}
	}
}