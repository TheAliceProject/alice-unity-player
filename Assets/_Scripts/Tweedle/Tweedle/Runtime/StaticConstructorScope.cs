using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class StaticConstructorScope : ExecutionScope
	{
		private TType m_Type;
        
        internal StaticConstructorScope(ExecutionScope inCaller, TType inClass)
			: base(inCaller)
		{
			m_Type = inClass;
			thisValue = TValue.FromType(inClass);
            callStackEntry = inClass.Name + "()";
        }

		protected override ScopePermissions GetPermissions()
		{
            return base.GetPermissions() | ScopePermissions.WriteReadonlyFields | ScopePermissions.InstantiateEnum;
        }

		internal ExecutionStep InvocationStep(string callStackEntry)
		{
			StepSequence main = new StepSequence(callStackEntry, this);
            m_Type.AddStaticInitializer(this, main);
            return main;
        }
	}
}
