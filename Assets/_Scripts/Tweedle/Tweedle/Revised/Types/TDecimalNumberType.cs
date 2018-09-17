namespace Alice.Tweedle
{
    /// <summary>
    /// Double floating-point value type.
    /// </summary>
    public sealed class TDecimalNumberType : TType
    {
        private TValue m_Default;

        public TDecimalNumberType(TType inBase)
            : base("DecimalNumber", inBase)
        {
        }

        protected override void LinkImpl(Parse.TweedleSystem inSystem)
        {
            base.LinkImpl(inSystem);

            m_Default = TValue.FromNumber(double.NaN);
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

            if (inValB.Type == TStaticTypes.WHOLE_NUMBER)
            {
                return ConvertToDouble(ref inValA) == inValB.ToDouble();
            }

            return base.Equals(ref inValA, ref inValB)
                && ConvertToDouble(ref inValA) == ConvertToDouble(ref inValB);
        }

        public override bool LessThan(ref TValue inValA, ref TValue inValB)
        {
            AssertValueIsType(ref inValA);

            if (inValB.Type == TStaticTypes.WHOLE_NUMBER)
            {
                return ConvertToDouble(ref inValA) < inValB.ToDouble();
            }

            return (inValA.Type == inValB.Type)
                && ConvertToDouble(ref inValA) < ConvertToDouble(ref inValB);
        }

        #endregion // Comparison Semantics

        #region Lifecycle

        public override TValue Instantiate()
        {
            return m_Default;
        }

        public TValue Instantiate(double inValue)
        {
            return TValue.FromNumber(inValue);
        }

        public override TValue DefaultValue()
        {
            return m_Default;
        }

        #endregion // Lifecycle

        #region Tweedle Casting

        #endregion // Tweedle Casting

        #region Conversion Semantics

        public override double ConvertToDouble(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue.RawNumber();
        }

        public override string ConvertToString(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue.RawNumber().ToString();
        }

        public override object ConvertToPObject(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return (object)(inValue.RawNumber());
        }

        #endregion // Conversion Semantics

        #region Misc

        public override int GetHashCode(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue.RawNumber().GetHashCode();
        }

        public override string ToTweedle(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue.RawNumber().ToString();
        }

        #endregion // Misc
    }
}