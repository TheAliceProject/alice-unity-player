using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Alice.VM;

namespace Alice.Tweedle
{
	public class MethodCallExpression : MemberAccessExpression
	{
		internal Dictionary<string, TweedleExpression> arguments;

		public string MethodName { get; }

		public MethodCallExpression(TweedleExpression target, string methodName, Dictionary<string, TweedleExpression> arguments)
			: base(target)
		{
			Contract.Requires(arguments != null);
			MethodName = methodName;
			this.arguments = arguments;
		}

		public TweedleExpression GetArg(string argName)
		{
			return arguments[argName];
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			MethodFrame methodFrame = frame.MethodCallFrame();
			EvaluationStep targetStep = base.AsStep(frame);
			ExecutionStep prepMethodStep = new SingleInputActionStep(
				targetStep,
				target =>
				{
					methodFrame.SetThis(target);
					methodFrame.Method = target.MethodNamed(frame, MethodName);
					if (methodFrame.Method == null)//|| !method.ExpectsArgs(callExpression.arguments))
					{
						throw new TweedleRuntimeException("No method matching " + target + "." + MethodName + "()");
					}
					methodFrame.callStackEntry = MethodName;
					methodFrame.CreateArgumentSteps(arguments);
				});
			return new StartStep(prepMethodStep, () => methodFrame.InvokeStep());
		}
	}
}