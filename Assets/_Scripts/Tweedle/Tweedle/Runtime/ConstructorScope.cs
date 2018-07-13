using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	class ConstructorScope : InvocationScope
	{
		internal TweedleClass tweedleClass;

		internal ConstructorScope(ExecutionScope caller, TweedleClass tweedleClass)
			: base(caller)
		{
			this.tweedleClass = tweedleClass;
			thisValue = new TweedleObject(tweedleClass);
			Result = thisValue;
			callStackEntry = "new " + tweedleClass.Name;
		}

		ConstructorScope(ConstructorScope constructorSubclassScope, TweedleClass superClass, TweedleConstructor constructor)
			: base(constructorSubclassScope)
		{
			tweedleClass = superClass;
			thisValue = constructorSubclassScope.thisValue;
			Result = thisValue;
			Method = constructor;
			callStackEntry = "super() => " + tweedleClass.Name;
		}

		internal override ExecutionStep InvocationStep(string callStackEntry, Dictionary<string, TweedleExpression> arguments)
		{
			Method = tweedleClass.ConstructorWithArgs(arguments);
			return base.InvocationStep(callStackEntry, arguments);
		}

		internal ConstructorScope SuperScope(Dictionary<string, TweedleExpression> arguments)
		{
			TweedleClass superClass = tweedleClass.SuperClass(this);
			while (superClass != null)
			{
				TweedleConstructor superConst = superClass?.ConstructorWithArgs(arguments);
				if (superConst != null)
				{
					return new ConstructorScope(this, superClass, superConst);
				}
				superClass = superClass.SuperClass(this);
			}
			throw new TweedleRuntimeException("No super constructor on" + tweedleClass + " with args " + arguments);
		}
	}
}
