using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
	public class MethodCallExpression : MemberAccessExpression
	{
		internal NamedArgument[] arguments;

		public string MethodName { get; }

		public MethodCallExpression(string methodName, NamedArgument[] arguments)
			: base(new ThisExpression())
		{
			Contract.Requires(arguments != null);
			MethodName = methodName;
			this.arguments = arguments;
		}

		public MethodCallExpression(ITweedleExpression target, string methodName, NamedArgument[] arguments)
			: base(target)
		{
			Contract.Requires(arguments != null);
			MethodName = methodName;
			this.arguments = arguments;
		}

		public static MethodCallExpression Super(string methodName, NamedArgument[] arguments)
		{
			Contract.Requires(arguments != null);
			MethodCallExpression callExpression = new MethodCallExpression(methodName, arguments);
			callExpression.invokeSuper = true;
			return callExpression;
		}

		public ITweedleExpression GetArg(string argName)
		{
            for (int i = 0; i < arguments.Length; ++i)
			{
				if (arguments[i].Name == argName)
                    return arguments[i].Argument;
            }
			return null;
        }

		public override ExecutionStep AsStep(ExecutionScope scope)
		{
			MethodScope methodScope = scope.MethodCallScope(MethodName, invokeSuper);

			var targetStep = TargetStep(scope);
			ExecutionStep prepMethodStep = new ValueOperationStep(
				"Invocation Prep",
				scope,
				methodScope.SetThis);
			targetStep.OnCompletionNotify(prepMethodStep);

			StepSequence main = new StepSequence(MethodName, scope);
			main.AddStep(targetStep);
			main.AddStep(new DelayedOperationStep(
				"Invocation",
				methodScope,
				() => methodScope.QueueInvocationStep(main, arguments)));
			return main;
		}
	}
}