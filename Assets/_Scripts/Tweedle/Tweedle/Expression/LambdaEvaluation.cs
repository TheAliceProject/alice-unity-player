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

		internal override ExecutionStep AsStep(ExecutionScope scope)
		{
			LambdaScope lambdaScope = scope.LambdaScope();
			var targetStep = target.AsStep(scope);
			var setTargetStep = new ValueOperationStep(
				"Set Target",
				lambdaScope,
				target => lambdaScope.lambda = (TweedleLambda)target);
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