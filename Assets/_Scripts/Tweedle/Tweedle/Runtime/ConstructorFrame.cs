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

		internal override NotifyingEvaluationStep InvocationStep(string callStack, NotifyingStep parent, Dictionary<string, TweedleExpression> arguments)
		{
			Method = tweedleClass.ConstructorWithArgs(arguments);
			return base.InvocationStep(callStack, parent, arguments);
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

		protected override void AddSteps(SequentialStepsEvaluation main, Dictionary<string, TweedleExpression> arguments)
		{
			AddFieldSteps(main);
			base.AddSteps(main, arguments);
		}

		void AddFieldSteps(SequentialStepsEvaluation main)
		{
			foreach (NotifyingStep initFieldStep in ((TweedleObject)thisValue).InitializationNotifyingSteps(this))
			{
				main.AddStep(initFieldStep);
			}
		}
	}
}
