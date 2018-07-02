using System;
using Alice.Tweedle;

namespace Alice.VM
{
	class ActionNotifyingStep : NotifyingStep
	{
		Action body;

		public ActionNotifyingStep(string callStack, TweedleFrame frame, Action body)
			: base(frame)
		{
			this.body = body;
			this.callStack = callStack;
		}

		internal override void Execute()
		{
			body.Invoke();
			base.Execute();
		}
	}
}