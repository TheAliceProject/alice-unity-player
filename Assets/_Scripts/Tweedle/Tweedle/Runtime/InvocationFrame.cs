using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public abstract class InvocationFrame : TweedleFrame
	{
		internal TweedleFrame callingFrame;
		internal TweedleMethod Method { get; set; }
		public TweedleValue Result { get; internal set; }

		public InvocationFrame(TweedleFrame frame)
			: base("Invocation", frame.vm)
		{
			callingFrame = frame;
		}

		internal override string StackWith(string stackTop)
		{
			return callingFrame.StackWith(stackTop + "\n" + callStackEntry);
		}

		internal void QueueInvocationStep(SequentialStepsEvaluation sequentialSteps, Dictionary<string, TweedleExpression> arguments)
		{
			//UnityEngine.Debug.Log("Queueing method invocation " + callStack);
			Method.AddInvocationSteps(this, sequentialSteps, arguments);
		}

		internal virtual NotifyingEvaluationStep InvocationStep(string callStackEntry, Dictionary<string, TweedleExpression> arguments)
		{
			return Method.AsStep(callStackEntry, this, arguments);
		}
	}
}
