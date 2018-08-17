using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public abstract class InvocationScope : ExecutionScope
	{
		internal ExecutionScope callingScope;
		internal TweedleMethod Method { get; set; }
		public TweedleValue Result { get; internal set; }

		public InvocationScope(ExecutionScope scope)
			: base("Invocation", scope.vm)
		{
			callingScope = scope;
		}

		internal override string StackWith(string stackTop)
		{
			return callingScope.StackWith(stackTop + "\n" + callStackEntry);
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
