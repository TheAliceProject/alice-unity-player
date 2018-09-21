using System.Collections;
using Alice.Tweedle;
using Alice.Tweedle.Parse;

namespace Alice.Tweedle.VM
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
            Library?.Prep(this);
        }

		public void Queue(ITweedleExpression exp)
		{
			Queue(new ExpressionStatement(exp));
		}

		public void Queue(TweedleStatement statement)
		{
			statement.QueueStepToNotify(staticScope, new ExecutionStep(staticScope));
		}

		protected internal void AddStep(ExecutionStep step)
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
