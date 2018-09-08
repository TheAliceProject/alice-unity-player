using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class ConstructorScope : InvocationScope
	{
		private TType m_Type;
        private bool m_RootInitializer;

        internal ConstructorScope(ExecutionScope inCaller, TType inClass)
			: base(inCaller)
		{
			m_Type = inClass;
			thisValue = inClass.CanInstantiateDefault() ? inClass.Instantiate() : TValue.UNDEFINED;
			Result = thisValue;
			callStackEntry = "new " + inClass.Name;
            m_RootInitializer = true;
        }

		private ConstructorScope(ConstructorScope inSubclassScope, TType inSuperType, TMethod inConstructor)
			: base(inSubclassScope)
		{
			m_Type = inSuperType;
			thisValue = inSubclassScope.thisValue;
			Result = thisValue;
			Method = inConstructor;
			callStackEntry = "super() => " + m_Type.Name;
            m_RootInitializer = false;
        }

		protected override ScopePermissions GetPermissions()
		{
            return base.GetPermissions() | ScopePermissions.WriteReadonlyFields;
        }

		internal override ExecutionStep InvocationStep(string callStackEntry, NamedArgument[] arguments)
		{
			Method = m_Type.Constructor(this, arguments);
            if (m_RootInitializer)
            {
                StepSequence main = new StepSequence(callStackEntry, this);
                m_Type.AddDefaultInitializer(this, main);
                main.AddStep(Method.AsStep(callStackEntry, this, arguments));
                return main;
            }
			else
			{
                return base.InvocationStep(callStackEntry, arguments);
            }
        }

		internal ConstructorScope SuperScope(NamedArgument[] arguments)
		{
			TType superType = m_Type.SuperType?.Get(this);
			while (superType != null)
			{
				TMethod superConst = superType.Constructor(this, arguments);
				if (superConst != null)
				{
					return new ConstructorScope(this, superType, superConst);
				}
				superType = superType.SuperType?.Get(this);
			}
			throw new TweedleRuntimeException("No super constructor on" + m_Type + " with args " + arguments);
		}
	}
}
