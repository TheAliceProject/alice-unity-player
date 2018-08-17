using System.Collections;
using Alice.Tweedle;
using Alice.Tweedle.Parse;

namespace Alice.VM
{
	// For use running unit tests.
	public class TestVirtualMachine : VirtualMachine
	{
		public TestVirtualMachine()
		{
		}

		public TestVirtualMachine(TweedleSystem tweedleSystem)
			: base(tweedleSystem)
		{
		}

		public void ExecuteToFinish(TweedleStatement statement, ExecutionScope scope)
		{
			statement.QueueStepToNotify(scope, new ExecutionStep(scope));
			executionQueue.ProcessOneFrame();
		}

		public TweedleValue EvaluateToFinish(TweedleExpression expression, ExecutionScope scope)
		{
			ExecutionStep step = expression.AsStep(scope);
			AddStep(step);
			return step.Result;
		}
	}
}
