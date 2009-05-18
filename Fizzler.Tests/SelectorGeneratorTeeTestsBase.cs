using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;

namespace Fizzler.Tests
{
	public class SelectorGeneratorTeeTestsBase
	{
		private SelectorGeneratorTee _tee;
		private FakeSelectorGenerator _primary;
		private FakeSelectorGenerator _secondary;

		protected SelectorGeneratorTee Tee
		{
			get { return _tee; }
		}

		protected FakeSelectorGenerator Primary
		{
			get { return _primary; }
		}

		protected FakeSelectorGenerator Secondary
		{
			get { return _secondary; }
		}

		[SetUp]
		public void Setup()
		{
			_primary = new FakeSelectorGenerator();
			_secondary = new FakeSelectorGenerator();

			_tee = new SelectorGeneratorTee(
					_primary, _secondary
					);
		}

		/// <summary>
		/// Take the passed action, run it, and then check that the last method
		/// and last args are the same for pri and sec.
		/// </summary>
		/// <param name="action"></param>
		protected void Run(Expression<Action> action)
		{
			MethodCallExpression methodCall = ((MethodCallExpression)action.Body);

			object[] args = (from a in methodCall.Arguments select ((ConstantExpression) a).Value).ToArray();

			methodCall.Method.Invoke(_tee, args);

			// Assert the fact that the pri and sec methods were both called
			Assert.AreEqual(methodCall.Method.Name, _primary.LastMethod);
			Assert.AreEqual(methodCall.Method.Name, _secondary.LastMethod);

			// Assert that the arguments were called
			for(int i = 0; i < args.Length; i++)
			{
				var o = args[i];
				Assert.AreEqual(o, _primary.LastArgs[i]);
				Assert.AreEqual(o, _secondary.LastArgs[i]);
			}
		}

		protected class FakeSelectorGenerator : ISelectorGenerator
		{
			private string _lastMethod;
			private object[] _lastArgs;

			public string LastMethod
			{
				get
				{
					return _lastMethod;
				}
			}

			public object[] LastArgs
			{
				get { return _lastArgs; }
			}

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
			    _lastMethod = method.Name;
				_lastArgs = args;
			}
		}
	}
}