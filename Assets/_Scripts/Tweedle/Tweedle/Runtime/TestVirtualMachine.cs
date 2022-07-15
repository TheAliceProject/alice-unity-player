using System.Collections;
using Alice.Tweedle;
using Alice.Tweedle.Parse;

namespace Alice.Tweedle.VM
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

        public static void ExecuteToFinish(TweedleStatement statement, ExecutionScope scope) {
            statement.QueueStepToNotify(scope);
        }

        public TValue EvaluateToFinish(ITweedleExpression expression, ExecutionScope scope)
        {
            ExecutionStep step = expression.AsStep(scope);
            AddStep(step);
            return step.Result;
        }

        public void QueueProgramMain(TweedleSystem system) {
            system.QueueProgramMain(this);
        }
    }
}
