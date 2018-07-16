using System;
using Alice.Tweedle;

namespace Alice.VM
{
	internal class TwoValueComputationStep : ExecutionStep
	{
		TweedleExpression exp1;
		TweedleExpression exp2;
		TweedleValue result1;
		TweedleValue result2;
		Func<TweedleValue, TweedleValue, TweedleValue> body;

		public TwoValueComputationStep(string callStackEntry,
															 ExecutionScope scope,
															 TweedleExpression exp1,
															 TweedleExpression exp2,
															 Func<TweedleValue, TweedleValue, TweedleValue> body)
			: base(scope)
		{
			this.callStack = scope.StackWith(callStackEntry);
			this.exp1 = exp1;
			this.exp2 = exp2;
			this.body = body;
		}

		void QueueExpressionStep(TweedleExpression exp, Action<TweedleValue> handler)
		{
			var evalStep = exp.AsStep(scope);
			var storeStep = new ValueOperationStep(callStack, scope, handler);
			evalStep.OnCompletionNotify(storeStep);
			storeStep.OnCompletionNotify(this);
			evalStep.Queue();
		}

		internal override void Execute()
		{
			if (result1 == null)
			{
				QueueExpressionStep(exp1, value => result1 = value);
				return;
			}
			if (result2 == null)
			{
				QueueExpressionStep(exp2, value => result2 = value);
				return;
			}
			result = body.Invoke(result1, result2);
			base.Execute();
		}
	}
}