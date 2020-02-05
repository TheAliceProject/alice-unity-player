using System.Reflection;
using Alice.Tweedle.Interop;
using Alice.Tweedle.Parse;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Proxy/platform field.
    /// </summary>
    public sealed class PField : TField
    {
        private FieldInfo m_FieldInfo;
        private bool m_IsStatic;

        public PField(TAssembly inAssembly, FieldInfo inFieldInfo)
        {
            m_FieldInfo = inFieldInfo;
            m_IsStatic = inFieldInfo.IsStatic;

            MemberFlags flags = MemberFlags.Field | MemberFlags.PInterop;
            if (m_IsStatic)
                flags |= MemberFlags.Static;
            else
                flags |= MemberFlags.Instance;

            if (inFieldInfo.IsInitOnly)
                flags |= MemberFlags.Readonly;

            TTypeRef tType = TInterop.TTypeFor(inFieldInfo.FieldType, inAssembly);
            SetupMember(inFieldInfo.Name, tType, flags);
        }

        #region TField

        public override TValue Get(ExecutionScope inScope, ref TValue inValue)
        {
            object retVal;
            if (m_IsStatic)
            {
                retVal = m_FieldInfo.GetValue(null);
            }
            else
            {
                retVal = m_FieldInfo.GetValue(inValue.ToPObject());
            }

            return TInterop.ToTValue(retVal, inScope);
        }

        public override bool HasInitializer()
        {
            return false;
        }

        public override ExecutionStep InitializeStep(ExecutionScope inScope, ref TValue inValue)
        {
            return null;
        }

        public override void Set(ExecutionScope inScope, ref TValue inValue, TValue inNewValue)
        {
            CheckSet(inScope, ref inValue, ref inNewValue);
            object newVal = TInterop.ToPObject(inNewValue, m_FieldInfo.FieldType, inScope);
            if (m_IsStatic)
            {
                m_FieldInfo.SetValue(null, newVal);
            }
            else
            {
                m_FieldInfo.SetValue(inValue.ToPObject(), newVal);
            }
        }

        #endregion // TField
    }
}