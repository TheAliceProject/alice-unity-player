using System.Collections.Generic;

namespace Alice.Tweedle.VM
{
    public class StaticConstructorScope : ExecutionScope, IStackFrame
    {
        private TType m_Type;
        
        internal StaticConstructorScope(ExecutionScope inCaller, TType inClass)
            : base(null, inCaller.vm)
        {
            m_Type = inClass;
            thisValue = TValue.FromType(inClass);
            callStackEntry = this;
        }

        protected override ScopePermissions GetPermissions()
        {
            return base.GetPermissions() | ScopePermissions.WriteReadonlyFields | ScopePermissions.InstantiateEnum;
        }

        internal ExecutionStep InvocationStep(IStackFrame callStackEntry)
        {
            StepSequence main = new StepSequence(callStackEntry, this);
            m_Type.AddStaticInitializer(this, main);
            return main;
        }

        public virtual string ToStackFrame() {
            return m_Type.Name + "()";
        }
    }
}
