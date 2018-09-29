using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Type reference value type.
    /// </summary>
    public sealed class TTypeRefType : TType
    {
        public TTypeRefType(TAssembly inAssembly)
            : base(inAssembly, "TypeRef")
        {
        }

        #region Object Semantics

        public override TField Field(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            AssertValueIsType(ref inValue);

            // Swap from instance to static
            inFlags &= ~MemberFlags.Instance;
            inFlags |= MemberFlags.Static;

            TType realType = inValue.TypeRef().Get(inScope);
            return realType.Field(inScope, ref inValue, inName, inFlags);
        }

        public override TMethod Method(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            AssertValueIsType(ref inValue);

            // Swap from instance to static
            inFlags &= ~MemberFlags.Instance;
            inFlags |= MemberFlags.Static;

            TType realType = inValue.TypeRef().Get(inScope);
            return realType.Method(inScope, ref inValue, inName, inFlags);
        }

        public override TMethod Constructor(ExecutionScope inScope, NamedArgument[] inArguments)
        {
            // TODO(Alex): Replace
            throw new System.NotImplementedException();
        }

        public override bool IsReferenceType()
        {
            return true;
        }

        #endregion // Object Semantics

        #region Lifecycle

        public override TValue Instantiate()
        {
            // TODO(Alex): Replace
            throw new System.NotImplementedException();
        }

        public TValue Instantiate(string inTypeName)
        {
            return TValue.FromType(new TTypeRef(inTypeName));
        }

        public TValue Instantiate(TType inType)
        {
            return TValue.FromType(inType.SelfRef);
        }

        public TValue Instantiate(TTypeRef inTypeRef)
        {
            return TValue.FromType(inTypeRef);
        }

        public override TValue DefaultValue()
        {
            // TODO(Alex): Replace
            throw new System.NotImplementedException();
        }

        #endregion // Lifecycle

        public override bool Equals(ref TValue inValA, ref TValue inValB)
        {
            return base.Equals(ref inValA, ref inValB)
                && inValA.TypeRef().Name == inValB.TypeRef().Name;
        }

        public override bool LessThan(ref TValue inValA, ref TValue inValB)
        {
            return (inValA.Type == inValB.Type)
                && inValA.TypeRef().Name.CompareTo(inValB.TypeRef().Name) < 0;
        }

        #region Tweedle Casting

        #endregion // Tweedle Casting

        #region Conversion Semantics

        public override object ConvertToPObject(ref TValue inValue)
        {
            return inValue.TypeRef();
        }

        #endregion // Conversion Semantics

        #region Misc

        public override bool IsValidIdentifier()
        {
            return false;
        }

        public override int GetHashCode(ref TValue inValue)
        {
            return inValue.TypeRef().GetHashCode();
        }

        public override string ToTweedle(ref TValue inValue)
        {
            return inValue.TypeRef().Name;
        }

        #endregion // Misc
    }
}