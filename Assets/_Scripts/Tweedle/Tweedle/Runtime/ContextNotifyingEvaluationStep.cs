using System;
using Alice.Tweedle;

namespace Alice.VM
{
	class ContextNotifyingEvaluationStep : ExecutionStep
	{
		Func<TweedleValue> body;

		public ContextNotifyingEvaluationStep(string callStack, TweedleFrame frame, Func<TweedleValue> body)
			: base(frame)
		{
			this.body = body;
			this.callStack = callStack;
		}

		internal override void Execute()
		{
			result = body.Invoke();
			base.Execute();
		}
	}
}