using System;
using System.Collections.Generic;
using Alice.Tweedle;
using Alice.Tweedle.Parsed;
using UnityEngine;

namespace Alice.VM
{
	public class VirtualMachine : MonoBehaviour
	{
		TweedleFrame staticFrame;
		public TweedleSystem Library { get; }
		ExecutionQueue executionQueue = new ExecutionQueue();

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
			executionQueue.AddToQueue(statement.RootStep(frame));
			executionQueue.ProcessQueues();
		}

		public TweedleValue EvaluateToFinish(TweedleExpression expression, TweedleFrame frame)
		{
			EvaluationStep step = expression.AsStep(frame);
			executionQueue.AddToQueue(step);
			executionQueue.ProcessQueues();
			return step.Result;
		}

		public void Execute(TweedleStatement statement)
		{
			executionQueue.AddToQueue((ExecutionStep)statement.RootStep(staticFrame));
			StartQueueProcessing();
		}

		private void StartQueueProcessing()
		{
			StartCoroutine("ProcessQueue");
		}
	}

}
