using Alice.VM;
using NUnit.Framework;

namespace Alice.Tweedle.Parsed
{
	[TestFixture]
	public class TweedleExpressionRunTest
	{
		VirtualMachine vm;
		TweedleFrame frame;

		private void Init()
		{
			vm = new VirtualMachine(null);
			frame = new TweedleFrame("Test", vm);
		}

		private TweedleValue RunExpression(string src)
		{
			TweedleExpression expression = new TweedleParser().ParseExpression(src);
			return expression.EvaluateNow();
		}

		private TweedleValue RunExpression(string src, TweedleFrame frame)
		{
			TweedleExpression expression = new TweedleParser().ParseExpression(src);
			return vm.EvaluateToFinish(expression, frame);
		}

		private void RunStatement(string src, TweedleFrame frame)
		{
			TweedleStatement statement = new TweedleParser().ParseStatement(src);
			vm.ExecuteToFinish(statement, frame);
		}

		[Test]
		public void ResultShouldBeCreatedForAddition()
		{
			Init();
			TweedlePrimitiveValue<int> tested = (TweedlePrimitiveValue<int>)RunExpression("12 + 17");
			Assert.AreEqual(29, tested.Value, "The VM should have returned 29.");
		}

		[Test]
		public void CreatedConcatenationExpressionShouldEvaluate()
		{
			Init();
			TweedlePrimitiveValue<string> tested = (TweedlePrimitiveValue<string>)RunExpression("\"hello\" .. \" there\"");

			Assert.AreEqual("hello there", tested.Value, "The StringConcatenationExpression should evaluate to \"hello there\".");
		}

		[Test]
		public void AnAssignmentExpressionShouldUpdateTheFrame()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			frame.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));

			TweedleValue noResult = RunExpression("x <- 3", frame);

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)frame.GetValue("x");
			Assert.AreEqual(3, newVal.Value, "The VM should have returned 3.");
		}

		[Test]
		public void APrimitiveArrayShouldBeCreated()
		{
			Init();
			RunStatement("WholeNumber[] a <- new WholeNumber[] {3, 4, 5};", frame);

			TweedleValue tested = frame.GetValue("a");
			Assert.IsInstanceOf<TweedleArray>(tested);
		}

		[Test]
		public void ArrayValueShouldBeReturned()
		{
			Init();
			RunStatement("WholeNumber[] a <- new WholeNumber[] {3, 4, 5};", frame);
			RunStatement("WholeNumber v <- a[1];", frame);

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)frame.GetValue("v");
			Assert.AreEqual(4, newVal.Value);
		}
	}
}
