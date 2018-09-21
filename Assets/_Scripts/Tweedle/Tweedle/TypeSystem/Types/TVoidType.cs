using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Void type. Only used as a return type.
    /// </summary>
    public sealed class TVoidType : TType
    {
        public TVoidType()
            : base("void")
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
            // TODO(Alex): Replace
            throw new System.NotImplementedException();
        }

        #endregion // Object Semantics

        #region Lifecycle

        public override TValue Instantiate()
        {
            // TODO(Alex): Replace
            throw new System.NotImplementedException();
        }

        public override TValue DefaultValue()
        {
            // TODO(Alex): Replace
            throw new System.NotImplementedException();
        }

        #endregion // Lifecycle

        #region Tweedle Casting

        public override bool CanCast(TType inType)
        {
            // TODO(Alex): Replace
            throw new System.NotImplementedException();
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