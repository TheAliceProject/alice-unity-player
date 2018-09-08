using Alice.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Tweedle class type.
    /// </summary>
    public sealed class TClassType : TType
    {
        private TField[] m_Fields;
        private TMethod[] m_Methods;
        private TMethod[] m_Constructors;

        public TClassType(string inName, TField[] inFields, TMethod[] inMethods, TMethod[] inConstructors)
            : base(inName)
        {
            m_Fields = inFields;
            m_Methods = inMethods;
            m_Constructors = inConstructors;
        }

        public TClassType(string inName, string inSuperType, TField[] inFields, TMethod[] inMethods, TMethod[] inConstructors)
            : base(inName, inSuperType)
        {
            m_Fields = inFields;
            m_Methods = inMethods;
            m_Constructors = inConstructors;
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

        public override bool IsReferenceType()
        {
            return true;
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
        
        #region Internal

        protected override void Finalize(Parse.TweedleSystem inSystem)
        {
            base.Finalize(inSystem);

            ResolveMembers(m_Fields, inSystem, this);
            ResolveMembers(m_Methods, inSystem, this);
            ResolveMembers(m_Constructors, inSystem, this);
        }

        #endregion // Internal

        #region Lifecycle

        public override TValue Instantiate()
        {
            return TValue.FromObject(this, new TObject());
        }

        public override TValue DefaultValue()
        {
            return TValue.NULL;
        }

        public override void AddDefaultInitializer(ConstructorScope inScope, StepSequence ioSteps)
        {
            base.AddDefaultInitializer(inScope, ioSteps);

            TValue _this = inScope.GetThis();
            for (int i = 0; i < m_Fields.Length; ++i)
            {
                TField field = m_Fields[i];
                if ((field.Flags & MemberFlags.Instance) == MemberFlags.Instance)
                {
                    ExecutionStep initializer = field.InitializeStep(inScope, ref _this);
                    if (initializer != null)
                        ioSteps.AddStep(initializer);
                }
            }
        }

        #endregion // Lifecycle

        public override bool Equals(ref TValue inValA, ref TValue inValB)
        {
            return base.Equals(ref inValA, ref inValB)
                && inValA.Object() == inValB.Object();
        }

        public override bool LessThan(ref TValue inValA, ref TValue inValB)
        {
            return false;
        }

        #region Tweedle Casting

        #endregion // Tweedle Casting

        #region Conversion Semantics

        public override object ConvertToPObject(ref TValue inValue)
        {
            return inValue;
        }

        #endregion // Conversion Semantics

        #region Misc

        public override bool IsValidIdentifier()
        {
            return true;
        }

        public override int GetHashCode(ref TValue inValue)
        {
            return inValue.Object().GetHashCode();
        }

        public override string ToTweedle(ref TValue inValue)
        {
            return Name;
        }

        #endregion // Misc
    }
}