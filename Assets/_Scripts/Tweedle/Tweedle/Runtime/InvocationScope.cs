using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public abstract class InvocationScope : ExecutionScope
	{
		internal ExecutionScope callingScope;
		internal TMethod Method { get; set; }
		public TValue Result { get; internal set; }

		public InvocationScope(ExecutionScope scope)
			: base("Invocation", scope.vm)
		{
			callingScope = scope;
		}

		internal override string StackWith(string stackTop)
		{
			return callingScope.StackWith(stackTop + "\n" + callStackEntry);
		}

		internal void QueueInvocationStep(StepSequence sequentialSteps, NamedArgument[] arguments)
		{
			//UnityEngine.Debug.Log("Queueing method invocation " + callStack);
			Method.AddInvocationSteps(this, sequentialSteps, arguments);
		}

		internal virtual ExecutionStep InvocationStep(string callStackEntry, NamedArgument[] arguments)
		{
			return Method.AsStep(callStackEntry, this, arguments);
		}

		internal void Return(TValue result)
		{
			Result = result;
		}
	}
}
