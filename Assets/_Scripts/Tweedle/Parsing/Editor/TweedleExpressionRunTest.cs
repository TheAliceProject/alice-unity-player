using NUnit.Framework;

namespace Alice.Tweedle.Parsed
{
	[TestFixture]
	public class TweedleExpressionRunTest
	{
		private TweedleValue RunExpression(string src)
		{
			TweedleExpression expression = new TweedleParser().ParseExpression(src);
			return expression.EvaluateNow();
		}

		private TweedleValue RunExpression(string src, TweedleFrame frame)
		{
			TweedleExpression expression = new TweedleParser().ParseExpression(src);
			return expression.EvaluateNow(frame);
		}


		[Test]
		public void ResultShouldBeCreatedForAddition()
		{
			TweedlePrimitiveValue<int> tested = (TweedlePrimitiveValue<int>)RunExpression("12 + 17");
			Assert.AreEqual(29, tested.Value, "The VM should have returned 29.");
		}

		[Test]
		public void CreatedConcatenationExpressionShouldEvaluate()
		{
			TweedlePrimitiveValue<string> tested = (TweedlePrimitiveValue<string>)RunExpression("\"hello\" .. \" there\"");

			Assert.AreEqual("hello there", tested.Value, "The StringConcatenationExpression should evaluate to \"hello there\".");
		}

		[Test]
		public void AnAssignmentExpressionShouldUpdateTheFrame()
		{
			TweedleFrame frame = new TweedleFrame(null);
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			frame.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));

			TweedleValue noResult = RunExpression("x <- 3", frame);
			UnityEngine.Debug.Log("Null? " + noResult);
			//TweedlePrimitiveValue<int> tested = (TweedlePrimitiveValue<int>) RunExpression("x <- 3", frame);

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)frame.GetValue("x");
			UnityEngine.Debug.Log("Assignment " + newVal);
			Assert.AreEqual(3, newVal.Value, "The VM should have returned 3.");
		}
	}
}
