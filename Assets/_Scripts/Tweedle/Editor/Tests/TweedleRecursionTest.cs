using Alice.Tweedle.VM;
using NUnit.Framework;

namespace Alice.Tweedle.Parse
{
	[TestFixture]
	public class TweedleRecursionTest
	{
		static TweedleParser parser = new TweedleParser();

		static TClassType ParseClass(string src)
		{
			return (TClassType)parser.ParseType(src);
		}

		static string fib =
			"class Fibonacci {\n" +
			"  Fibonacci()\n{}\n" +
			"  WholeNumber compute(WholeNumber n)\n{\n" +
			"    if (n==1 || n==0) \n" +
			"      { return 1 }\n" +
			"    else\n" +
			"      {return compute(n: n-2) + compute(n: n-1)}\n" +
			"  }\n" +
			"}";


		TweedleSystem NewSystem()
		{
			TweedleSystem system = new TweedleSystem();
			system.GetRuntimeAssembly().Add(ParseClass(fib));
            system.Link();
            return system;
		}

		TestVirtualMachine vm;
		ExecutionScope scope;

		public void Init()
		{
			vm = new TestVirtualMachine(NewSystem());
			scope = new ExecutionScope("Test", vm);
		}

		void ExecuteStatement(string src)
		{
			vm.ExecuteToFinish(parser.ParseStatement(src), scope);
		}

		[Test]
		public void AClassShouldBeCreatedForFib()
		{
			TClassType tested = ParseClass(fib);
			Assert.NotNull(tested, "The parser should have returned something.");
		}

		[Test]
		public void AnObjectShouldBeCreatedByConstruction()
		{
			Init();
			ExecuteStatement("Fibonacci fib <- new Fibonacci();");
			var tested = scope.GetValue("fib");

			Assert.IsInstanceOf<TObject>(tested.Object());
		}

		[Test]
		public void Fib1ShouldReturnValue()
		{
			Init();
			ExecuteStatement("Fibonacci fib <- new Fibonacci();");
			ExecuteStatement("WholeNumber x <- fib.compute(n: 1);");
			TValue tested = scope.GetValue("x");

			Assert.IsInstanceOf<TWholeNumberType>(tested.Type);
		}

		[Test]
		public void Fib1ShouldBe1()
		{
			Init();
			ExecuteStatement("Fibonacci fib <- new Fibonacci();");
			ExecuteStatement("WholeNumber x <- fib.compute(n: 1);");
			TValue tested = scope.GetValue("x");

			Assert.AreEqual(1, tested.ToInt());
		}

		[Test]
		public void Fib2ShouldBe2()
		{
			Init();
			ExecuteStatement("Fibonacci fib <- new Fibonacci();");
			ExecuteStatement("WholeNumber x <- fib.compute(n: 2);");
			TValue tested = scope.GetValue("x");

			Assert.AreEqual(2, tested.ToInt());
		}

		[Test]
		public void Fib3ShouldBe3()
		{
			Init();
			ExecuteStatement("Fibonacci fib <- new Fibonacci();");
			ExecuteStatement("WholeNumber x <- fib.compute(n: 3);");
			TValue tested = scope.GetValue("x");

			Assert.AreEqual(3, tested.ToInt());
		}

		[Test]
		public void Fib4ShouldBe5()
		{
			Init();
			ExecuteStatement("Fibonacci fib <- new Fibonacci();");
			ExecuteStatement("WholeNumber x <- fib.compute(n: 4);");
			TValue tested = scope.GetValue("x");

			Assert.AreEqual(5, tested.ToInt());
		}

		[Test]
		public void Fib5ShouldBe8()
		{
			Init();
			ExecuteStatement("Fibonacci fib <- new Fibonacci();");
			ExecuteStatement("WholeNumber x <- fib.compute(n: 5);");
			TValue tested = scope.GetValue("x");

			Assert.AreEqual(8, tested.ToInt());
		}

		[Test]
		public void Fib8ShouldBe34()
		{
			Init();
			ExecuteStatement("Fibonacci fib <- new Fibonacci();");
			ExecuteStatement("WholeNumber x <- fib.compute(n: 8);");
			TValue tested = scope.GetValue("x");

			Assert.AreEqual(34, tested.ToInt());
		}

		[Test]
		public void Fib15ShouldBe987()
		{
			Init();
			ExecuteStatement("Fibonacci fib <- new Fibonacci();");
			ExecuteStatement("WholeNumber x <- fib.compute(n: 15);");
			TValue tested = scope.GetValue("x");

			Assert.AreEqual(987, tested.ToInt());
		}
	}
}
