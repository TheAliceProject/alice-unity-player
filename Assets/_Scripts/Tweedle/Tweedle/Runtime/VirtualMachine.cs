using System.Collections;
using Alice.Tweedle;
using Alice.Tweedle.Parsed;

namespace Alice.VM
{
	public class VirtualMachine // : MonoBehaviour
	{
		ExecutionScope staticScope;
		public TweedleSystem Library { get; private set; }
		public ExecutionQueue executionQueue = new ExecutionQueue();

		public VirtualMachine()
		{
			staticScope = new ExecutionScope("VM", this);
		}

		public VirtualMachine(TweedleSystem tweedleSystem)
		{
			staticScope = new ExecutionScope("VM", this);
			Initialize(tweedleSystem);
		}

		internal void Initialize(TweedleSystem tweedleSystem)
		{
			Library = tweedleSystem;
			InstantiateEnums();
			// TODO Evaluate static variables
			// make enums hard refs?
		}

		void InstantiateEnums()
		{
			// TODO add enums to the staticScope
		}

		public void Queue(TweedleExpression exp)
		{
			Queue(new ExpressionStatement(exp));
		}

		public void Queue(TweedleStatement statement)
		{
			statement.QueueStepToNotify(staticScope, new ExecutionStep(staticScope));
		}

		// Used by tests
		public void ExecuteToFinish(TweedleStatement statement, ExecutionScope scope)
		{
			statement.QueueStepToNotify(scope, new ExecutionStep(scope));
			executionQueue.ProcessOneFrame();
		}

		// Used by tests
		public TweedleValue EvaluateToFinish(TweedleExpression expression, ExecutionScope scope)
		{
			ExecutionStep step = expression.AsStep(scope);
			executionQueue.AddToQueue(step);
			executionQueue.ProcessOneFrame();
			return step.Result;
		}

		internal void AddStep(ExecutionStep step)
		{
			executionQueue.AddToQueue(step);
			executionQueue.ProcessOneFrame();
		}

		internal IEnumerator ProcessQueue()
		{
			executionQueue.ProcessOneFrame();
			yield return null;
		}
	}

}
