using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public abstract class InvocationScope : ExecutionScope
	{
		internal ExecutionScope callingScope;
		public TValue Result { get; internal set; }

        protected TMethod method;

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
			method.AddInvocationSteps(this, sequentialSteps, arguments);
		}

		internal virtual ExecutionStep InvocationStep(string callStackEntry, NamedArgument[] arguments)
		{
			return method.AsStep(callStackEntry, this, arguments);
		}

		internal void Return(TValue result)
		{
			Result = result;
		}
	}
}
