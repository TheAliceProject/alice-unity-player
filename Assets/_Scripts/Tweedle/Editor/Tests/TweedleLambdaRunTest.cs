using NUnit.Framework;
using Alice.Tweedle.VM;
using System.Text.RegularExpressions;
using UnityEngine.TestTools;

namespace Alice.Tweedle.Parse
{
    public class TweedleLambdaRunTest
    {
        static TweedleParser parser = new TweedleParser();

        private static IStackFrame testStackFrame = new StaticStackFrame("Test");

        TClassType lambdaTest;
        TestVirtualMachine vm;
        ExecutionScope scope;

        [SetUp]
        public void Setup()
        {
            string sourceWithLambdas =
                "class LambdaTest {\n" +
                "  LambdaTest() {}\n" +
                "  WholeNumber receiveOneArgLambda(<WholeNumber->WholeNumber> lambda) {\n" +
                "      return 5;\n" +
                "  }\n" +
                "  WholeNumber sendLambda() {\n" +
                "    return this.receiveOneArgLambda(lambda: (WholeNumber value)-> {\n" +
                "      return value;\n" +
                "    });\n" +
                "  }\n" +
                "  WholeNumber evaluateOneArgLambda(<WholeNumber->WholeNumber> lambda) {\n" +
                "      return lambda(5);\n" +
                "  }\n" +
                "  WholeNumber sendLambdaToEval() {\n" +
                "    return this.evaluateOneArgLambda(lambda: (WholeNumber value)-> {\n" +
                "      return value + 7;\n" +
                "    });\n" +
                "  }\n" +
                "  WholeNumber evaluateTwoArgLambda(<(WholeNumber,WholeNumber)->WholeNumber> lambda) {\n" +
                "      return lambda(5, 5);\n" +
                "  }\n" +
                "  WholeNumber sendSumLambda() {\n" +
                "    return this.evaluateTwoArgLambda(lambda: (WholeNumber l, WholeNumber r)-> {\n" +
                "      return l + r;\n" +
                "    });\n" +
                "  }\n" +
                "  WholeNumber sendMultLambda() {\n" +
                "    return this.evaluateTwoArgLambda(lambda: (WholeNumber l, WholeNumber r)-> {\n" +
                "      return l * r;\n" +
                "    });\n" +
                "  }\n" +
                "  WholeNumber sendSingleLambdaToDoubleEval() {\n" +
                "    return this.evaluateTwoArgLambda(lambda: (WholeNumber value)-> {\n" +
                "      return value + 7;\n" +
                "    });\n" +
                "  }\n" +
                "}";
            var system = new TweedleSystem();
            lambdaTest = (TClassType)parser.ParseType(sourceWithLambdas, system.GetRuntimeAssembly());
            system.GetRuntimeAssembly().Add(lambdaTest);
            vm = new TestVirtualMachine(system);
            scope = new ExecutionScope(testStackFrame, vm);
            system.Link();
            ExecuteStatement("LambdaTest lam <- new LambdaTest();");
        }

        void ExecuteStatement(string src)
        {
            TestVirtualMachine.ExecuteToFinish(parser.ParseStatement(src), scope);
        }

        [Test]
        public void ClassShouldHaveMethods()
        {
            Assert.False(lambdaTest.Methods(null).Length.Equals(0), "The class should have methods.");
        }

        [Test]
        public void LambdaShouldBePassed()
        {
            ExecuteStatement("WholeNumber x <- lam.sendLambda();");
            TValue tested = scope.GetValue("x");

            Assert.AreEqual(5, tested.ToInt());
        }

        [Test]
        public void LambdaShouldBeEvaluated()
        {
            ExecuteStatement("WholeNumber x <- lam.sendLambdaToEval();");
            TValue tested = scope.GetValue("x");

            Assert.AreEqual(12, tested.ToInt());
        }

        [Test]
        public void SumLambdaShouldBeEvaluated()
        {
            ExecuteStatement("WholeNumber x <- lam.sendSumLambda();");
            TValue tested = scope.GetValue("x");

            Assert.AreEqual(10, tested.ToInt());
        }

        [Test]
        public void MultLambdaShouldBeEvaluated()
        {
            ExecuteStatement("WholeNumber x <- lam.sendMultLambda();");
            TValue tested = scope.GetValue("x");

            Assert.AreEqual(25, tested.ToInt());
        }

        [Test]
        public void SingleParamLambdaShouldBeRejectedAsMultiParamLambda()
        {
            LogAssert.Expect(UnityEngine.LogType.Error, new Regex("Unable to treat value Alice.Tweedle.TLambda of type <WholeNumber->Number> as type <WholeNumber,WholeNumber->WholeNumber>"));
            Assert.Throws<TweedleRuntimeException>(
                () => ExecuteStatement("WholeNumber x <- lam.sendSingleLambdaToDoubleEval();"));
        }
    }
}
