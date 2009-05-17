using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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
				SetupMethodCallExpectations();
			}

			public void OnClose()
			{
				SetupMethodCallExpectations();
			}

			public void OnSelector()
			{
				SetupMethodCallExpectations();
			}

			public void Type(string type)
			{
				SetupMethodCallExpectations(type);
			}

			public void Universal()
			{
				SetupMethodCallExpectations();
			}

			public void Id(string id)
			{
				SetupMethodCallExpectations(id);
			}

			public void Class(string clazz)
			{
				SetupMethodCallExpectations(clazz);
			}

			public void AttributeExists(string name)
			{
				SetupMethodCallExpectations(name);
			}

			public void AttributeExact(string name, string value)
			{
				SetupMethodCallExpectations(name, value);
			}

			public void AttributeIncludes(string name, string value)
			{
				SetupMethodCallExpectations(name, value);
			}

			public void AttributeDashMatch(string name, string value)
			{
				SetupMethodCallExpectations(name, value);
			}

			public void AttributePrefixMatch(string name, string value)
			{
				SetupMethodCallExpectations(name, value);
			}

			public void AttributeSuffixMatch(string name, string value)
			{
				SetupMethodCallExpectations(name, value);
			}

			public void AttributeSubstring(string name, string value)
			{
				SetupMethodCallExpectations(name, value);
			}

			public void FirstChild()
			{
				SetupMethodCallExpectations();
			}

			public void LastChild()
			{
				SetupMethodCallExpectations();
			}

			public void NthChild(int a, int b)
			{
				SetupMethodCallExpectations(a,b);
			}

			public void OnlyChild()
			{
				SetupMethodCallExpectations();
			}

			public void Empty()
			{
				SetupMethodCallExpectations();
			}

			public void Child()
			{
				SetupMethodCallExpectations();
			}

			public void Descendant()
			{
				SetupMethodCallExpectations();
			}

			public void Adjacent()
			{
				SetupMethodCallExpectations();
			}

			public void GeneralSibling()
			{
				SetupMethodCallExpectations();
			}
			
			private void SetupMethodCallExpectations(params object[] ps)
			{
				_lastMethod = GetCallingStackFrame().GetMethod().Name;
				_lastArgs = ps;
			}

			private static StackFrame GetCallingStackFrame()
			{
				return new StackTrace(1, true).GetFrames()[1];
			}
		}
	}
}