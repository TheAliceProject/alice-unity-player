using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Alice.Tweedle.Parse;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public abstract partial class TType : IComparable<TType>, ILinkable
    {
        #region Enums

        private enum Status
        {
            Unlinked,
            Linked,
            PostLinked,
            Prepped
        }

        #endregion // Enums

        public readonly Guid ID = Guid.NewGuid();

        public readonly string Name;
        public readonly TTypeRef SuperType;
        public readonly TTypeRef SelfRef;

        private Status m_Status;
        private int m_InheritanceDepth;
        private TObject m_StaticStorage;

        #region Constructors

        protected TType(string inName) : this(inName, (TTypeRef)null) { }

        protected TType(string inName, TType inSuperType) : this(inName, (TTypeRef)inSuperType) { }

        protected TType(string inName, string inSuperTypeName) : this(inName, new TTypeRef(inSuperTypeName)) { }

        protected TType(string inName, TTypeRef inSuperTypeRef)
        {
            Name = inName;
            SuperType = inSuperTypeRef;
            SelfRef = new TTypeRef(this);
        }

        #endregion // Constructors

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

        public virtual bool CanInstantiate(ExecutionScope inScope) { return IsReferenceType(); }

        public virtual bool HasDefaultInstantiate() { return true; }
        public abstract TValue Instantiate();
        public abstract TValue DefaultValue();

        public virtual void AddInstanceInitializer(ConstructorScope inScope, StepSequence ioSteps)
        {
            // Default initialization steps for super class
            if (SuperType != null)
                SuperType.Get(inScope).AddInstanceInitializer(inScope, ioSteps);
        }

        #endregion // Lifecycle

        #region Statics

        protected virtual bool HasStaticConstructor() { return false; }

        /// <summary>
        /// Preps the type for execution.
        /// Returns any additional steps that need to be executed on the VM.
        /// </summary>
        public void Prep(out ITweedleExpression outAdditionalPrep)
        {
            if (m_Status == Status.Prepped)
            {
                outAdditionalPrep = null;
                return;
            }

            m_StaticStorage?.Clear();
            m_Status = Status.Prepped;

            if (HasStaticConstructor())
            {
                outAdditionalPrep = new StaticInstantiation(this.SelfRef);
            }
            else
            {
                outAdditionalPrep = null;
            }
        }

        public virtual void AddStaticInitializer(ExecutionScope inScope, StepSequence ioSteps)
        {
        }

        // Returns the backing object for static fields on this type
        internal TObject StaticStorage()
        {
            if (m_StaticStorage == null)
                m_StaticStorage = new TObject();
            return m_StaticStorage;
        }

        #endregion // Static Initialization

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

        #region Linker

        public void Link(TAssembly[] inAssemblies)
        {
            if (m_Status == Status.Unlinked)
            {
                LinkImpl(inAssemblies);
                m_Status = Status.Linked;
            }
        }

        protected virtual void LinkImpl(TAssembly[] inAssemblies)
        {
            if (SuperType != null)
            {
                SuperType.Resolve(inAssemblies);
            }
        }

        public void PostLink(TAssembly[] inAssemblies)
        {
            if (m_Status == Status.Linked)
            {
                PostLinkImpl(inAssemblies);
                m_Status = Status.PostLinked;
            }
        }

        protected virtual void PostLinkImpl(TAssembly[] inAssemblies)
        {
            m_InheritanceDepth = CalculateInheritanceDepth(this);
        }

        #endregion // Linker

        #region IComparable

        int IComparable<TType>.CompareTo(TType other)
        {
            if (m_InheritanceDepth < other.m_InheritanceDepth)
                return -1;
            if (m_InheritanceDepth > other.m_InheritanceDepth)
                return 1;
            return 0;
        }

        #endregion // IComparable
    }
}
