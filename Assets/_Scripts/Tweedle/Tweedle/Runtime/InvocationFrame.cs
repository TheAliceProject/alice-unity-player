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

		internal void QueueInvocationStep(StepSequence sequentialSteps, Dictionary<string, TweedleExpression> arguments)
		{
			//UnityEngine.Debug.Log("Queueing method invocation " + callStack);
			Method.AddInvocationSteps(this, sequentialSteps, arguments);
		}

		internal virtual ExecutionStep InvocationStep(string callStackEntry, Dictionary<string, TweedleExpression> arguments)
		{
			return Method.AsStep(callStackEntry, this, arguments);
		}

		internal void Return(TweedleValue result)
		{
			Result = result;
		}
	}
}
