using System;
using Alice.VM;

namespace Alice.Tweedle
{
	internal class MethodFrame : InvocationFrame
	{
		internal MethodFrame(VirtualMachine vm, TweedleValue target, Action<TweedleValue> next)
			: base(vm, next)
		{
			// target may be an instance (object or enumValue) or type (class or enum)
			thisValue = target;
		}
	}
}