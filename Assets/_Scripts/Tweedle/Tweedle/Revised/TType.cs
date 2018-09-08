using System;
using System.Diagnostics;
using System.Text;
using Alice.Tweedle.Parse;

namespace Alice.Tweedle
{
    public abstract class TType
    {
        public readonly string Name;
        public readonly TTypeRef SuperType;
        public readonly TTypeRef SelfRef;

        private TObject m_StaticStorage;
        private bool m_Finalized;

        protected TType(string inName) : this(inName, (TTypeRef)null) { }

        protected TType(string inName, TType inSuperType) : this(inName, (TTypeRef)inSuperType) { }

        protected TType(string inName, string inSuperTypeName) : this(inName, new TTypeRef(inSuperTypeName)) { }

        protected TType(string inName, TTypeRef inSuperTypeRef)
        {
            Name = inName;
            SuperType = inSuperTypeRef;
            SelfRef = new TTypeRef(this);
        }

        #region Internal

        // Returns the backing object for static fields on this type
        internal TObject StaticStorage()
        {
            if (m_StaticStorage == null)
                m_StaticStorage = new TObject();
            return m_StaticStorage;
        }

        // Resolves links between types
        protected virtual void Finalize(TweedleSystem inSystem)
        {
            if (SuperType != null)
                SuperType.Resolve(inSystem);
            m_Finalized = true;
        }

        #endregion // Internal

        #region Object Semantics 

        public abstract TField Field(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None);
        public abstract TMethod Method(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None);
        public abstract TMethod Constructor(ExecutionScope inScope, NamedArgument[] inArguments);

        public abstract bool IsReferenceType();

        public virtual TField[] Fields(ExecutionScope inScope, ref TValue inValue) { return TField.EMPTY_ARRAY; }
        public virtual TMethod[] Methods(ExecutionScope inScope, ref TValue inValue) { return TMethod.EMPTY_ARRAY; }
        public virtual TMethod[] Constructors(ExecutionScope inScope) { return TMethod.EMPTY_ARRAY; }

        // Shortcuts for accessing without a value

        public TField Field(ExecutionScope inScope, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            TValue val = new TValue(this);
            return Field(inScope, ref val, inName, inFlags);
        }

        public TMethod Method(ExecutionScope inScope, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            TValue val = new TValue(this);
            return Method(inScope, ref val, inName, inFlags);
        }

        public TField[] Fields(ExecutionScope inScope)
        {
            TValue val = new TValue(this);
            return Fields(inScope, ref val);
        }

        public TMethod[] Methods(ExecutionScope inScope)
        {
            TValue val = new TValue(this);
            return Methods(inScope, ref val);
        }

        #endregion // Object Semantics

        #region Lifecycle

        public virtual bool CanInstantiateDefault() { return true; }
        
        public abstract TValue Instantiate();
        public abstract TValue DefaultValue();

        public virtual void AddDefaultInitializer(ConstructorScope inScope, StepSequence ioSteps)
        {
            // Default initialization steps for super class
            if (SuperType != null)
                SuperType.Get(inScope).AddDefaultInitializer(inScope, ioSteps);
        }

        #endregion // Lifecycle

        #region Comparison Semantics

        public virtual bool Equals(ref TValue inValA, ref TValue inValB)
        {
            AssertValueIsType(ref inValA);

            if (inValA.Type != inValB.Type)
                return false;

            return true;
        }

        public virtual bool LessThan(ref TValue inValA, ref TValue inValB)
        {
            AssertValueIsType(ref inValA);
            return false;
        }

        #endregion // Comparison Semantics

        #region Tweedle Casting

        public virtual bool CanCast(TType inType)
        {
            TType type = this;
            while (type != null)
            {
                if (type == inType)
                    return true;
                type = type.SuperType;
            }
            return inType == TStaticTypes.ANY;
        }

        public virtual TValue Cast(ref TValue inValue, TType inType)
        {
            AssertValueIsType(ref inValue);
            return inValue;
        }

        // This check is disabled in non-development builds
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("DEBUG")]
        protected void AssertValueIsType(ref TValue inValue)
        {
            if (!InstanceOf(ref inValue, this, false))
                throw new TweedleRuntimeException("Expected type " + this + ", but value was type " + inValue.Type);
        }

        // This check is disabled in non-development builds
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("DEBUG")]
        protected void AssertValueIsTypeOrTypeRef(ref TValue inValue)
        {
            if (!InstanceOf(ref inValue, this, true))
                throw new TweedleRuntimeException("Expected type " + this + " or " + TStaticTypes.TYPE_REF + ", but value was type " + inValue.Type);
        }

        #endregion // Tweedle casting

        #region Conversion Semantics

        public virtual int ConvertToInt(ref TValue inValue)
        {
            throw new TweedleRuntimeException("This type (" + this + ") cannot convert the value " + inValue + " to an integer.");
        }

        public virtual double ConvertToDouble(ref TValue inValue)
        {
            throw new TweedleRuntimeException("This type (" + this + ") cannot convert the value " + inValue + " to a double.");
        }

        public virtual string ConvertToString(ref TValue inValue)
        {
            throw new TweedleRuntimeException("This type (" + this + ") cannot convert the value " + inValue + " to a string.");
        }

        public virtual bool ConvertToBoolean(ref TValue inValue)
        {
            throw new TweedleRuntimeException("This type (" + this + ") cannot convert the value " + inValue + " to a boolean.");
        }

        public virtual object ConvertToPObject(ref TValue inValue)
        {
            throw new TweedleRuntimeException("This type (" + this + ") cannot convert the value " + inValue + " to a c-sharp object.");
        }

        #endregion // Conversion Semantics

        #region Misc

        public virtual bool IsValidIdentifier()
        {
            return true;
        }

        public virtual bool IsValidReturnType()
        {
            return IsValidIdentifier();
        }

        public virtual string ToTweedle(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return "a " + Name;
        }

        public abstract int GetHashCode(ref TValue inValue);

        public override string ToString()
        {
            return Name;
        }

        #endregion // Misc
    
        #region Helper Methods

        /// <summary>
        /// Finds the member with the given name and flags.
        /// </summary>
        static protected T FindMember<T>(T[] inMembers, string inName, MemberFlags inFlags) where T : class, ITypeMember
        {
            for (int i = 0; i < inMembers.Length; ++i)
            {
                T member = inMembers[i];
                if (member.Name.Equals(inName, StringComparison.Ordinal))
                {
                    if ((member.Flags & inFlags) != inFlags)
                        return null;

                    return member;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a method with the given name, arguments, and flags.
        /// </summary>
        static protected T FindMethodWithArgs<T>(T[] inMembers, string inName, NamedArgument[] inArguments, MemberFlags inFlags) where T : TMethod
        {
            for (int i = 0; i < inMembers.Length; ++i)
            {
                T method = inMembers[i];
                if (method.Name.Equals(inName, StringComparison.Ordinal)
                    && (method.Flags & inFlags) == inFlags
                    && method.ExpectsArgs(inArguments))
                    return method;
            }

            return null;
        }

        /// <summary>
        /// Resolves all members.
        /// </summary>
        static protected void ResolveMembers<T>(T[] inMembers, TweedleSystem inSystem, TType inType) where T : ITypeMember
        {
            for (int i = 0; i < inMembers.Length; ++i)
            {
                inMembers[i].Resolve(inSystem, inType);
            }
        }

        /// <summary>
        /// Returns if a source type is assignable to the target type.
        /// </summary>
        static protected bool IsAssignableFrom(TType inTarget, TType inSource)
        {
            TType type = inSource;
            while(type != null)
            {
                if (type == inSource)
                    return true;
                type = inSource.SuperType.Get();
            }

            return inSource == TStaticTypes.ANY;
        }

        /// <summary>
        /// Returns if a value's type is within the given type hierarchy.
        /// </summary>
        static protected bool InstanceOf(ref TValue inValue, TType inType, bool inbAllowTypeRef = true)
        {
            TType type = inValue.Type;
            if (type == inType)
                return true;
            if (inbAllowTypeRef && type == TStaticTypes.TYPE_REF)
                type = inValue.TypeRef().Get();
            return IsAssignableFrom(inType, type);
        }

        #endregion // Helper Methods

        /// <summary>
        /// Finalizes type references, methods, and fields.
        /// </summary>
        static public void Finalize(TweedleSystem inSystem, TType inType)
        {
            TType type = inType;
            while(type != null && !type.m_Finalized)
            {
                type.Finalize(inSystem);
                type = type.SuperType;
            }
        }

        #region Debugging

        static public string DumpOutline(TType inType)
        {
            StringBuilder builder = new StringBuilder(1024);
            builder.Append("type ").Append(inType.Name);
            if (inType.SuperType != null)
            {
                builder.Append(" extends ").Append(inType.SuperType);
            }
            builder.Append(" {")
                .Append("\n");

            // Write out constructors
            builder.Append("\n\t//Fields");
            foreach(var field in inType.Fields(null))
            {
                builder.Append("\n\t");
                DumpField(builder, field);
            }

            // Write out constructors
            builder.Append("\n\n\t//Constructors");
            foreach(var constructor in inType.Constructors(null))
            {
                builder.Append("\n\t");
                DumpMethod(builder, constructor);
            }

            // Write out constructors
            builder.Append("\n\n\t//Methods");
            foreach(var method in inType.Methods(null))
            {
                builder.Append("\n\t");
                DumpMethod(builder, method);
            }

            builder.Append("\n}");

            return builder.ToString();
        }

        static private void DumpMethod(StringBuilder inBuilder, TMethod inMethod)
        {
            if (inMethod.IsStatic())
                inBuilder.Append("static ");
            inBuilder.Append(inMethod.ReturnType).Append(" ").Append(inMethod.Name).Append('(');
            int paramCount = 0;
            foreach(var requiredParam in inMethod.RequiredParams)
            {
                if (paramCount > 0)
                    inBuilder.Append(", ");
                inBuilder.Append(requiredParam.ToTweedle());
                ++paramCount;
            }
            foreach(var optionalParam in inMethod.OptionalParams)
            {
                if (paramCount > 0)
                    inBuilder.Append(", ");
                inBuilder.Append(optionalParam.ToTweedle());
                ++paramCount;
            }
            inBuilder.Append(") [").Append(inMethod.Flags).Append("];");
        }

        static private void DumpField(StringBuilder inBuilder, TField inField)
        {
            if (inField.IsStatic())
                inBuilder.Append("static ");
            inBuilder.Append(inField.ToTweedle());
            inBuilder.Append(" [").Append(inField.Flags).Append("];");
        }

        #endregion // Debugging
    }
}
