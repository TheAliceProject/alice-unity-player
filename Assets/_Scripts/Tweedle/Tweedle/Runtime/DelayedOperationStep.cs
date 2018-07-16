using System;
using Alice.Tweedle;

namespace Alice.VM
{
	// A step to execute an action only after all preceeding steps are complete.
	// Used to invoke methods, constructors, and lambdas.
	class DelayedOperationStep : ExecutionStep
	{
		Action body;

		public DelayedOperationStep(string callStackEntry, ExecutionScope scope, Action body)
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