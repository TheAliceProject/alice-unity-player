﻿using System;
using Alice.Tweedle;

namespace Alice.VM
{
	class ContextNotifyingEvaluationStep : NotifyingEvaluationStep
	{
		Func<TweedleValue> body;

		public ContextNotifyingEvaluationStep(string callStack, TweedleFrame frame, NotifyingStep parent, Func<TweedleValue> body)
			: base(frame, parent)
		{
			this.body = body;
			this.callStack = callStack;
		}

		internal override void Execute()
		{
			result = body.Invoke();
			MarkCompleted();
		}
	}
}