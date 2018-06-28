using System;
using Alice.Tweedle;

namespace Alice.VM
{
	internal class SingleInputActionNotificationStep : NotifyingStep
	{
		TweedleValue initialValue;
		Action<TweedleValue> body;

		public SingleInputActionNotificationStep(string callStack, TweedleFrame frame, NotifyingStep next, Action<TweedleValue> body)
			: base(frame, next)
		{
			this.callStack = callStack;
			this.body = body;
		}

		internal override void BlockerFinished(NotifyingStep notifyingStep)
		{
			base.BlockerFinished(notifyingStep);
			initialValue = ((NotifyingEvaluationStep)notifyingStep).Result;
		}

		internal override void Execute()
		{
			body.Invoke(initialValue);
			base.Execute();
		}
	}
}