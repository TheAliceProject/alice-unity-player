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

		internal override NotifyingEvaluationStep AsStep(TweedleFrame frame)
		{
			MethodFrame methodFrame = frame.MethodCallFrame(MethodName, invokeSuper);

			var targetStep = TargetStep(frame);
			NotifyingStep prepMethodStep = new SingleInputActionNotificationStep(
				"Invocation Prep",
				frame,
				methodFrame.SetThis);
			targetStep.OnCompletionNotify(prepMethodStep);

			SequentialStepsEvaluation main = new SequentialStepsEvaluation(MethodName, frame);
			main.AddStep(targetStep);
			main.AddStep(new ActionNotifyingStep(
				"Invocation",
				methodFrame,
				() => methodFrame.QueueInvocationStep(main, arguments)));
			return main;
		}
	}
}