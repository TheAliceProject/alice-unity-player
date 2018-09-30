using System.Collections.Generic;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
	public class LambdaEvaluation : TweedleExpression
	{
		ITweedleExpression target;
		ITweedleExpression[] arguments;

		public ITweedleExpression[] Arguments
		{
			get { return arguments; }
		}

		public LambdaEvaluation(ITweedleExpression target)
		{
			this.target = target;
			arguments = new ITweedleExpression[0];
		}

		public LambdaEvaluation(ITweedleExpression target, ITweedleExpression[] arguments)
		{
			this.target = target;
			this.arguments = arguments;
		}

		public override string ToTweedle()
		{
			// TODO improve expression
			return "lambda eval ()";
		}

		public override ExecutionStep AsStep(ExecutionScope scope)
		{
			LambdaScope lambdaScope = scope.LambdaScope();
			var targetStep = target.AsStep(scope);
			var setTargetStep = new ValueOperationStep(
				"Set Target",
				lambdaScope,
				target => lambdaScope.lambda = target.Lambda());
			targetStep.OnCompletionNotify(setTargetStep);

			StepSequence main = new StepSequence(ToTweedle(), scope);
			main.AddStep(targetStep);
			main.AddStep(new DelayedOperationStep(
				"Invocation",
				lambdaScope,
				() => lambdaScope.QueueInvocationStep(main, arguments)));
			return main;
		}
	}
}