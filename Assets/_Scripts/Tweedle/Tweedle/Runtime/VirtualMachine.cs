using Alice.Tweedle;
using Alice.Tweedle.Parsed;
using UnityEngine;

namespace Alice.VM
{
	public class VirtualMachine : MonoBehaviour
	{
		TweedleFrame staticFrame;
		public TweedleSystem Library { get; }
		public NotifyingStepExecutionQueue executionQueue = new NotifyingStepExecutionQueue();

		public VirtualMachine(TweedleSystem tweedleSystem)
		{
			Library = tweedleSystem;
			Initialize();
		}

		void Initialize()
		{
			staticFrame = new TweedleFrame("VM", this);
			InstantiateEnums();
			// TODO Evaluate static variables
			// make enums hard refs?
		}

		void InstantiateEnums()
		{
			// TODO add enums to the staticFrame
			// throw new NotImplementedException();
		}

		public void Execute(TweedleExpression exp)
		{
			Execute(new ExpressionStatement(exp));
		}

		public void ExecuteToFinish(TweedleStatement statement, TweedleFrame frame)
		{
			statement.AddStep(null, frame);
			executionQueue.ProcessQueues();
		}

		public TweedleValue EvaluateToFinish(TweedleExpression expression, TweedleFrame frame)
		{
			NotifyingEvaluationStep step = expression.AsStep(null, frame);
			executionQueue.AddToQueue(step);
			executionQueue.ProcessQueues();
			return step.Result;
		}

		public void Execute(TweedleStatement statement)
		{
			statement.AddStep(null, staticFrame);
			StartQueueProcessing();
		}

		public void AddStep(NotifyingStep step)
		{
			executionQueue.AddToQueue(step);
			executionQueue.ProcessQueues();
		}

		private void StartQueueProcessing()
		{
			StartCoroutine("ProcessQueue");
		}
	}

}
