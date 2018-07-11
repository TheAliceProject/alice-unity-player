using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class LambdaEvaluation : TweedleExpression
	{
		TweedleExpression target;
		List<TweedleExpression> arguments;

		public List<TweedleExpression> Arguments
		{
			get { return arguments; }
		}

		public LambdaEvaluation(TweedleExpression target)
		{
			this.target = target;
			arguments = new List<TweedleExpression>();
		}

		public LambdaEvaluation(TweedleExpression target, List<TweedleExpression> arguments)
		{
			this.target = target;
			this.arguments = arguments;
		}

		internal override string ToTweedle()
		{
			// TODO improve expression
			return "lambda eval ()";
		}

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			LambdaFrame lambdaFrame = frame.LambdaFrame();
			var targetStep = target.AsStep(frame);
			var setTargetStep = new SingleInputActionNotificationStep(
				"Set Target",
				lambdaFrame,
				target => lambdaFrame.lambda = (TweedleLambda)target);
			targetStep.OnCompletionNotify(setTargetStep);

			StepSequence main = new StepSequence(ToTweedle(), frame);
			main.AddStep(targetStep);
			main.AddStep(new ActionNotifyingStep(
				"Invocation",
				lambdaFrame,
				() => lambdaFrame.QueueInvocationStep(main, arguments)));
			return main;
		}
	}
}