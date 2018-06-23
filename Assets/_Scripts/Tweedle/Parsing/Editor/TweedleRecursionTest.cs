﻿using Alice.VM;
using NUnit.Framework;

namespace Alice.Tweedle.Parsed
{
    [TestFixture]
    public class TweedleRecursionTest
    {
        static TweedleParser parser = new TweedleParser();

        static TweedleClass ParseClass(string src)
        {
            return (TweedleClass)parser.ParseType(src);
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
            system.AddClass(ParseClass(fib));
            return system;
        }

        VirtualMachine vm;
        TweedleFrame frame;

        public void Init()
        {
            vm = new VirtualMachine(NewSystem());
            frame = new TweedleFrame("Test", vm);
        }

        void ExecuteStatement(string src)
        {
            vm.ExecuteToFinish(parser.ParseStatement(src), frame);
        }

        [Test]
        public void AClassShouldBeCreatedForFib()
        {
            TweedleClass tested = ParseClass(fib);
            Assert.NotNull(tested, "The parser should have returned something.");
        }

        [Test]
        public void AnObjectShouldBeCreatedByConstruction()
        {
            Init();
			ExecuteStatement("Fibonacci fib <- new Fibonacci();");
            var tested = frame.GetValue("fib");

            Assert.IsInstanceOf<TweedleObject>(tested);
		}

        [Test]
        public void Fib1ShouldReturnValue()
        {
            Init();
			ExecuteStatement("Fibonacci fib <- new Fibonacci();");
            ExecuteStatement("WholeNumber x <- fib.compute(n: 1);");
            TweedleValue tested = frame.GetValue("x");

            Assert.IsInstanceOf<TweedlePrimitiveValue<int>>(tested);
		}

        [Test]
        public void Fib1ShouldBe1()
        {
            Init();
			ExecuteStatement("Fibonacci fib <- new Fibonacci();");
            ExecuteStatement("WholeNumber x <- fib.compute(n: 1);");
            TweedleValue tested = frame.GetValue("x");

            Assert.AreEqual(1, ((TweedlePrimitiveValue<int>)tested).Value);
		}

        [Test]
        public void Fib2ShouldBe2()
        {
            Init();
            ExecuteStatement("Fibonacci fib <- new Fibonacci();");
            ExecuteStatement("WholeNumber x <- fib.compute(n: 2);");
            TweedleValue tested = frame.GetValue("x");

            Assert.AreEqual(2, ((TweedlePrimitiveValue<int>)tested).Value);
		}

        [Test]
        public void Fib3ShouldBe3()
        {
            Init();
            ExecuteStatement("Fibonacci fib <- new Fibonacci();");
            ExecuteStatement("WholeNumber x <- fib.compute(n: 3);");
            TweedleValue tested = frame.GetValue("x");

            Assert.AreEqual(3, ((TweedlePrimitiveValue<int>)tested).Value);
		}

        [Test]
        public void Fib4ShouldBe5()
        {
            Init();
            ExecuteStatement("Fibonacci fib <- new Fibonacci();");
            ExecuteStatement("WholeNumber x <- fib.compute(n: 4);");
            TweedleValue tested = frame.GetValue("x");

            Assert.AreEqual(5, ((TweedlePrimitiveValue<int>)tested).Value);
		}

        [Test]
        public void Fib5ShouldBe8()
        {
            Init();
            ExecuteStatement("Fibonacci fib <- new Fibonacci();");
            ExecuteStatement("WholeNumber x <- fib.compute(n: 5);");
            TweedleValue tested = frame.GetValue("x");

            Assert.AreEqual(8, ((TweedlePrimitiveValue<int>)tested).Value);
		}

        [Test]
        public void Fib8ShouldBe34()
        {
            Init();
            ExecuteStatement("Fibonacci fib <- new Fibonacci();");
            ExecuteStatement("WholeNumber x <- fib.compute(n: 8);");
            TweedleValue tested = frame.GetValue("x");

            Assert.AreEqual(34, ((TweedlePrimitiveValue<int>)tested).Value);
		}

        [Test]
        [Ignore("Not fast enough yet")]
        public void Fib15ShouldBe987()
        {
            Init();
            ExecuteStatement("Fibonacci fib <- new Fibonacci();");
            ExecuteStatement("WholeNumber x <- fib.compute(n: 15);");
            TweedleValue tested = frame.GetValue("x");

            Assert.AreEqual(987, ((TweedlePrimitiveValue<int>)tested).Value);
        }
    }
}