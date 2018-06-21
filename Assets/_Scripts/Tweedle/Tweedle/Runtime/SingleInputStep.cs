using System;
using Alice.Tweedle;

namespace Alice.VM
{
	internal class SingleInputStep : EvaluationStep
	{
		EvaluationStep blockingValue;
		Func<TweedleValue, TweedleValue> body;

		public SingleInputStep(string callStack, EvaluationStep blockingValue, Func<TweedleValue, TweedleValue> body)
			: base(blockingValue)
		{
			this.body = body;
			this.blockingValue = blockingValue;
			this.callStack = callStack;
		}

		internal override bool Execute()
		{
			result = body.Invoke(blockingValue.Result);
			return MarkCompleted();
		}
	}
}