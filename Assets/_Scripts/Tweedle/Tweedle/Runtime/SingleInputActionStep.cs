using System;
using Alice.Tweedle;

namespace Alice.VM
{
	internal class SingleInputActionStep : ExecutionStep
	{
		EvaluationStep blockingValue;
		Action<TweedleValue> body;

		public SingleInputActionStep(string callStack, EvaluationStep blockingValue, Action<TweedleValue> body)
			: base(blockingValue)
		{
			this.callStack = callStack;
			this.body = body;
			this.blockingValue = blockingValue;
		}

		internal override bool Execute()
		{
			body.Invoke(blockingValue.Result);
			return MarkCompleted();
		}
	}
}