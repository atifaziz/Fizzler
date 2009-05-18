using System;
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

		    var method = methodCall.Method;
		    method.Invoke(Tee, args);

			// Assert the fact that the pri and sec methods were both called
			Assert.AreEqual(method.Name, Primary.LastMethod);
			Assert.AreEqual(method.Name, Secondary.LastMethod);

			Assert.AreEqual(args, Primary.LastArgs);
			Assert.AreEqual(args, Secondary.LastArgs);
		}

		protected class FakeSelectorGenerator : ISelectorGenerator
		{
		    public string LastMethod { get; private set; }
		    public object[] LastArgs { get; private set; }

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
			    LastMethod = method.Name;
				LastArgs = args;
			}
		}
	}
}