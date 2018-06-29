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

		internal void QueueInvocationStep(string callStack, Dictionary<string, TweedleExpression> arguments, NotifyingStep next)
		{
			//UnityEngine.Debug.Log("Queueing method invocation " + callStack);
			vm.AddStep(InvocationStep(callStack, arguments, next));
		}

		internal virtual NotifyingEvaluationStep InvocationStep(string callStack, Dictionary<string, TweedleExpression> arguments, NotifyingStep next)
		{
			return Method.AsStep(callStack, this, arguments, next);
		}
	}
}
