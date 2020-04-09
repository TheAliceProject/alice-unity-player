using System.Text.RegularExpressions;
using Alice.Player.Primitives;
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
            var tweedleSystem = new TweedleSystem();
            tweedleSystem.AddStaticAssembly(Player.PlayerAssemblies.Assembly(Player.PlayerAssemblies.CURRENT));
            vm = new TestVirtualMachine(tweedleSystem);
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
            TestVirtualMachine.ExecuteToFinish(statement, scope);
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
        public void DecimalNumberShouldImplicitlyCastToTextString()
        {
            Init();
            RunStatement("TextString x <- 3.5555;", scope);
            Assert.AreEqual("3.5555", scope.GetValue("x").ToTextString(), "The value should be \"3.5555\".");
        }

        [Test]
        public void DecimalNumberShouldExplicitlyCastToTextString()
        {
            Init();
            RunStatement("TextString x <- 3.5555 as TextString;", scope);
            Assert.AreEqual("3.5555", scope.GetValue("x").ToTextString(), "The value should be \"3.5555\".");
        }

        [Test]
        public void DecimalNumberShouldNotBeInstanceOfString()
        {
            Init();
            RunStatement("Boolean x <- 3.5 instanceof TextString;", scope);
            Assert.IsFalse(scope.GetValue("x").ToBoolean());
        }

        [Test]
        public void DecimalNumberShouldBeInstanceOfNumber()
        {
            Init();
            RunStatement("Boolean x <- 3.5 instanceof Number;", scope);
            Assert.IsTrue(scope.GetValue("x").ToBoolean());
        }

        [Test]
        public void DecimalNumberShouldBeInstanceOfDecimalNumber()
        {
            Init();
            RunStatement("Boolean x <- 3.5 instanceof DecimalNumber;", scope);
            
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
        public void WholeNumberShouldBeInstanceOfNumber()
        {
            Init();
            RunStatement("Boolean x <- 3 instanceof Number;", scope);
            Assert.IsTrue(scope.GetValue("x").ToBoolean());
        }

        [Test]
        public void WholeNumberShouldBeInstanceOfWholeNumber()
        {
            Init();
            RunStatement("Boolean x <- 3 instanceof WholeNumber;", scope);
            
            Assert.IsTrue(scope.GetValue("x").ToBoolean());
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

        [Test]
        public void AColorShouldBeCreated()
        {
            Init();
            RunStatement("Color c <- Color.RED;", scope);
            Assert.IsInstanceOf<TPClassType>(scope.GetValue("c").Type);
            Assert.AreEqual(typeof(Color), ((TPClassType) scope.GetValue("c").Type).GetPObjectType());
        }

        [Test]
        public void AColorShouldBeAPaint()
        {
            Init();
            RunStatement("Paint c <- Color.RED;", scope);
            Assert.IsInstanceOf<TPClassType>(scope.GetValue("c").Type);
            Assert.AreEqual(typeof(Color),  ((TPClassType) scope.GetValue("c").Type).GetPObjectType());
        }

        [Test]
        public void AnImageSourceShouldBeCreated()
        {
            Init();
            RunStatement("ImageSource i <- new ImageSource(resource: \"Floor/blue_white\");", scope);
            Assert.IsInstanceOf<TPClassType>(scope.GetValue("i").Type);
            Assert.AreEqual(typeof(ImageSource), ((TPClassType) scope.GetValue("i").Type).GetPObjectType());
        }

        [Test]
        public void AnImageSourceShouldBeAPaint()
        {
            Init();
            RunStatement("Paint i <- new ImageSource(resource: \"Floor/blue_white\");", scope);
            Assert.IsInstanceOf<TPClassType>(scope.GetValue("i").Type);
            Assert.AreEqual(typeof(ImageSource), ((TPClassType) scope.GetValue("i").Type).GetPObjectType());
        }

        [Test]
        public void AnImageSourceShouldNotEqualAColor()
        {
            Init();
            RunStatement("Boolean b <- (Color.RED == new ImageSource(resource: \"Floor/blue_white\"));", scope);
            Assert.IsFalse(scope.GetValue("b").ToBoolean());
        }

        [Test]
        public void AColorShouldNotEqualAnImageSource()
        {
            Init();
            RunStatement("Boolean b <- (new ImageSource(resource: \"Floor/blue_white\") == Color.RED);", scope);
            Assert.IsFalse(scope.GetValue("b").ToBoolean());
        }

        [Test]
        public void AnImageSourceInAPaintVariableShouldCompareToAColor()
        {
            Init();
            RunStatement("Paint i <- new ImageSource(resource: \"Floor/blue_white\");", scope);
            RunStatement("Boolean b <- (i == Color.RED);", scope);
            Assert.AreEqual(typeof(ImageSource), ((TPClassType) scope.GetValue("i").Type).GetPObjectType());
        }

        [Test]
        public void AnColorInAPaintVariableShouldCompareToAColor()
        {
            Init();
            RunStatement("Paint i <- Color.RED);", scope);
            RunStatement("Boolean b <- (i == Color.RED);", scope);
            Assert.IsTrue(scope.GetValue("b").ToBoolean());
        }

        [Test]
        public void OrShouldShortCircuitOnLHSTrue()
        {
            Init();
            RunStatement("Boolean x <- true || y.isGood();", scope);
            Assert.IsTrue(scope.GetValue("x").ToBoolean());
        }

        [Test]
        public void AndShouldShortCircuitOnLHSFalse()
        {
            Init();
            RunStatement("Boolean x <- false && y.isGood();", scope);
            Assert.IsFalse(scope.GetValue("x").ToBoolean());
        }

    }
}
