using System;
using System.Collections.Generic;
using Alice.Tweedle;

namespace Alice.VM
{
	abstract class EvaluationStep : ExecutionStep
	{
		protected TweedleValue result;
		public TweedleValue Result { get { return result; } }

		protected EvaluationStep()
			: base()
		{
		}

		protected EvaluationStep(ExecutionStep blockingStep)
			: base(blockingStep)
		{
		}

		protected EvaluationStep(List<ExecutionStep> blockingSteps)
			: base(blockingSteps)
		{
		}

		internal TweedleValue EvaluateNow()
		{
			foreach (var blocker in BlockingSteps)
			{
				if (blocker is EvaluationStep)
				{
					((EvaluationStep)blocker).EvaluateNow();
				}
				else
				{
					throw new TweedleRuntimeException("Expression depends on a term that can not be evaluated.");
				}
			}
			if (!Execute())
			{
				throw new TweedleRuntimeException("Expression did not complete evaluation.");
			}
			return result;
		}
	}
}