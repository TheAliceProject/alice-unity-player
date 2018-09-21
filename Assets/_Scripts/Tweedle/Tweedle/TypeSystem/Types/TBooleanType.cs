using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Boolean value type.
    /// </summary>
    public sealed class TBooleanType : TType
    {
        public TBooleanType()
            : base("Boolean")
        {
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
            return base.Equals(ref inValA, ref inValB)
                && ConvertToBoolean(ref inValA) == ConvertToBoolean(ref inValB);
        }

        public override bool LessThan(ref TValue inValA, ref TValue inValB)
        {
            AssertValueIsType(ref inValA);
            return (inValA.Type == inValB.Type)
                && !ConvertToBoolean(ref inValA) && ConvertToBoolean(ref inValB);
        }

        #endregion // Comparison Semantics

        #region Lifecycle

        public override TValue Instantiate()
        {
            return TValue.FALSE;
        }

        public TValue Instantiate(bool inbValue)
        {
            return TValue.FromBoolean(inbValue);
        }

        public override TValue DefaultValue()
        {
            return TValue.FALSE;
        }

        #endregion // Lifecycle

        #region Tweedle Casting

        #endregion // Tweedle Casting

        #region Conversion Semantics

        public override bool ConvertToBoolean(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue.RawNumber() != 0;
        }

        public override string ConvertToString(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue.RawNumber() != 0
                ? "true"
                : "false";
        }

        public override object ConvertToPObject(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return (object)(inValue.RawNumber() != 0 ? true : false);
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
            return inValue.RawNumber() != 0
                ? "true"
                : "false";
        }

        #endregion // Misc
    }
}