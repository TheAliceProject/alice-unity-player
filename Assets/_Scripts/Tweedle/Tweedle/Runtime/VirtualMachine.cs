using System.Collections;
using Alice.Tweedle;
using Alice.Tweedle.Parse;

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
