using System;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Tweedle class type.
    /// </summary>
    public abstract class TTypeWithMembers : TType
    {
        protected TField[] m_Fields { get; private set; }
        protected TMethod[] m_Methods { get; private set; }
        protected TMethod[] m_Constructors { get; private set; }
        
        private bool m_HasStaticConstructor;

        protected TTypeWithMembers(TAssembly inAssembly, string inName)
            : base(inAssembly, inName)
        {
        }

        public TTypeWithMembers(TAssembly inAssembly, string inName, string inSuperType)
            : base(inAssembly, inName, inSuperType)
        {
        }

        public TTypeWithMembers(TAssembly inAssembly, string inName, TTypeRef inSuperType)
            : base(inAssembly, inName, inSuperType)
        {
        }

        protected void AssignMembers(TField[] inFields, TMethod[] inMethods, TMethod[] inConstructors)
        {
            m_Fields = inFields;
            m_Methods = inMethods;
            m_Constructors = inConstructors;

            m_HasStaticConstructor = HasStaticInitializerField();
        }

        protected virtual bool HasStaticInitializerField()
        {
            for (int i = 0; i < m_Fields.Length; ++i)
            {
                TField field = m_Fields[i];
                if (field.IsStatic() && field.HasInitializer())
                    return true;
            }

            return false;
        }

        #region Object Semantics

        public override TField Field(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            AssertValueIsTypeOrTypeRef(ref inValue);
            TField field = FindMember(m_Fields, inName, inFlags | MemberFlags.Field);
            if (field == null && SuperType != null)
                field = SuperType.Get(inScope).Field(inScope, ref inValue, inName, inFlags);
            return field;
        }

        public override TMethod Method(ExecutionScope inScope, ref TValue inValue, string inName, string[] inArgNames, MemberFlags inFlags = MemberFlags.None)
        {
            AssertValueIsTypeOrTypeRef(ref inValue);
            TMethod method = FindMethodWithArgsQuietly(m_Methods, inName, inArgNames, inFlags | MemberFlags.Method);
            if (method == null && SuperType != null)
                method = SuperType.Get(inScope).Method(inScope, ref inValue, inName, inArgNames, inFlags);
            return method;
        }

        public override TMethod Constructor(ExecutionScope inScope, NamedArgument[] inArguments)
        {
            string[] argumentNames = Array.ConvertAll(inArguments, (arg) => arg.Name);
            return FindMethodWithArgs(m_Constructors, TMethod.ConstructorName, argumentNames, MemberFlags.Instance | MemberFlags.Constructor);
        }

        public override TField[] Fields(ExecutionScope inScope, ref TValue inValue)
        {
            return m_Fields;
        }

        public override TMethod[] Methods(ExecutionScope inScope, ref TValue inValue)
        {
            return m_Methods;
        }

        public override TMethod[] Constructors(ExecutionScope inScope)
        {
            return m_Constructors;
        }

        #endregion // Object Semantics

        #region Static

        protected override bool HasStaticConstructor()
        {
            return m_HasStaticConstructor;
        }

        #endregion // Static

        #region Link

        protected override void LinkImpl(TAssembly inAssembly)
        {
            base.LinkImpl(inAssembly);

            LinkMembers(m_Fields, inAssembly, this);
            LinkMembers(m_Methods, inAssembly, this);
            LinkMembers(m_Constructors, inAssembly, this);
        }

        #endregion // Link
    }
}