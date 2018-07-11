using System;
using Alice.Tweedle;

namespace Alice.VM
{
	class SingleInputNotificationStep : ExecutionStep
	{
		TweedleValue initialValue;
		Func<TweedleValue, TweedleValue> body;

		public SingleInputNotificationStep(string callStackEntry, TweedleFrame frame, Func<TweedleValue, TweedleValue> body)
			: base(frame)
		{
			this.body = body;
			this.callStack = frame.StackWith(callStackEntry);
		}

		internal override void BlockerFinished(ExecutionStep notifyingStep)
		{
			base.BlockerFinished(notifyingStep);
			initialValue = notifyingStep.Result;
		}

		internal override void Execute()
		{
			result = body.Invoke(initialValue);
			base.Execute();
		}
	}
}