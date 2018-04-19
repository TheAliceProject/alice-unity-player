using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Alice.Tweedle.Unlinked
{
	public class TweedleExpressionParseTest
	{
		private TweedleExpression ParseExpression(string src)
		{
			return new TweedleUnlinkedParser().ParseExpression(src);
		}

		[Test]
		public void SomethingShouldBeCreatedForThis()
		{
			TweedleExpression tested = ParseExpression("this");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AThisExpressionShouldBeCreated()
		{
			TweedleExpression tested = ParseExpression("this");

			Assert.IsInstanceOf<ThisExpression>(tested, "The parser should have returned a ThisExpression.");
		}

		[Test]
		public void SomethingShouldBeCreatedForAddition()
		{
			TweedleExpression tested = ParseExpression("3 + 4");

			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AnAdditionExpressionShouldBeCreated()
		{
			TweedleExpression tested = ParseExpression("3 + 4");

			Assert.IsInstanceOf<AdditionWholeExpression>(tested, "The parser should have returned an AdditionExpression.");
		}

		/*	[Test]
			public void additionExpressionShouldHaveLeft() {
				AdditionExpression tested = (AdditionExpression) ParseExpression( "3 + 4" );
				Assert.True("The AdditionExpression should have a left hand side.", tested. );
			}*/

		[Test]
		public void CreatedAdditionExpressionShouldEvaluate()
		{
			AdditionWholeExpression tested = (AdditionWholeExpression)ParseExpression("3 + 4");

			Assert.NotNull(tested.Evaluate(null), "The AdditionExpression should evaluate to something.");
		}

		[Test]
		public void CreatedAdditionExpressionShouldEvaluateToAPrimitiveValue()
		{
			AdditionWholeExpression tested = (AdditionWholeExpression)ParseExpression("3 + 4");

			Assert.IsInstanceOf<TweedlePrimitiveValue>(tested.Evaluate(null), "The AdditionExpression should evaluate to a tweedle value.");
		}

		[Test]
		public void CreatedAdditionExpressionShouldContainCorrectResult()
		{
			AdditionWholeExpression tested = (AdditionWholeExpression)ParseExpression("3 + 4");

			Assert.AreEqual(7,
				((TweedlePrimitiveValue<int>)tested.Evaluate(null)).Value,
				"The AdditionExpression should evaluate correctly.");
		}

		[Test]
		public void WholeNumberAdditionShouldKnowItsType()
		{
			TweedleExpression tested = ParseExpression("3 + 4");
			Assert.AreEqual(TweedleTypes.WHOLE_NUMBER, 
				tested.Type,
				"The type should be WholeNumber");
		}
	}
}
