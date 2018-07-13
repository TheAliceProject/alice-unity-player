using System;
using Alice.Tweedle;

namespace Alice.VM
{
	class ComputationStep : ExecutionStep
	{
		TweedleValue initialValue;
		Func<TweedleValue, TweedleValue> body;

		public ComputationStep(string callStackEntry, TweedleFrame frame, Func<TweedleValue, TweedleValue> body)
			: base(frame)
		{
			this.body = body;
			this.callStack = frame.StackWith(callStackEntry);
		}

		internal override void BlockerFinished(ExecutionStep blockingStep)
		{
			base.BlockerFinished(blockingStep);
			initialValue = blockingStep.Result;
		}

		internal override void Execute()
		{
			result = body.Invoke(initialValue);
			base.Execute();
		}
	}
}