using System;
using Alice.VM;

namespace Alice.Tweedle
{
	abstract class InvocationFrame : TweedleFrame
	{
		Action<TweedleValue> nextAction;

		internal InvocationFrame(VirtualMachine vm, Action<TweedleValue> next)
			: base(vm)
		{
			nextAction = next;
		}

		internal void Complete(TweedleValue result)
		{
			nextAction(result);
		}
	}
}
