using System;
using Alice.Tweedle;

namespace Alice.VM
{
	class ActionNotifyingStep : ExecutionStep
	{
		Action body;

		public ActionNotifyingStep(string callStackEntry, ExecutionScope scope, Action body)
			: base(scope)
		{
			this.callStack = scope.StackWith(callStackEntry);
			this.body = body;
		}

		internal override void Execute()
		{
			body.Invoke();
			base.Execute();
		}
	}
}