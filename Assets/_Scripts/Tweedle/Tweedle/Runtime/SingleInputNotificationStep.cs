using System;
using Alice.Tweedle;

namespace Alice.VM
{
	class SingleInputNotificationStep : NotifyingEvaluationStep
	{
		TweedleValue initialValue;
		Func<TweedleValue, TweedleValue> body;

		public SingleInputNotificationStep(string callStack, TweedleFrame frame, Func<TweedleValue, TweedleValue> body, NotifyingStep next)
			: base(frame, next)
		{
			this.body = body;
			this.callStack = callStack;
		}

		public SingleInputNotificationStep(Func<TweedleValue, TweedleValue> body, NotifyingStep next)
			: base(next.frame, next)
		{
			this.body = body;
			this.callStack = next.callStack;
		}

		internal override void BlockerFinished(NotifyingStep notifyingStep)
		{
			base.BlockerFinished(notifyingStep);
			initialValue = ((NotifyingEvaluationStep)notifyingStep).Result;
		}

		internal override void Execute()
		{
			result = body.Invoke(initialValue);
			base.Execute();
		}
	}
}