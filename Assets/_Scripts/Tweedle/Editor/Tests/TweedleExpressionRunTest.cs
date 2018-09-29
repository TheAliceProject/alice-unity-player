using Alice.Tweedle.VM;
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

		private TValue RunExpression(string src)
		{
			ITweedleExpression expression = new TweedleParser().ParseExpression(src);
			return expression.EvaluateNow();
		}

		private TValue RunExpression(string src, ExecutionScope scope)
		{
			ITweedleExpression expression = new TweedleParser().ParseExpression(src);
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
			TValue tested = RunExpression("12 + 17");
			Assert.AreEqual(29, tested.ToInt(), "The VM should have returned 29.");
		}

		[Test]
		public void CreatedConcatenationExpressionShouldEvaluate()
		{
			Init();
			TValue tested = RunExpression("\"hello\" .. \" there\"");

			Assert.AreEqual("hello there", tested.ToTextString(), "The StringConcatenationExpression should evaluate to \"hello there\".");
		}

		[Test]
		public void AnAssignmentExpressionShouldUpdateTheScope()
		{
			Init();
			TLocalVariable xDec = new TLocalVariable(TStaticTypes.WHOLE_NUMBER, "x");
			scope.SetLocalValue(xDec, TStaticTypes.WHOLE_NUMBER.Instantiate(12));

			TValue noResult = RunExpression("x <- 3", scope);

			TValue newVal = scope.GetValue("x");
			Assert.AreEqual(3, newVal.ToInt(), "The VM should have returned 3.");
		}

		[Test]
		public void APrimitiveArrayShouldBeCreated()
		{
			Init();
			RunStatement("WholeNumber[] a <- new WholeNumber[] {3, 4, 5};", scope);

			TValue tested = scope.GetValue("a");
			Assert.IsInstanceOf<TArray>(tested.Array());
		}

		[Test]
		public void ArrayValueShouldBeReturned()
		{
			Init();
			RunStatement("WholeNumber[] a <- new WholeNumber[] {3, 4, 5};", scope);
			RunStatement("WholeNumber v <- a[1];", scope);

			TValue newVal = scope.GetValue("v");
			Assert.AreEqual(4, newVal.ToInt());
		}
	}
}
