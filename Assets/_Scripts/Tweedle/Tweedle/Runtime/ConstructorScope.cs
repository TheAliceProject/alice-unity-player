using System.Collections.Generic;

namespace Alice.Tweedle.VM
{
    public class ConstructorScope : InvocationScope
    {
        private TType m_Type;
        private bool m_RootInitializer;

        internal class ConstructorStackFrame: IStackFrame {
            private TType m_InClass;

            public ConstructorStackFrame(TType inClass) {
                m_InClass = inClass;
            }

            public string ToStackFrame()
            {
                return "new " + m_InClass.Name;
            }
        }

        internal class ConstructorSuperStackFrame: IStackFrame {
            private TType m_InSuperType;

            public ConstructorSuperStackFrame(TType inSuperType) {
                m_InSuperType = inSuperType;
            }

            public string ToStackFrame()
            {
                return "super() => " + m_InSuperType.Name;
            }
        }

        internal ConstructorScope(ExecutionScope inCaller, TType inClass)
            : base(new ConstructorStackFrame(inClass), inCaller)
        {
            m_Type = inClass;
            thisValue = inClass.HasDefaultInstantiate() ? inClass.Instantiate() : TValue.UNDEFINED;
            Result = thisValue;
            m_RootInitializer = true;
        }

        internal ConstructorScope(ExecutionScope inCaller, TEnumType inEnumType, TEnumValueInitializer inValueInitializer)
            : this(inCaller, inEnumType)
        {
            Result = thisValue = inEnumType.Instantiate(inValueInitializer);
        }

        private ConstructorScope(ConstructorScope inSubclassScope, TType inSuperType, TMethod inConstructor)
            : base(new ConstructorSuperStackFrame(inSuperType), inSubclassScope)
        {
            m_Type = inSuperType;
            thisValue = inSubclassScope.thisValue;
            Result = thisValue;
            method = inConstructor;
            m_RootInitializer = false;
        }

        protected override ScopePermissions GetPermissions()
        {
            return base.GetPermissions() | ScopePermissions.WriteReadonlyFields;
        }

        internal override ExecutionStep InvocationStep(IStackFrame callStackEntry, NamedArgument[] arguments)
        {
            method = m_Type.Constructor(this, arguments);
            if (m_RootInitializer)
            {
                StepSequence main = new StepSequence(callStackEntry, this);
                m_Type.AddInstanceInitializer(this, main);
                main.AddStep(method.AsStep(callStackEntry, this, arguments));
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
