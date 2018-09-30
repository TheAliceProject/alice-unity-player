using System;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Integer value type.
    /// </summary>
    public sealed class TWholeNumberType : TType
    {
        private TValue m_Default;

        public TWholeNumberType(TAssembly inAssembly, TType inBase)
            : base(inAssembly, "WholeNumber", inBase)
        {
        }

        protected override void LinkImpl(TAssemblyLinkContext inContext)
        {
            base.LinkImpl(inContext);

            m_Default = TValue.FromInt(0);
        }

        #region Object Semantics

        public override TField Field(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            // TODO(Alex): Replace
            throw new System.NotImplementedException();
        }

        public override TMethod Method(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            // TODO(Alex): Replace
            throw new System.NotImplementedException();
        }

        public override TMethod Constructor(ExecutionScope inScope, NamedArgument[] inArguments)
        {
            // TODO(Alex): Replace
            throw new System.NotImplementedException();
        }

        public override bool IsReferenceType()
        {
            return false;
        }

        #endregion // Object Semantics

        #region Comparison Semantics

        public override bool Equals(ref TValue inValA, ref TValue inValB)
        {
            AssertValueIsType(ref inValA);

            if (inValB.Type == TBuiltInTypes.DECIMAL_NUMBER)
            {
                return ConvertToDouble(ref inValA) == inValB.ToDouble();
            }

            return base.Equals(ref inValA, ref inValB)
                && ConvertToInt(ref inValA) == ConvertToInt(ref inValB);
        }

        public override bool LessThan(ref TValue inValA, ref TValue inValB)
        {
            AssertValueIsType(ref inValA);

            if (inValB.Type == TBuiltInTypes.DECIMAL_NUMBER)
            {
                return ConvertToDouble(ref inValA) < inValB.ToDouble();
            }

            return (inValB.Type == inValB.Type)
                && ConvertToInt(ref inValA) < ConvertToInt(ref inValB);
        }

        #endregion // Comparison Semantics

        #region Lifecycle

        public override TValue Instantiate()
        {
            return m_Default;
        }

        public TValue Instantiate(int inValue)
        {
            return TValue.FromInt(inValue);
        }

        public override TValue DefaultValue()
        {
            return m_Default;
        }

        #endregion // Lifecycle

        #region Tweedle Casting

        public override bool CanCast(TType inType)
        {
            return inType == TBuiltInTypes.DECIMAL_NUMBER || base.CanCast(inType);
        }

        public override TValue Cast(ref TValue inValue, TType inType)
        {
            if (inType == TBuiltInTypes.DECIMAL_NUMBER)
            {
                return TValue.FromNumber(inValue.RawNumber());
            }

            return base.Cast(ref inValue, inType);
        }

        #endregion // Tweedle Casting

        #region Conversion Semantics

        public override int ConvertToInt(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return (int)inValue.RawNumber();
        }

        public override double ConvertToDouble(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue.RawNumber();
        }

        public override string ConvertToString(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return ((int)inValue.RawNumber()).ToString();
        }

        public override object ConvertToPObject(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return (object)((int)inValue.RawNumber());
        }

        public override Type GetPObjectType()
        {
            return typeof(int);
        }

        #endregion // Conversion Semantics

        #region Misc

        public override int GetHashCode(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return ((int)inValue.RawNumber()).GetHashCode();
        }

        public override string ToTweedle(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return ((int)inValue.RawNumber()).ToString();
        }

        #endregion // Misc
    }
}