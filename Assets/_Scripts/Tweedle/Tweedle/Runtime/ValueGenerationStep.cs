using System;
using Alice.Tweedle;

namespace Alice.Tweedle.VM
{
	class ValueGenerationStep : ExecutionStep
	{
		Func<TValue> body;

		public ValueGenerationStep(string callStack, ExecutionScope scope, Func<TValue> body)
			: base(scope)
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