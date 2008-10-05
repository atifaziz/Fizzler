using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
	[TestClass]
	public class MultipleSelectors : SelectorBaseTest
	{
		[TestMethod]
		public void CommaSupport_With_No_Space()
		{
			var result = Parser.Parse("p.hiclass,a");

			Assert.AreEqual(2, result.Count);
		}

		[TestMethod]
		public void CommaSupport_With_Post_Pended_Space()
		{
			var result = Parser.Parse("p.hiclass, a");
			
			Assert.AreEqual(2, result.Count);
		}

		[TestMethod]
		public void CommaSupport_With_Pre_Post_Pended_Space()
		{
			var result = Parser.Parse("p.hiclass , a");

			Assert.AreEqual(2, result.Count);
		}

		[TestMethod]
		public void CommaSupport_With_Pre_Pended_Space()
		{
			var result = Parser.Parse("p.hiclass ,a");

			Assert.AreEqual(2, result.Count);
		}
	}
}