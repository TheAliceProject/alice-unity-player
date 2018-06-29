using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	class ConstructorFrame : InvocationFrame
	{
		internal TweedleClass tweedleClass;

		internal ConstructorFrame(TweedleFrame caller, TweedleClass tweedleClass)
			: base(caller)
		{
			this.tweedleClass = tweedleClass;
			thisValue = new TweedleObject(tweedleClass);
			Result = thisValue;
			callStackEntry = "new " + tweedleClass.Name;
		}

		ConstructorFrame(ConstructorFrame constructorFrame, TweedleClass superClass, TweedleConstructor constructor)
			: base(constructorFrame)
		{
			tweedleClass = superClass;
			thisValue = constructorFrame.thisValue;
			Result = thisValue;
			Method = constructor;
			callStackEntry = "super() => " + tweedleClass.Name;
		}

		internal override NotifyingEvaluationStep InvocationStep(string callStack, Dictionary<string, TweedleExpression> arguments, NotifyingStep next)
		{
			Method = tweedleClass.ConstructorWithArgs(arguments);
			return base.InvocationStep(callStack, arguments, next);
		}

		internal ConstructorFrame SuperFrame(Dictionary<string, TweedleExpression> arguments)
		{
			TweedleClass superClass = tweedleClass.SuperClass(this);
			while (superClass != null)
			{
				TweedleConstructor superConst = superClass?.ConstructorWithArgs(arguments);
				if (superConst != null)
				{
					return new ConstructorFrame(this, superClass, superConst);
				}
				superClass = superClass.SuperClass(this);
			}
			throw new TweedleRuntimeException("No super constructor on" + tweedleClass + " with args " + arguments);
		}
	}
}
