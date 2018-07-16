using System;
using Alice.Tweedle;

namespace Alice.VM
{
	class ValueGenerationStep : ExecutionStep
	{
		Func<TweedleValue> body;

		public ValueGenerationStep(string callStack, ExecutionScope scope, Func<TweedleValue> body)
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