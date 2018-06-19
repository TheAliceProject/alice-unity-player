using System;
using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	class ConstructorFrame : InvocationFrame
	{
		internal TweedleClass highestClass;

		internal ConstructorFrame(VirtualMachine vm, TweedleClass tweedleClass, Action<TweedleValue> next)
			: base(vm, next)
		{
			highestClass = tweedleClass;
			thisValue = new TweedleObject(tweedleClass);
		}

		internal void Instantiate(Dictionary<string, TweedleExpression> arguments)
		{
			// TODO restore as ExecutionStep
			//highestClass.ConstructorWithArgs(arguments).Invoke(this, arguments);
		}

		internal void SuperInstantiate(Dictionary<string, TweedleExpression> arguments)
		{
			TweedleConstructor superConst = NextSuperConstructor(arguments);
			if (superConst != null)
			{
				// TODO restore as ExecutionStep
				//superConst.Invoke(this, arguments);
			}
			else
			{
				// TODO test and fix. There may be more steps after call to super()
				Complete(thisValue);
			}
		}

		TweedleConstructor NextSuperConstructor(Dictionary<string, TweedleExpression> arguments)
		{
			TweedleConstructor superConst = null;
			while (superConst == null && highestClass != null)
			{
				highestClass = highestClass.SuperClass(this);
				superConst = highestClass?.ConstructorWithArgs(arguments);
			}

			return superConst;
		}
	}
}
