using System.Collections;
using Alice.Tweedle;
using Alice.Tweedle.Parsed;

namespace Alice.VM
{
	public class VirtualMachine // : MonoBehaviour
	{
		TweedleFrame staticFrame;
		public TweedleSystem Library { get; private set; }
		public NotifyingStepExecutionQueue executionQueue = new NotifyingStepExecutionQueue();

		public VirtualMachine()
		{
			staticFrame = new TweedleFrame("VM", this);
		}

		public VirtualMachine(TweedleSystem tweedleSystem)
		{
			staticFrame = new TweedleFrame("VM", this);
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
			// TODO add enums to the staticFrame
		}

		public void Queue(TweedleExpression exp)
		{
			Queue(new ExpressionStatement(exp));
		}

		public void Queue(TweedleStatement statement)
		{
			statement.QueueStepToNotify(staticFrame, new ExecutionStep(staticFrame));
		}

		// Used by tests
		public void ExecuteToFinish(TweedleStatement statement, TweedleFrame frame)
		{
			statement.QueueStepToNotify(frame, new ExecutionStep(frame));
			executionQueue.ProcessOneFrame();
		}

		// Used by tests
		public TweedleValue EvaluateToFinish(TweedleExpression expression, TweedleFrame frame)
		{
			ExecutionStep step = expression.AsStep(frame);
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
