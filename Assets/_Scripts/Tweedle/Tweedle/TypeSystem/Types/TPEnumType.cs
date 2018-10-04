using System;
using Alice.Tweedle.Interop;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Proxy/platform enum type.
    /// </summary>
    public sealed class TPEnumType : TTypeWithMembers
    {
        private Type m_Type;

        public TPEnumType(TAssembly inAssembly, Type inType)
            : base(inAssembly, TInterop.InteropTypeName(inType))
        {
            m_Type = inType;

            Array values = Enum.GetValues(inType);
            TField[] enumFields = new TField[values.Length];

            for (int i = 0; i < values.Length; ++i)
            {
                var value = values.GetValue(i);
                enumFields[i] = new PConstant(inAssembly, value.ToString(), m_Type, value);
            }

            AssignMembers(enumFields, TMethod.EMPTY_ARRAY, TMethod.EMPTY_ARRAY);
        }

        #region Statics

        #endregion // Statics

        public override bool IsReferenceType() { return false; }

        #region Lifecycle

        public override bool CanInstantiate(ExecutionScope inScope)
        {
            return false;
        }

        public override TValue Instantiate()
        {
            return TValue.UNDEFINED;
        }

        public override TValue DefaultValue()
        {
            return TValue.UNDEFINED;
        }

        #endregion // Lifecycle

        public override bool Equals(ref TValue inValA, ref TValue inValB)
        {
            return base.Equals(ref inValA, ref inValB)
                && inValA.RawObject<object>() == inValB.RawObject<object>();
        }

        public override bool LessThan(ref TValue inValA, ref TValue inValB)
        {
            return inValA.Enum().Value < inValB.Enum().Value;
        }

        #region Tweedle Casting

        #endregion // Tweedle Casting

        #region Conversion Semantics

        public override int ConvertToInt(ref TValue inValue)
        {
            return (int)inValue.RawObject<object>();
        }

        public override string ConvertToString(ref TValue inValue)
        {
            return inValue.RawObject<object>().ToString();
        }

        public override object ConvertToPObject(ref TValue inValue)
        {
            return inValue.RawObject<object>();
        }

        public override Type GetPObjectType()
        {
            return m_Type;
        }

        #endregion // Conversion Semantics

        #region Misc

        public override bool IsValidIdentifier()
        {
            return true;
        }

        public override int GetHashCode(ref TValue inValue)
        {
            return inValue.RawObject<object>().GetHashCode();
        }

        public override string ToTweedle(ref TValue inValue)
        {
            return inValue.RawObject<object>().ToString();
        }

        #endregion // Misc
    }
}