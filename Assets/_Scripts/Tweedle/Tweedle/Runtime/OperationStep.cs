using System;
using Alice.Tweedle;

namespace Alice.VM
{
	internal class OperationStep : ExecutionStep
	{
		TweedleValue initialValue;
		Action<TweedleValue> body;

		public OperationStep(string callStackEntry, ExecutionScope scope, Action<TweedleValue> body)
			: base(scope)
		{
			this.callStack = scope.StackWith(callStackEntry);
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