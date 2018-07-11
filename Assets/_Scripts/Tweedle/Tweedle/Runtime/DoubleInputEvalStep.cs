using System;
using Alice.Tweedle;

namespace Alice.VM
{
	internal class DoubleInputEvalStep : ExecutionStep
	{
		TweedleExpression exp1;
		TweedleExpression exp2;
		TweedleValue result1;
		TweedleValue result2;
		Func<TweedleValue, TweedleValue, TweedleValue> body;

		public DoubleInputEvalStep(string callStackEntry,
								   TweedleFrame frame,
								   TweedleExpression exp1,
								   TweedleExpression exp2,
								   Func<TweedleValue, TweedleValue, TweedleValue> body)
			: base(frame)
		{
			this.callStack = callStackEntry;
			this.exp1 = exp1;
			this.exp2 = exp2;
			this.body = body;
		}

		void QueueExpressionStep(TweedleExpression exp, Action<TweedleValue> handler)
		{
			var evalStep = exp.AsStep(frame);
			var storeStep = new SingleInputActionNotificationStep(callStack, frame, handler);
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