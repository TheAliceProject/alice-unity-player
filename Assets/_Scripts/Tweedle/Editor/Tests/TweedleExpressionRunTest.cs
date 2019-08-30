﻿using System.Text.RegularExpressions;
using Alice.Tweedle.VM;
using NUnit.Framework;
using UnityEngine.TestTools;

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
        public void AnAssignmentExpressionShouldUpdateArrayElement()
        {
            Init();
            RunStatement("WholeNumber[] a <- new WholeNumber[] {3, 4, 5};", scope);
            RunStatement("a[1] <- 7;", scope);
            RunStatement("WholeNumber v <- a[1];", scope);

            TValue newVal = scope.GetValue("v");
            Assert.AreEqual(7, newVal.ToInt());
        }

        [Test]
        public void DecimalNumberShouldNotImplicitlyCastToWholeNumber()
        {
            Init();
            RunStatement("DecimalNumber x <- 3.0;", scope);

            LogAssert.Expect(UnityEngine.LogType.Error, new Regex("Unable to treat value 3 of type DecimalNumber as type WholeNumber"));
            Assert.Throws<TweedleRuntimeException>(() => {
                RunStatement("WholeNumber y <- x;", scope);
            });
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
        public void DecimalNumberShouldNotExplicitlyCastToTextString()
        {
            Init();
            LogAssert.Expect(UnityEngine.LogType.Error, new Regex("Cannot cast type DecimalNumber to type TextString"));
            Assert.Throws<TweedleRuntimeException>(() => {
                RunStatement("Any x <- 3.5555 as TextString;", scope);
            });
        }

        [Test]
        public void DecimalNumberShouldBeInstanceOfNumber()
        {
            Init();
            RunStatement("Boolean x <- 3.5 instanceof Number;", scope);
            Assert.IsTrue(scope.GetValue("x").ToBoolean());
        }

        [Test]
        public void DecimalNumberShouldNotBeInstanceOfWholeNumber()
        {
            Init();
            RunStatement("Boolean x <- 3.5 instanceof WholeNumber;", scope);
            
            Assert.IsFalse(scope.GetValue("x").ToBoolean());
        }

        [Test]
        public void WholeNumberShouldNotBeInstanceOfDecimalNumber()
        {
            Init();
            RunStatement("Boolean x <- 3 instanceof DecimalNumber;", scope);
            
            Assert.IsFalse(scope.GetValue("x").ToBoolean());
        }

        [Test]
        public void ArithmeticExpressionShouldFollowNormalOrderOfOperations()
        {
            Init();
            TValue tested = RunExpression("3 - 4 * 3 + 12 / 3 + 6 % 5");
            Assert.AreEqual(-4, tested.ToInt(), "The VM should have returned -4.");
        }

        [Test]
        public void ConcatOperatorShouldHappenAfterArithmeticOperators()
        {
            Init();
            TValue tested = RunExpression("\"\" .. 3 + 4");
            Assert.AreEqual("7", tested.ToTextString(), "The VM should have returned a text string");
        }

        [Test]
        public void InstanceofOperatorShouldHappenAfterAsOperator()
        {
            Init();
            RunStatement("Boolean x <- 3 as DecimalNumber instanceof WholeNumber;", scope);
            Assert.IsFalse(scope.GetValue("x").ToBoolean());
        }

        [Test]
        public void AsOperatorShouldHappenBeforeArithmeticOperators()
        {
            Init();
            RunStatement("Boolean x <- 3 instanceof DecimalNumber;", scope);
            TValue tested = RunExpression("4 + 3.6 as WholeNumber", scope);
            Assert.AreEqual(7, tested.ToInt(), "The VM should have returned 7");
        }

        [Test]
        public void InstanceofOperatorShouldHappenBeforeEqualityOperatorsAndAfterOtherComparisonOperators()
        {
            Init();
            RunStatement("Boolean x <- 3 > 5 instanceof Boolean;", scope);
            Assert.IsTrue(scope.GetValue("x").ToBoolean());
        }

    }
}
