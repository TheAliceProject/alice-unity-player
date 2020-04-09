using Alice.Tweedle.VM;
using NUnit.Framework;

namespace Alice.Tweedle.Parse
{
    [TestFixture]
    public class TweedleRecursionTest
    {
        static readonly TweedleParser Parser = new TweedleParser();

        static TClassType ParseClass(string src)
        {
            return (TClassType)Parser.ParseType(src);
        }

        private const string Fib = @"class Fibonacci {
  Fibonacci()
  {}

  WholeNumber compute(WholeNumber n) {
    if (n==1 || n==0) {
      return 1;
    } else {
      return compute(n: n-2) + compute(n: n-1)};
    }
  }";

        private const string FibTogether = @"class ParallelFibonacci {
  ParallelFibonacci()
  {}

  WholeNumber compute(WholeNumber n) {
    if (n==1 || n==0) {
      return 1;
    } else {
      WholeNumber n2 <- 0;
      WholeNumber n1 <- 0;
      doTogether {
        n2 <- compute(n: n-2);
        n1 <- compute(n: n-1);
      }
      return n2 + n1;
    }
  }";


        TweedleSystem NewSystem()
        {
            TweedleSystem system = new TweedleSystem();
            system.GetRuntimeAssembly().Add(ParseClass(Fib));
            system.GetRuntimeAssembly().Add(ParseClass(FibTogether));
            system.Link();
            return system;
        }

        TestVirtualMachine vm;
        ExecutionScope scope;

        private void Init()
        {
            vm = new TestVirtualMachine(NewSystem());
            scope = new ExecutionScope("Test", vm);
        }

        void ExecuteStatement(string src)
        {
            TestVirtualMachine.ExecuteToFinish(Parser.ParseStatement(src), scope);
        }

        [Test]
        public void AClassShouldBeCreatedForFib()
        {
            TClassType tested = ParseClass(Fib);
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void AClassShouldBeCreatedForTogetherFib()
        {
            TClassType tested = ParseClass(FibTogether);
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
        public void AnObjectShouldBeCreatedByConstructionOfParallel()
        {
            Init();
            ExecuteStatement("ParallelFibonacci fib <- new ParallelFibonacci();");
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
        public void ParallelFib1ShouldReturnValue()
        {
            Init();
            ExecuteStatement("ParallelFibonacci fib <- new ParallelFibonacci();");
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
        public void ParallelFib1ShouldBe1()
        {
            Init();
            ExecuteStatement("ParallelFibonacci fib <- new ParallelFibonacci();");
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
        public void ParallelFib2ShouldBe2()
        {
            Init();
            ExecuteStatement("ParallelFibonacci fib <- new ParallelFibonacci();");
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
        public void ParallelFib3ShouldBe3()
        {
            Init();
            ExecuteStatement("ParallelFibonacci fib <- new ParallelFibonacci();");
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
        public void ParallelFib4ShouldBe5()
        {
            Init();
            ExecuteStatement("ParallelFibonacci fib <- new ParallelFibonacci();");
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
        public void ParallelFib5ShouldBe8()
        {
            Init();
            ExecuteStatement("ParallelFibonacci fib <- new ParallelFibonacci();");
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
        public void ParallelFib8ShouldBe34()
        {
            Init();
            ExecuteStatement("ParallelFibonacci fib <- new ParallelFibonacci();");
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

        [Test]
        public void ParallelFib15ShouldBe987()
        {
            Init();
            ExecuteStatement("ParallelFibonacci fib <- new ParallelFibonacci();");
            ExecuteStatement("WholeNumber x <- fib.compute(n: 15);");
            TValue tested = scope.GetValue("x");

            Assert.AreEqual(987, tested.ToInt());
        }
    }
}
