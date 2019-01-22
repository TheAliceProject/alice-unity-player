﻿using Alice.Tweedle.VM;
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
            vm = new TestVirtualMachine(new TweedleSystem());
            vm.Library.Link();
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
            TLocalVariable xDec = new TLocalVariable(TBuiltInTypes.WHOLE_NUMBER, "x");
            scope.SetLocalValue(xDec, TBuiltInTypes.WHOLE_NUMBER.Instantiate(12));

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

        [Test]
        public void DecimalNumberShouldImplicitlyCastToWholeNumber()
        {
            Init();
            RunStatement("DecimalNumber x <- 3.0;", scope);
            RunStatement("WholeNumber y <- x;", scope);

            Assert.IsInstanceOf<TWholeNumberType>(scope.GetValue("y").Type);
        }

        [Test]
        public void WholeNumberShouldImplicitlyCastToDecimalNumber()
        {
            Init();
            RunStatement("WholeNumber x <- 3;", scope);
            RunStatement("DecimalNumber y <- x;", scope);

            Assert.IsInstanceOf<TDecimalNumberType>(scope.GetValue("y").Type);
        }

        [Test]
        public void WholeNumberShouldExplicitlyCastToDecimalNumber()
        {
            Init();
            RunStatement("Any x <- 3 as DecimalNumber;", scope);
            
            Assert.IsInstanceOf<TDecimalNumberType>(scope.GetValue("x").Type);
        }

        [Test]
        public void DecimalNumberShouldExplicitlyCastToWholeNumber()
        {
            Init();
            RunStatement("Any x <- 3.5555 as WholeNumber;", scope);
            
            Assert.IsInstanceOf<TWholeNumberType>(scope.GetValue("x").Type);
        }

        [Test]
        public void DecimalNumberShouldNotExplicitlyCastToTextString()
        {
            Init();
            RunStatement("Any x <- 3.5555 as TextString;", scope);
            
            Assert.IsInstanceOf<TNullType>(scope.GetValue("x").Type);
        }

        [Test]
        public void DecimalNumberIsInstanceOfNumber()
        {
            Init();
            RunStatement("Boolean x <- 3.5 instanceof Number;", scope);
            
            Assert.IsTrue(scope.GetValue("x").ToBoolean());
        }

        [Test]
        public void DecimalNumberIsNotInstanceOfWholeNumber()
        {
            Init();
            RunStatement("Boolean x <- 3.5 instanceof WholeNumber;", scope);
            
            Assert.IsFalse(scope.GetValue("x").ToBoolean());
        }

    }
}
