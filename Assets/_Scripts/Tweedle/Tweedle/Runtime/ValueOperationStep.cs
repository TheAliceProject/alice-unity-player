using System;
using Alice.Tweedle;

namespace Alice.VM
{
	public class ValueOperationStep : ExecutionStep
	{
		TValue initialValue;
		Action<TValue> body;

		public ValueOperationStep(string callStackEntry, ExecutionScope scope, Action<TValue> body)
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