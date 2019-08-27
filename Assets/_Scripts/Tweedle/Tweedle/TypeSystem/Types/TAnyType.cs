using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Any type
    /// </summary>
    public sealed class TAnyType : TType
    {
        public TAnyType(TAssembly inAssembly)
            : base(inAssembly, "Any")
        {
        }

        #region Object Semantics

        public override TField Field(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            throw new TweedleNoMembersException(inName, this, "Field");
        }

        public override TMethod Method(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            throw new TweedleNoMembersException(inName, this, "Method");
        }

        public override TMethod Constructor(ExecutionScope inScope, NamedArgument[] inArguments)
        {
            throw new TweedleConstructorUnsupportedException(this);
        }

        public override bool IsReferenceType()
        {
            return true;
        }

        #endregion // Object Semantics

        #region Comparison Semantics

        #endregion // Comparison Semantics

        #region Lifecycle

        public override TValue Instantiate()
        {
            return TValue.NULL;
        }

        public override TValue DefaultValue()
        {
            return TValue.NULL;
        }

        #endregion // Lifecycle

        #region Tweedle Casting

        public override bool CanCast(TType inType)
        {
            return true;
        }

        #endregion // Tweedle Casting

        #region Conversion Semantics

        public override string ConvertToString(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);

            return null;
        }

        public override object ConvertToPObject(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);

            return null;
        }

        #endregion // Conversion Semantics

        #region Misc

        public override bool IsValidIdentifier()
        {
            return false;
        }

        public override string ToTweedle(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return "any";
        }

        public override int GetHashCode(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return 0;
        }

        #endregion // Misc
    }
}