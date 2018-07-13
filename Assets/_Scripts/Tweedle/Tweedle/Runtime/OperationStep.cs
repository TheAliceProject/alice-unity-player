using System;
using Alice.Tweedle;

namespace Alice.VM
{
	internal class OperationStep : ExecutionStep
	{
		TweedleValue initialValue;
		Action<TweedleValue> body;

		public OperationStep(string callStackEntry, TweedleFrame frame, Action<TweedleValue> body)
			: base(frame)
		{
			this.callStack = frame.StackWith(callStackEntry);
			this.body = body;
		}

		internal override void BlockerFinished(ExecutionStep blockingStep)
		{
			base.BlockerFinished(blockingStep);
			initialValue = blockingStep.Result;
		}

		internal override void Execute()
		{
			body.Invoke(initialValue);
			base.Execute();
		}
	}
}