using System;
using Alice.Tweedle.Interop;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Array type.
    /// </summary>
    public sealed class TArrayType : TType
    {
        private TField[] m_Fields;
        private TMethod[] m_Methods;

        public TArrayType(TAssembly inAssembly, TTypeRef inElementType)
            : base(inAssembly, inElementType.Name + "[]")
        {
            ElementType = inElementType;
            m_Fields = new TField[]
            {
                LengthField()
            };
            m_Methods = new TMethod[]
            {
                
            };
        }

        public readonly TTypeRef ElementType;

        private Type m_ConvertedType;

        #region Custom Members

        private TField LengthField()
        {
            return new TPropertyField("length", TBuiltInTypes.WHOLE_NUMBER, MemberFlags.Readonly, (ExecutionScope inScope, ref TValue inValue) =>
            {
                return TValue.FromInt(inValue.Array().Length);
            });
        }

        #endregion // Custom Members

        #region Link

        protected override void LinkImpl(TAssemblyLinkContext inContext)
        {
            base.LinkImpl(inContext);

            ElementType.Resolve(inContext);

            LinkMembers(m_Fields, inContext, this);
            LinkMembers(m_Methods, inContext, this);
        }

        #endregion // Link

        #region Object Semantics

        public override TField Field(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            return FindMember(m_Fields, inName, inFlags);
        }

        public override TMethod Method(ExecutionScope inScope, ref TValue inValue, string inName, string[] inArgNames, MemberFlags inFlags = MemberFlags.None)
        {
            return FindMember(m_Methods, inName, inFlags);
        }

        public override TMethod Constructor(ExecutionScope inScope, NamedArgument[] inArguments)
        {
            throw new TweedleRuntimeException("Array constructors are implemented through ArrayInitializer");
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

        #endregion // Object Semantics

        #region Comparison Semantics

        public override bool Equals(ref TValue inValA, ref TValue inValB)
        {
            return base.Equals(ref inValA, ref inValB)
                && inValA.Array() == inValB.Array();
        }

        public override bool LessThan(ref TValue inValA, ref TValue inValB)
        {
            return false;
        }

        #endregion // Comparison Semantics

        #region Lifecycle

        public override TValue Instantiate()
        {
            return TValue.NULL;
        }

        public TValue Instantiate(TValue[] inValues)
        {
            return TValue.FromObject(this, new TArray((TType)ElementType, inValues));
        }

        public TValue Instantiate(int inLength)
        {
            return TValue.FromObject(this, new TArray((TType)ElementType, inLength));
        }

        public override TValue DefaultValue()
        {
            return TValue.NULL;
        }

        #endregion // Lifecycle

        #region Tweedle Casting

        public override bool CanCast(TType inType)
        {
            if (base.CanCast(inType))
                return true;

            // Arrays are castable upwards
            // but this only affects perception, not behavior
            // ex.
            //      class A, class B : A
            //      B[] can be treated as A[], but it's still a B[];
            //      assigning an element of type A will cause a runtime error
            TArrayType arrType = inType as TArrayType;
            if (arrType != null)
            {
                TType elementType = ElementType.Get();
                TType otherElementType = arrType.ElementType.Get();

                return elementType.CanCast(otherElementType);
            }

            return false;
        }

        #endregion // Tweedle Casting

        #region Conversion Semantics

        public override string ConvertToString(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue.Array().ToString();
        }

        public override object ConvertToPObject(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue;
        }

        public override Type GetPObjectType()
        {
            if (m_ConvertedType == null)
            {
                Type elementType = ElementType.Get().GetPObjectType();
                m_ConvertedType = elementType.MakeArrayType();
            }
            return m_ConvertedType;
        }

        #endregion // Conversion Semantics

        #region Misc

        public override int GetHashCode(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue.Array().GetHashCode();
        }

        public override string ToTweedle(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue.Array().ToString();
        }

        #endregion // Misc
    }
}