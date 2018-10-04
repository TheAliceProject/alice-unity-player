using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Void type. Only used as a return type.
    /// </summary>
    public sealed class TVoidType : TType
    {
        public TVoidType(TAssembly inAssembly)
            : base(inAssembly, "void")
        {
        }

        #region Object Semantics

        public override TField Field(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            throw new TweedleNoMembersException(this, "Field");
        }

        public override TMethod Method(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            throw new TweedleNoMembersException(this, "Method");
        }

        public override TMethod Constructor(ExecutionScope inScope, NamedArgument[] inArguments)
        {
            throw new TweedleNoMembersException(this, "Constructor");
        }

        public override bool IsReferenceType()
        {
            throw new TweedleUnsupportedException(this, "IsReferenceType");
        }

        #endregion // Object Semantics

        #region Lifecycle

        public override TValue Instantiate()
        {
            throw new TweedleUnsupportedException(this, "Instantiate");
        }

        public override TValue DefaultValue()
        {
            throw new TweedleUnsupportedException(this, "DefaultValue");
        }

        #endregion // Lifecycle

        #region Tweedle Casting

        public override bool CanCast(TType inType)
        {
            return this == inType;
        }

        #endregion // Tweedle Casting

        #region Conversion Semantics

        #endregion // Conversion Semantics

        #region Misc

        public override bool IsValidIdentifier()
        {
            return false;
        }

        public override bool IsValidReturnType()
        {
            return true;
        }

        public override string ToTweedle(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return "void";
        }

        public override int GetHashCode(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return 0;
        }

        #endregion // Misc
    }
}