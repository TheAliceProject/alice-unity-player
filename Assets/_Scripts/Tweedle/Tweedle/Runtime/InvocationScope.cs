using System.Collections.Generic;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public abstract class InvocationScope : ExecutionScope
    {
        internal ExecutionScope callingScope;
        public TValue Result { get; internal set; }
        
        private bool m_Returned = false;
        internal override bool ShouldExit()
        {
            return m_Returned;
        }

        protected TMethod method;

        public InvocationScope(IStackFrame callStackEntry, ExecutionScope scope)
            : base(callStackEntry, scope.vm)
        {
            // Note: ExecutionScope.parent is not set by the base constructor used above
            // so permissions aren't inherited from the parent
            // This prevents Readonly and Enum Instantiation permissions from creeping into invalid scopes
            callingScope = scope;
        }

        internal override void StackWith(System.Text.StringBuilder stackBuilder)
        {   
            stackBuilder.Append("\n");
            stackBuilder.Append(callStackEntry);
            callingScope.StackWith(stackBuilder);
        }

        internal void QueueInvocationStep(StepSequence sequentialSteps, NamedArgument[] arguments)
        {
            //UnityEngine.Debug.Log("Queueing method invocation " + callStack);
            method.AddInvocationSteps(this, sequentialSteps, arguments);
        }

        internal virtual ExecutionStep InvocationStep(IStackFrame callStackEntry, NamedArgument[] arguments)
        {
            return method.AsStep(callStackEntry, this, arguments);
        }

        internal override void Return(TValue result)
        {
            if (m_Returned)
                throw new TweedleRuntimeException("Multiple returns fired for a single InvocationScope");
            Result = result;
            m_Returned = true;
        }

        internal override TType CallingType(string methodName)
        {
            return callingScope != null ? callingScope.CallingType(methodName) : base.CallingType(methodName);
        }
    }
}
