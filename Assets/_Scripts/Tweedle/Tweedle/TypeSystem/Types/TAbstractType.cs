using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Abstract type.
    /// </summary>
    public class TAbstractType : TType
    {
        public TAbstractType(TAssembly inAssembly, string inName)
            : base(inAssembly, inName)
        {
        }

        public TAbstractType(TAssembly inAssembly, string inName, TType inBase)
            : base(inAssembly, inName, inBase)
        {
        }

        #region Object Semantics

        public override TField Field(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            throw new TweedleNoMembersException(inName, this, "Field");
        }

        public override TMethod Method(ExecutionScope inScope, ref TValue inValue, string inName, string[] inArgNames, MemberFlags inFlags = MemberFlags.None)
        {
            throw new TweedleNoMembersException(inName, this, "Method");
        }

        public override TMethod Constructor(ExecutionScope inScope, NamedArgument[] inArguments)
        {
            throw new TweedleConstructorUnsupportedException(this);
        }

        public override bool IsReferenceType()
        {
            return false;
        }

        #endregion // Object Semantics

        #region Comparison Semantics

        public override bool Equals(ref TValue inValA, ref TValue inValB)
        {
            throw new TweedleUnsupportedException(this, "Equals");
        }

        public override bool LessThan(ref TValue inValA, ref TValue inValB)
        {
            throw new TweedleUnsupportedException(this, "LessThan");
        }

        #endregion // Comparison Semantics

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
            throw new TweedleUnsupportedException(this, "CanCast");
        }

        #endregion // Tweedle Casting

        #region Conversion Semantics

        #endregion // Conversion Semantics

        #region Misc

        public override int GetHashCode(ref TValue inValue)
        {
            throw new TweedleUnsupportedException(this, "GetHashCode");
        }

        #endregion // Misc
    }
}