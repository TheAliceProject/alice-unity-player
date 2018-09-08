using System;
using Alice.Tweedle;

namespace Alice.VM
{
	internal class TwoValueComputationStep : ExecutionStep
	{
		ITweedleExpression exp1;
		ITweedleExpression exp2;
		TValue result1;
		TValue result2;
		Func<TValue, TValue, TValue> body;

		public TwoValueComputationStep(string callStackEntry,
															 ExecutionScope scope,
															 ITweedleExpression exp1,
															 ITweedleExpression exp2,
															 Func<TValue, TValue, TValue> body)
			: base(scope)
		{
			this.callStack = scope.StackWith(callStackEntry);
			this.exp1 = exp1;
			this.exp2 = exp2;
			this.body = body;
		}

		void QueueExpressionStep(ITweedleExpression exp, Action<TValue> handler)
		{
			var evalStep = exp.AsStep(scope);
			var storeStep = new ValueOperationStep(callStack, scope, handler);
			evalStep.OnCompletionNotify(storeStep);
			storeStep.OnCompletionNotify(this);
			evalStep.Queue();
		}

		internal override void Execute()
		{
			if (result1 == TValue.UNDEFINED)
			{
				QueueExpressionStep(exp1, value => result1 = value);
				return;
			}
			if (result2 == TValue.UNDEFINED)
			{
				QueueExpressionStep(exp2, value => result2 = value);
				return;
			}
			result = body.Invoke(result1, result2);
			base.Execute();
		}
	}
}