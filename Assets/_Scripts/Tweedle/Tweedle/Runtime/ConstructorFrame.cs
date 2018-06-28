using System.Collections.Generic;
using Alice.VM;

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
			callStackEntry = "new " + tweedleClass.Name;
			CreateArgumentSteps(arguments);
		}

		ConstructorFrame(ConstructorFrame constructorFrame, TweedleClass superClass, TweedleConstructor constructor, Dictionary<string, TweedleExpression> arguments)
			: base(constructorFrame)
		{
			tweedleClass = superClass;
			thisValue = constructorFrame.thisValue;
			Result = thisValue;
			Method = constructor;
			callStackEntry = "super() => " + tweedleClass.Name;
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

		internal override EvaluationStep InvokeStep(string callStack)
		{
			EvaluationStep invocation = base.InvokeStep(callStack);
			invocation.AddBlockingStep(InitializeObjectStep());
			return invocation;
		}

		protected override void AddSteps(SequentialStepsEvaluation main, Dictionary<string, TweedleExpression> arguments)
		{
			AddFieldSteps(main);
			base.AddSteps(main, arguments);
		}

		private void AddFieldSteps(SequentialStepsEvaluation main)
		{
			foreach (NotifyingStep initFieldStep in ((TweedleObject)thisValue).InitializationNotifyingSteps(this))
			{
				main.AddStep(initFieldStep);
			}
		}

		private ExecutionStep InitializeObjectStep()
		{
			var initObjectStep = new CompletionStep();
			foreach (ExecutionStep initFieldStep in ((TweedleObject)thisValue).InitializationSteps(this))
			{
				initObjectStep.AddBlockingStep(initFieldStep);
			}
			return initObjectStep;
		}
	}
}
