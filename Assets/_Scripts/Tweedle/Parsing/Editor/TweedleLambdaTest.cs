using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Alice.Tweedle.Unlinked
{
	public class TweedleLambdaTest
	{
		private TweedleClass tested;

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
			tested = (TweedleClass)new TweedleUnlinkedParser().ParseType(sourceWithLambdas);
		}

		[Test]
		public void ClassWithMethodShouldHaveMethod()
		{
			Assert.False(tested.Methods.Count.Equals(0), "The class should have a method.");
		}

		[Test]
		public void MethodShouldBeNamed()
		{
			TweedleMethod initListeners = tested.Methods[0];
			Assert.AreEqual("initializeEventListeners", initListeners.Name, "The method should be named.");
		}

		[Test]
		public void MethodShouldHaveTwoStatements()
		{
			TweedleMethod initListeners = tested.Methods[0];
			Assert.AreEqual(2, initListeners.Body.Count, "The method should have two statements.");
		}

		[Test]
		public void MethodShouldHaveNonNullStatements()
		{
			TweedleMethod initListeners = tested.Methods[0];
			Assert.NotNull(initListeners.Body[0], "The first method statement should not be null.");
			Assert.NotNull(initListeners.Body[1], "The second method statement should not be null.");
		}

		[Test]
		public void MethodsFirstStatementShouldBeAnExpressionStatement()
		{
			TweedleMethod initListeners = tested.Methods[0];
			TweedleStatement statement = initListeners.Body[0];
			Assert.IsInstanceOf<ExpressionStatement>(statement, "The first method statement should be an ExpressionStatement.");
		}

		[Test]
		public void MethodsFirstExpressionStatementShouldHoldAMethodCall()
		{
			TweedleMethod initListeners = tested.Methods[0];
			ExpressionStatement statement = (ExpressionStatement)initListeners.Body[0];
			Assert.IsInstanceOf<MethodCallExpression>(statement.Expression, "The expression statement should hold a MethodCall.");
		}

		[Test]
		public void MethodsFirstMethodCallShouldBeAddSceneActivationListener()
		{
			TweedleMethod initListeners = tested.Methods[0];
			ExpressionStatement statement = (ExpressionStatement)initListeners.Body[0];
			MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
			Assert.AreEqual("addSceneActivationListener", methodCall.MethodName,
				"The first method statement should be calling addSceneActivationListener.");
		}

		[Test]
		public void AddSceneActivationListenerShouldHaveOneArg()
		{
			TweedleMethod initListeners = tested.Methods[0];
			ExpressionStatement statement = (ExpressionStatement)initListeners.Body[0];
			MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
			Assert.NotNull(methodCall.GetArg("listener"),
				"The addSceneActivationListener should have a listener arg.");
		}

		[Test]
		public void AddSceneActivationListenerOneArgShouldBeLambda()
		{
			TweedleMethod initListeners = tested.Methods[0];
			ExpressionStatement statement = (ExpressionStatement)initListeners.Body[0];
			MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
			Assert.IsInstanceOf<LambdaExpression>(methodCall.GetArg("listener"),
				"The addSceneActivationListener should have a listener arg.");
		}

		[Test]
		public void AddSceneActivationListenerLambdaShouldHaveOneParameter()
		{
			TweedleMethod initListeners = tested.Methods[0];
			ExpressionStatement statement = (ExpressionStatement)initListeners.Body[0];
			MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
			LambdaExpression listener = (LambdaExpression)methodCall.GetArg("listener");
			Assert.AreEqual(1, listener.Parameters.Count,
				"The lambda listener should have a parameter.");
		}

		[Test]
		public void AddSceneActivationListenerLambdaParameterShouldHaveOneParameterNamedEvent()
		{
			TweedleMethod initListeners = tested.Methods[0];
			ExpressionStatement statement = (ExpressionStatement)initListeners.Body[0];
			MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
			LambdaExpression listener = (LambdaExpression)methodCall.GetArg("listener");
			TweedleRequiredParameter parameter = listener.Parameters[0];
			Assert.AreEqual("event", parameter.Name,
				"The lambda listener should have a parameter.");
		}

		[Test]
		public void AddSceneActivationListenerLambdaParameterShouldHaveOneParameterTypedReference()
		{
			TweedleMethod initListeners = tested.Methods[0];
			ExpressionStatement statement = (ExpressionStatement)initListeners.Body[0];
			MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
			LambdaExpression listener = (LambdaExpression)methodCall.GetArg("listener");
			TweedleRequiredParameter parameter = listener.Parameters[0];
			Assert.IsInstanceOf<TweedleTypeReference>(parameter.Type,
				"The lambda listener should have a parameter.");
		}

		[Test]
		public void AddSceneActivationListenerLambdaParameterShouldHaveOneParameterTypedSceneActivationEvent()
		{
			TweedleMethod initListeners = tested.Methods[0];
			ExpressionStatement statement = (ExpressionStatement)initListeners.Body[0];
			MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
			LambdaExpression listener = (LambdaExpression)methodCall.GetArg("listener");
			TweedleRequiredParameter parameter = listener.Parameters[0];
			TweedleTypeReference type = (TweedleTypeReference)parameter.Type;
			Assert.AreEqual("SceneActivationEvent", type.Name,
				"The lambda listener should have parameter typed SceneActivationEvent.");
		}

		[Test]
		public void AddSceneActivationListenerSecondLambdaParameterShouldHaveOneParameterTypedArrowKeyEvent()
		{
			TweedleMethod initListeners = tested.Methods[0];
			ExpressionStatement statement = (ExpressionStatement)initListeners.Body[1];
			MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
			LambdaExpression listener = (LambdaExpression)methodCall.GetArg("listener");
			TweedleRequiredParameter parameter = listener.Parameters[0];
			TweedleTypeReference  type = (TweedleTypeReference)parameter.Type;
			Assert.AreEqual("ArrowKeyEvent", type.Name,
				"The lambda listener should have parameter typed ArrowKeyEvent.");
		}

		[Test]
		public void AddSceneActivationListenerLambdaParameterShouldHaveOneStatement()
		{
			TweedleMethod initListeners = tested.Methods[0];
			ExpressionStatement statement = (ExpressionStatement)initListeners.Body[0];
			MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
			LambdaExpression listener = (LambdaExpression)methodCall.GetArg("listener");
			Assert.AreEqual(1, listener.Statements.Count,
				"The lambda listener should have parameter typed SceneActivationEvent.");
		}

		[Test]
		public void AddSceneActivationListenerLambdaParameterShouldHaveOneNonNullStatement()
		{
			TweedleMethod initListeners = tested.Methods[0];
			ExpressionStatement statement = (ExpressionStatement)initListeners.Body[0];
			MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
			LambdaExpression listener = (LambdaExpression)methodCall.GetArg("listener");
			Assert.NotNull(listener.Statements[0],
				"The lambda listener should have parameter typed SceneActivationEvent.");
		}

		[Test]
		public void AddSceneActivationListenerLambdaParameterShouldHaveOneExpressionStatement()
		{
			TweedleMethod initListeners = tested.Methods[0];
			ExpressionStatement statement = (ExpressionStatement)initListeners.Body[0];
			MethodCallExpression methodCall = (MethodCallExpression)statement.Expression;
			LambdaExpression listener = (LambdaExpression)methodCall.GetArg("listener");
			Assert.IsInstanceOf<ExpressionStatement>(listener.Statements[0],
				"The lambda listener should have parameter typed SceneActivationEvent.");
		}
	}
}