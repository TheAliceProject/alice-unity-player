using Alice.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Tweedle class type.
    /// </summary>
    public sealed class TClassType : TTypeWithMembers
    {
        public TClassType(string inName, TField[] inFields, TMethod[] inMethods, TMethod[] inConstructors)
            : base(inName)
        {
            AssignMembers(inFields, inMethods, inConstructors);
        }

        public TClassType(string inName, string inSuperType, TField[] inFields, TMethod[] inMethods, TMethod[] inConstructors)
            : base(inName, inSuperType)
        {
            AssignMembers(inFields, inMethods, inConstructors);
        }
        
        #region Statics

        public override void AddStaticInitializer(ExecutionScope inScope, StepSequence ioSteps)
        {
            TValue thisRef = TValue.FromType(this);
            for (int i = 0; i < m_Fields.Length; ++i)
            {
                TField field = m_Fields[i];
                if (field.IsStatic())
                    ioSteps.AddStep(field.InitializeStep(inScope, ref thisRef));
            }
        }

        #endregion // Statics

        public override bool IsReferenceType() { return true; }

        #region Lifecycle

        public override TValue Instantiate()
        {
            return TValue.FromObject(this, new TObject());
        }

        public override TValue DefaultValue()
        {
            return TValue.NULL;
        }

        public override void AddInstanceInitializer(ConstructorScope inScope, StepSequence ioSteps)
        {
            base.AddInstanceInitializer(inScope, ioSteps);

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