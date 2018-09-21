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

        protected TTypeWithMembers(string inName)
            : base(inName)
        {
        }

        public TTypeWithMembers(string inName, string inSuperType)
            : base(inName, inSuperType)
        {
        }

        public TTypeWithMembers(string inName, TTypeRef inSuperType)
            : base(inName, inSuperType)
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

        public override TMethod Method(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            AssertValueIsTypeOrTypeRef(ref inValue);
            TMethod method = FindMember(m_Methods, inName, inFlags | MemberFlags.Method);
            if (method == null && SuperType != null)
                method = SuperType.Get(inScope).Method(inScope, ref inValue, inName, inFlags);
            return method;
        }

        public override TMethod Constructor(ExecutionScope inScope, NamedArgument[] inArguments)
        {
            return FindMethodWithArgs(m_Constructors, TMethod.ConstructorName, inArguments, MemberFlags.Instance | MemberFlags.Constructor);
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

        protected override void LinkImpl(Parse.TweedleSystem inSystem)
        {
            base.LinkImpl(inSystem);

            LinkMembers(m_Fields, inSystem, this);
            LinkMembers(m_Methods, inSystem, this);
            LinkMembers(m_Constructors, inSystem, this);
        }

        #endregion // Link
    }
}