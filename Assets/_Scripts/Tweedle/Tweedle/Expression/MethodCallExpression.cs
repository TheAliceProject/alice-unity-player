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

		internal override NotifyingEvaluationStep AsStep(NotifyingStep parent, TweedleFrame frame)
		{
			MethodFrame methodFrame = frame.MethodCallFrame();
			NotifyingStep prepMethodStep = new SingleInputActionNotificationStep(
				frame.StackWith("Invocation Prep"),
				frame,
				new ActionNotifyingStep("InvPrep", methodFrame, null, () => methodFrame.QueueInvocationStep(frame.StackWith("Invocation"), parent, arguments)),
				target =>
				{
					methodFrame.SetThis(target);
					methodFrame.Method = invokeSuper ? target.SuperMethodNamed(frame, MethodName) : target.MethodNamed(frame, MethodName);
					if (methodFrame.Method == null)//|| !method.ExpectsArgs(callExpression.arguments))
					{
						throw new TweedleRuntimeException("No method matching " + target + "." + MethodName + "()");
					}
					methodFrame.callStackEntry = MethodName;
				});

			return base.AsStep(prepMethodStep, frame);
		}
	}
}