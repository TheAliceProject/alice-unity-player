using System;
using Alice.Tweedle;

namespace Alice.VM
{
	class ActionNotifyingStep : ExecutionStep
	{
		Action body;

		public ActionNotifyingStep(string callStackEntry, TweedleFrame frame, Action body)
			: base(frame)
		{
			this.callStack = frame.StackWith(callStackEntry);
			this.body = body;
		}

		internal override void Execute()
		{
			body.Invoke();
			base.Execute();
		}
	}
}