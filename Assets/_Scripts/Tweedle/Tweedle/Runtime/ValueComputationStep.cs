using System;
using Alice.Tweedle;

namespace Alice.VM
{
	class ValueComputationStep : ExecutionStep
	{
		TweedleValue initialValue;
		Func<TweedleValue, TweedleValue> body;

		public ValueComputationStep(string callStackEntry, ExecutionScope scope, Func<TweedleValue, TweedleValue> body)
			: base(scope)
		{
			this.body = body;
			this.callStack = scope.StackWith(callStackEntry);
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