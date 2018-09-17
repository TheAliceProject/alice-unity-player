using System;
using Alice.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Tweedle enum type.
    /// </summary>
    public sealed class TEnumType : TTypeWithMembers
    {
        private TEnumValueInitializer[] m_ValueInitializers;

        public TEnumType(string inName, TEnumValueInitializer[] inValueInitializers, TField[] inFields, TMethod[] inMethods, TMethod[] inConstructors)
            : base(inName)
        {
            m_ValueInitializers = inValueInitializers;

            AppendEnumValues(ref inFields, inValueInitializers);
            AssignMembers(inFields, inMethods, inConstructors);
        }

        public TEnumValueInitializer[] ValueInitializers() { return m_ValueInitializers; }

        public TEnumValueInitializer GetValueInitializer(string inName)
        {
            for (int i = 0; i < m_ValueInitializers.Length; ++i)
            {
                if (m_ValueInitializers[i].Name == inName)
                    return m_ValueInitializers[i];
            }

            return null;
        }

        #region Statics

        protected override void PostLinkImpl(Parse.TweedleSystem inSystem)
        {
            base.PostLinkImpl(inSystem);

            for (int i = 0; i < m_ValueInitializers.Length; ++i)
                m_ValueInitializers[i].AssignValue(i);
        }

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

        public override bool IsReferenceType() { return false; }

        #region Lifecycle

        public override bool CanInstantiate(ExecutionScope inScope)
        {
            return inScope.HasPermissions(ScopePermissions.InstantiateEnum);
        }

        public override TValue Instantiate()
        {
            return TValue.UNDEFINED;
        }

        public TValue Instantiate(TEnumValueInitializer inInitializer)
        {
            return TValue.FromObject(this, new TEnum(inInitializer.Name, inInitializer.Value));
        }

        public override TValue DefaultValue()
        {
            return TValue.UNDEFINED;
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
            return inValA.Enum().Value < inValB.Enum().Value;
        }

        #region Tweedle Casting

        #endregion // Tweedle Casting

        #region Conversion Semantics

        public override int ConvertToInt(ref TValue inValue)
        {
            return inValue.Enum().Value;
        }

        public override string ConvertToString(ref TValue inValue)
        {
            return inValue.Enum().Name;
        }

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
            return inValue.Enum().GetHashCode();
        }

        public override string ToTweedle(ref TValue inValue)
        {
            return inValue.Enum().Name;
        }

        #endregion // Misc

        private void AppendEnumValues(ref TField[] ioFields, TEnumValueInitializer[] inValues)
        {
            int startIdx = ioFields.Length;
            Array.Resize(ref ioFields, ioFields.Length + inValues.Length);
            for (int i = 0; i < inValues.Length; ++i)
            {
                var valueInitializer = inValues[i];
                TStaticField enumField = new TStaticField(valueInitializer.Name, SelfRef, MemberFlags.Readonly, new EnumInstantiation(this, valueInitializer));
                ioFields[startIdx + i] = enumField;
            }
        }
    }

    public sealed class TEnumValueInitializer
    {
        public readonly string Name;
        public readonly NamedArgument[] Arguments;
        public int Value { get; private set; }

        public TEnumValueInitializer(string inName, NamedArgument[] inArguments)
        {
            Name = inName;
            Arguments = inArguments;
        }

        public void AssignValue(int inValue)
        {
            Value = inValue;
        }
    }
}