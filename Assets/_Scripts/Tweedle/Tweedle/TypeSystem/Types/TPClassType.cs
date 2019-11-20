using System;
using System.Collections.Generic;
using System.Reflection;
using Alice.Tweedle.Interop;

namespace Alice.Tweedle
{
    /// <summary>
    /// Proxy class type.
    /// </summary>
    public sealed class TPClassType : TTypeWithMembers
    {
        private Type m_Type;
        private bool m_IsModule;

        public TPClassType(TAssembly inAssembly, Type inType)
            : base(inAssembly, TInterop.InteropTypeName(inType), TInterop.TTypeFor(inType.BaseType, inAssembly))
        {
            m_Type = inType;
            m_IsModule = m_Type.IsAbstract && m_Type.IsSealed;

            AssignMembers(
                TInterop.GenerateFields(inAssembly, m_Type),
                TInterop.GenerateMethods(inAssembly, m_Type),
                TInterop.GenerateConstructors(inAssembly, m_Type)
            );
        }

        #region Object Semantics

        public override bool IsReferenceType()
        {
            return true;
        }

        #endregion // Object Semantics
        
        #region Link

        #endregion // Link

        #region Lifecycle

        public override bool HasDefaultInstantiate()
        {
            return false;
        }

        public override TValue Instantiate()
        {
            return TValue.UNDEFINED;
        }

        public override TValue DefaultValue()
        {
            return TValue.NULL;
        }

        #endregion // Lifecycle

        public override bool Equals(ref TValue inValA, ref TValue inValB)
        {
            return base.Equals(ref inValA, ref inValB)
                && inValA.Object() == inValB.Object();
        }

        public override bool LessThan(ref TValue inValA, ref TValue inValB)
        {
            return false;
        }

        #region Tweedle Casting

        #endregion // Tweedle Casting

        #region Conversion Semantics

        public override object ConvertToPObject(ref TValue inValue)
        {
            return inValue.RawObject<object>();
        }

        public override string ConvertToString(ref TValue inValue)
        {
            return inValue.RawObject<object>().ToString();
        }

        public override double ConvertToDouble(ref TValue inValue) {
            TMethod conversion = FindMethodWithArgsQuietly(m_Methods, "toDecimalNumber", new string[0], MemberFlags.Instance);
            if (conversion == null || !(conversion is PMethod)) {
                throw new TweedleRuntimeException("This type (" + this + ") cannot convert the value " + inValue + " to a double.");
            }
            return (double) ((PMethod)conversion).InvokeNow(inValue.RawObject<object>(), new object[0]);
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
            return Name;
        }

        #endregion // Misc
    }
}