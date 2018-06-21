using System.Collections.Generic;

namespace Alice.Tweedle
{
	class ConstructorFrame : InvocationFrame
	{
		internal TweedleClass tweedleClass;

		internal ConstructorFrame(TweedleFrame caller, TweedleClass tweedleClass, Dictionary<string, TweedleExpression> arguments)
			: base(caller)
		{
			this.tweedleClass = tweedleClass;
			thisValue = new TweedleObject(tweedleClass);
			Result = thisValue;
			FindConstructor(arguments);
			CreateArgumentSteps(arguments);
		}

		private ConstructorFrame(ConstructorFrame constructorFrame, TweedleClass superClass, TweedleConstructor constructor, Dictionary<string, TweedleExpression> arguments)
			: base(constructorFrame)
		{
			tweedleClass = superClass;
			thisValue = constructorFrame.thisValue;
			Result = thisValue;
			Method = constructor;
			CreateArgumentSteps(arguments);
		}

		void FindConstructor(Dictionary<string, TweedleExpression> arguments)
		{
			Method = tweedleClass.ConstructorWithArgs(arguments);
		}

		internal ConstructorFrame SuperFrame(Dictionary<string, TweedleExpression> arguments)
		{
			TweedleClass superClass = tweedleClass.SuperClass(this);
			while (superClass != null)
			{
				TweedleConstructor superConst = superClass?.ConstructorWithArgs(arguments);
				if (superConst != null)
				{
					return new ConstructorFrame(this, superClass, superConst, arguments);
				}
				superClass = superClass.SuperClass(this);
			}
			throw new TweedleRuntimeException("No super constructor on" + tweedleClass + " with args " + arguments);
		}
	}
}
