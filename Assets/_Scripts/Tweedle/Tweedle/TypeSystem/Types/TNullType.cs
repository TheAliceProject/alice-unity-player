using System;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Null reference value type.
    /// </summary>
    public sealed class TNullType : TType
    {
        public TNullType(TAssembly inAssembly)
            : base(inAssembly, "null")
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
            // Null can be used to represent reference-type values
            // but not value-type values (int, double, bool)
            return inType.IsReferenceType();
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

        public override Type GetPObjectType()
        {
            return typeof(object);
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
            return "null";
        }

        public override int GetHashCode(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return 0;
        }

        #endregion // Misc
    }
}