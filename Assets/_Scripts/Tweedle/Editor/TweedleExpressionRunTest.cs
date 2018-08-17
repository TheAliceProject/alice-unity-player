using Alice.VM;
using NUnit.Framework;

namespace Alice.Tweedle.Parse
{
	[TestFixture]
	public class TweedleExpressionRunTest
	{
		TestVirtualMachine vm;
		ExecutionScope scope;

		private void Init()
		{
			vm = new TestVirtualMachine(null);
			scope = new ExecutionScope("Test", vm);
		}

		private TweedleValue RunExpression(string src)
		{
			TweedleExpression expression = new TweedleParser().ParseExpression(src);
			return expression.EvaluateNow();
		}

		private TweedleValue RunExpression(string src, ExecutionScope scope)
		{
			TweedleExpression expression = new TweedleParser().ParseExpression(src);
			return vm.EvaluateToFinish(expression, scope);
		}

		private void RunStatement(string src, ExecutionScope scope)
		{
			TweedleStatement statement = new TweedleParser().ParseStatement(src);
			vm.ExecuteToFinish(statement, scope);
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
		public void AnAssignmentExpressionShouldUpdateTheScope()
		{
			Init();
			TweedleLocalVariable xDec = new TweedleLocalVariable(TweedleTypes.WHOLE_NUMBER, "x");
			scope.SetLocalValue(xDec, TweedleTypes.WHOLE_NUMBER.Instantiate(12));

			TweedleValue noResult = RunExpression("x <- 3", scope);

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)scope.GetValue("x");
			Assert.AreEqual(3, newVal.Value, "The VM should have returned 3.");
		}

		[Test]
		public void APrimitiveArrayShouldBeCreated()
		{
			Init();
			RunStatement("WholeNumber[] a <- new WholeNumber[] {3, 4, 5};", scope);

			TweedleValue tested = scope.GetValue("a");
			Assert.IsInstanceOf<TweedleArray>(tested);
		}

		[Test]
		public void ArrayValueShouldBeReturned()
		{
			Init();
			RunStatement("WholeNumber[] a <- new WholeNumber[] {3, 4, 5};", scope);
			RunStatement("WholeNumber v <- a[1];", scope);

			TweedlePrimitiveValue<int> newVal = (TweedlePrimitiveValue<int>)scope.GetValue("v");
			Assert.AreEqual(4, newVal.Value);
		}
	}
}
