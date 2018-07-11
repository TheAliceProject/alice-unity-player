using System;
using Alice.Tweedle;

namespace Alice.VM
{
	internal class SingleInputActionNotificationStep : ExecutionStep
	{
		TweedleValue initialValue;
		Action<TweedleValue> body;

		public SingleInputActionNotificationStep(string callStackEntry, TweedleFrame frame, Action<TweedleValue> body)
			: base(frame)
		{
			this.callStack = frame.StackWith(callStackEntry);
			this.body = body;
		}

		internal override void BlockerFinished(ExecutionStep notifyingStep)
		{
			base.BlockerFinished(notifyingStep);
			initialValue = notifyingStep.Result;
		}

		internal override void Execute()
		{
			body.Invoke(initialValue);
			base.Execute();
		}
	}
}