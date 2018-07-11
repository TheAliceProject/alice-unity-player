using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class TweedleLambda : TweedleValue
	{
		readonly LambdaExpression source;

		public TweedleLambda(LambdaExpression expression)
			: base(expression.LambdaType())
		{
			source = expression;
		}

		internal void AddInvocationSteps(LambdaFrame frame, StepSequence main, List<TweedleExpression> arguments)
		{
			AddArgumentSteps(frame, main, arguments);
			main.AddStep(source.Body.AsSequentialStep(frame));
			main.AddStep(ResultStep(frame));
		}

		void AddArgumentSteps(LambdaFrame frame, StepSequence main, List<TweedleExpression> arguments)
		{
			for (int i = 0; i < arguments.Count; i++)
			{
				TweedleRequiredParameter param = source.Parameters[i];
				TweedleExpression argExp = arguments[i];
				ExecutionStep argStep = argExp.AsStep(frame.callingFrame);
				var storeStep = new SingleInputNotificationStep(
					"Arg",
					frame.callingFrame,
					argVal => frame.SetLocalValue(param, argVal));
				argStep.OnCompletionNotify(storeStep);
				main.AddStep(argStep);
			}
		}

		ExecutionStep ResultStep(LambdaFrame frame)
		{
			return new SingleInputNotificationStep("call", frame, arg => frame.Result);
		}
	}
}