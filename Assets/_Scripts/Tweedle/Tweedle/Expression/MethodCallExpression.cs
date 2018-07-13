using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Alice.VM;

namespace Alice.Tweedle
{
	public class MethodCallExpression : MemberAccessExpression
	{
		internal Dictionary<string, TweedleExpression> arguments;

		public string MethodName { get; }

		public MethodCallExpression(string methodName, Dictionary<string, TweedleExpression> arguments)
			: base(new ThisExpression())
		{
			Contract.Requires(arguments != null);
			MethodName = methodName;
			this.arguments = arguments;
		}

		public MethodCallExpression(TweedleExpression target, string methodName, Dictionary<string, TweedleExpression> arguments)
			: base(target)
		{
			Contract.Requires(arguments != null);
			MethodName = methodName;
			this.arguments = arguments;
		}

		public static MethodCallExpression Super(string methodName, Dictionary<string, TweedleExpression> arguments)
		{
			Contract.Requires(arguments != null);
			MethodCallExpression callExpression = new MethodCallExpression(methodName, arguments);
			callExpression.invokeSuper = true;
			return callExpression;
		}

		public TweedleExpression GetArg(string argName)
		{
			return arguments[argName];
		}

		internal override ExecutionStep AsStep(ExecutionScope scope)
		{
			MethodScope methodScope = scope.MethodCallScope(MethodName, invokeSuper);

			var targetStep = TargetStep(scope);
			ExecutionStep prepMethodStep = new OperationStep(
				"Invocation Prep",
				scope,
				methodScope.SetThis);
			targetStep.OnCompletionNotify(prepMethodStep);

			StepSequence main = new StepSequence(MethodName, scope);
			main.AddStep(targetStep);
			main.AddStep(new ActionNotifyingStep(
				"Invocation",
				methodScope,
				() => methodScope.QueueInvocationStep(main, arguments)));
			return main;
		}
	}
}