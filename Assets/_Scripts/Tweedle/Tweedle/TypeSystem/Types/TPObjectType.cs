using System;
using System.Collections.Generic;
using System.Reflection;
using Alice.Tweedle.Interop;

namespace Alice.Tweedle
{
    /// <summary>
    /// Proxy class type.
    /// </summary>
    public sealed class TPObjectType : TTypeWithMembers
    {
        private Type m_Type;
        private bool m_IsModule;

        public TPObjectType(Type inType)
            : base(TInterop.TTypeNameForType(inType), TInterop.TTypeFor(inType.BaseType))
        {
            m_Type = inType;
            m_IsModule = m_Type.IsAbstract && m_Type.IsSealed;

            AssignMembers(
                TInterop.GenerateFields(m_Type),
                TInterop.GenerateMethods(m_Type),
                TInterop.GenerateConstructors(m_Type)
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