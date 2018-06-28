using System;
using Alice.Tweedle;

namespace Alice.VM
{
	class ActionNotifyingStep : NotifyingStep
	{
		Action body;

		public ActionNotifyingStep(string callStack, TweedleFrame frame, NotifyingStep next, Action body)
			: base(frame, next)
		{
			this.body = body;
			this.callStack = callStack;
		}

		public ActionNotifyingStep(NotifyingStep next, Action body)
			: base(next.frame, next)
		{
			this.body = body;
			this.callStack = next.callStack;
		}

		internal override void Execute()
		{
			body.Invoke();
			base.Execute();
		}
	}
}