using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Alice.Tweedle.Parse
{
    public class TweedleLambdaTest
    {
        private TClassType tested;

        [SetUp]
        public void Setup()
        {
            string sourceWithLambdas =
                        "class Scene extends SScene {\n  Scene() {\n    super();\n  }\n\n  void initializeEventListeners() {\n"
                                        + "    this.addSceneActivationListener(listener: (SceneActivationEvent event)-> {\n"
                                        + "      this.myFirstMethod();\n    });\n"
                                        + "    this.addArrowKeyPressListener(listener: (ArrowKeyEvent event)-> {\n"
                                        + "      this.camera.move(direction: event.getMoveDirection(movedirectionplane: MoveDirectionPlane.FORWARD_BACKWARD_LEFT_RIGHT), amount: 0.25);\n"
                                        + "    });\n  } \n }";
            var system = new TweedleSystem();
            tested = (TClassType)new TweedleParser().ParseType(sourceWithLambdas, system.GetRuntimeAssembly());
        }

        [Test]
        public void ClassWithMethodShouldHaveMethod()
        {
            Assert.False(tested.Methods(null).Length.Equals(0), "The class should have a method.");
        }

        [Test]
        public void MethodShouldBeNamed()
        {
            TTMethod initListeners = (TTMethod)tested.Methods(null)[0];
            Assert.AreEqual("initializeEventListeners", initListeners.Name, "The method should be named.");
        }

        [Test]
        public void MethodShouldHaveTwoStatements()
        {
            TTMethod initListeners = (TTMethod)tested.Methods(null)[0];
            Assert.AreEqual(2, initListeners.Body.Statements.Length, "The method should have two statements.");
        }

        [Test]
        public void MethodShouldHaveNonNullStatements()
        {
            TTMethod initListeners = (TTMethod)tested.Methods(null)[0];
            Assert.NotNull(initListeners.Body.Statements[0], "The first method statement should not be null.");
            Assert.NotNull(initListeners.Body.Statements[1], "The second method statement should not be null.");
        }

        [Test]
        public void MethodsFirstStatementShouldBeAnExpressionStatement()
        {
            TTMethod initListeners = (TTMethod)tested.Methods(null)[0];
            TweedleStatement statement = initListeners.Body.Statements[0];
            Assert.IsInstanceOf<ExpressionStatement>(statement, "The first method statement should be an ExpressionStatement.");
        }

        [Test]
        public void MethodsFirstExpressionStatementShouldHoldAMethodCall()
        {
            TTMethod initListeners = (TTMethod)tested.Methods(null)[0];
            ExpressionStatement statement = (ExpressionStatement)initListeners.Body.Statements[0];
            Assert.IsInstanceOf<MethodCallExpression>(statement.Expression, "The expression statement should hold a MethodCall.");
        }

        [Test]
        public void MethodsFirstMethodCallShouldBeAddSceneActivationListener()
        {
            TTMethod initListeners = (TTMethod)tested.Methods(null)[0];
            ExpressionStatement statement = (ExpressionStatement)initListeners.Body.Statements[0];
            MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
            Assert.AreEqual("addSceneActivationListener", methodCall.MethodName,
                "The first method statement should be calling addSceneActivationListener.");
        }

        [Test]
        public void AddSceneActivationListenerShouldHaveOneArg()
        {
            TTMethod initListeners = (TTMethod)tested.Methods(null)[0];
            ExpressionStatement statement = (ExpressionStatement)initListeners.Body.Statements[0];
            MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
            Assert.NotNull(methodCall.GetArg("listener"),
                "The addSceneActivationListener should have a listener arg.");
        }

        [Test]
        public void AddSceneActivationListenerOneArgShouldBeLambda()
        {
            TTMethod initListeners = (TTMethod)tested.Methods(null)[0];
            ExpressionStatement statement = (ExpressionStatement)initListeners.Body.Statements[0];
            MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
            Assert.IsInstanceOf<LambdaExpression>(methodCall.GetArg("listener"),
                "The addSceneActivationListener should have a listener arg.");
        }

        [Test]
        public void AddSceneActivationListenerLambdaShouldHaveOneParameter()
        {
            TTMethod initListeners = (TTMethod)tested.Methods(null)[0];
            ExpressionStatement statement = (ExpressionStatement)initListeners.Body.Statements[0];
            MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
            LambdaExpression listener = (LambdaExpression)methodCall.GetArg("listener");
            Assert.AreEqual(1, listener.Parameters.Length,
                "The lambda listener should have a parameter.");
        }

        [Test]
        public void AddSceneActivationListenerLambdaParameterShouldHaveOneParameterNamedEvent()
        {
            TTMethod initListeners = (TTMethod)tested.Methods(null)[0];
            ExpressionStatement statement = (ExpressionStatement)initListeners.Body.Statements[0];
            MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
            LambdaExpression listener = (LambdaExpression)methodCall.GetArg("listener");
            TParameter parameter = listener.Parameters[0];
            Assert.AreEqual("event", parameter.Name,
                "The lambda listener should have a parameter.");
        }

        [Test]
        public void AddSceneActivationListenerLambdaParameterShouldHaveOneParameterTypedReference()
        {
            TTMethod initListeners = (TTMethod)tested.Methods(null)[0];
            ExpressionStatement statement = (ExpressionStatement)initListeners.Body.Statements[0];
            MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
            LambdaExpression listener = (LambdaExpression)methodCall.GetArg("listener");
            TParameter parameter = listener.Parameters[0];
            Assert.IsInstanceOf<TTypeRef>(parameter.Type,
                "The lambda listener should have a parameter.");
        }

        [Test]
        public void AddSceneActivationListenerLambdaParameterShouldHaveOneParameterTypedSceneActivationEvent()
        {
            TTMethod initListeners = (TTMethod)tested.Methods(null)[0];
            ExpressionStatement statement = (ExpressionStatement)initListeners.Body.Statements[0];
            MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
            LambdaExpression listener = (LambdaExpression)methodCall.GetArg("listener");
            TParameter parameter = listener.Parameters[0];
            TTypeRef type = parameter.Type;
            Assert.AreEqual("SceneActivationEvent", type.Name,
                "The lambda listener should have parameter typed SceneActivationEvent.");
        }

        [Test]
        public void AddSceneActivationListenerSecondLambdaParameterShouldHaveOneParameterTypedArrowKeyEvent()
        {
            TTMethod initListeners = (TTMethod)tested.Methods(null)[0];
            ExpressionStatement statement = (ExpressionStatement)initListeners.Body.Statements[1];
            MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
            LambdaExpression listener = (LambdaExpression)methodCall.GetArg("listener");
            TParameter parameter = listener.Parameters[0];
            TTypeRef type = parameter.Type;
            Assert.AreEqual("ArrowKeyEvent", type.Name,
                "The lambda listener should have parameter typed ArrowKeyEvent.");
        }

        [Test]
        public void AddSceneActivationListenerLambdaParameterShouldHaveOneStatement()
        {
            TTMethod initListeners = (TTMethod)tested.Methods(null)[0];
            ExpressionStatement statement = (ExpressionStatement)initListeners.Body.Statements[0];
            MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
            LambdaExpression listener = (LambdaExpression)methodCall.GetArg("listener");
            Assert.AreEqual(1, listener.Body.Statements.Length,
                "The lambda listener should have parameter typed SceneActivationEvent.");
        }

        [Test]
        public void AddSceneActivationListenerLambdaParameterShouldHaveOneNonNullStatement()
        {
            TTMethod initListeners = (TTMethod)tested.Methods(null)[0];
            ExpressionStatement statement = (ExpressionStatement)initListeners.Body.Statements[0];
            MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
            LambdaExpression listener = (LambdaExpression)methodCall.GetArg("listener");
            Assert.NotNull(listener.Body.Statements[0],
                "The lambda listener should have parameter typed SceneActivationEvent.");
        }

        [Test]
        public void AddSceneActivationListenerLambdaParameterShouldHaveOneExpressionStatement()
        {
            TTMethod initListeners = (TTMethod)tested.Methods(null)[0];
            ExpressionStatement statement = (ExpressionStatement)initListeners.Body.Statements[0];
            MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
            LambdaExpression listener = (LambdaExpression)methodCall.GetArg("listener");
            Assert.IsInstanceOf<ExpressionStatement>(listener.Body.Statements[0],
                "The lambda listener should have parameter typed SceneActivationEvent.");
        }
    }
}
