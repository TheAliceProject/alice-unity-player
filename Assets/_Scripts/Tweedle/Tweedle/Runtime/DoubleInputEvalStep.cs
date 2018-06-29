using System;
using Alice.Tweedle;

namespace Alice.VM
{
	internal class DoubleInputEvalStep : NotifyingEvaluationStep
	{
		TweedleExpression exp1;
		TweedleExpression exp2;
		TweedleValue result1;
		TweedleValue result2;
		Func<TweedleValue, TweedleValue, TweedleValue> body;

		public DoubleInputEvalStep(string callStack,
								   TweedleFrame frame,
								   NotifyingStep parent,
								   TweedleExpression exp1,
								   TweedleExpression exp2,
								   Func<TweedleValue, TweedleValue, TweedleValue> body)
			: base(frame, parent)
		{
			this.callStack = callStack;
			this.exp1 = exp1;
			this.exp2 = exp2;
			this.body = body;
		}

		SingleInputActionNotificationStep HandleValueStep(Action<TweedleValue> handler)
		{
			return new SingleInputActionNotificationStep(callStack, frame, handler, this);
		}

		internal override void Execute()
		{
			if (result1 == null)
			{
				exp1.AddStep(HandleValueStep(value => result1 = value), frame);
				return;
			}
			if (result2 == null)
			{
				exp2.AddStep(HandleValueStep(value => result2 = value), frame);
				return;
			}
			result = body.Invoke(result1, result2);
			base.Execute();
		}
	}
}