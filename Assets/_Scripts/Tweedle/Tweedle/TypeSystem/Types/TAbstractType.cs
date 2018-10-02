using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Abstract type.
    /// </summary>
    public class TAbstractType : TType
    {
        public TAbstractType(string inName)
            : base(inName)
        {
        }

        public TAbstractType(string inName, TType inBase)
            : base(inName, inBase)
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
            // TODO(Alex): Replace
            throw new System.NotImplementedException();
        }

        public override bool LessThan(ref TValue inValA, ref TValue inValB)
        {
            // TODO(Alex): Replace
            throw new System.NotImplementedException();
        }

        #endregion // Comparison Semantics

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
            return true;
        }

        public override int GetHashCode(ref TValue inValue)
        {
            // TODO(Alex): Replace
            throw new System.NotImplementedException();
        }

        #endregion // Misc
    }
}