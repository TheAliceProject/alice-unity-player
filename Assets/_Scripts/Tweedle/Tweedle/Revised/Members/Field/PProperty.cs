using System.Reflection;
using Alice.Tweedle.Interop;
using Alice.Tweedle.Parse;
using Alice.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Proxy/platform property.
    /// </summary>
    public sealed class PProperty : TField
    {
        private PropertyInfo m_PropertyInfo;
        private bool m_IsStatic;

        public PProperty(PropertyInfo inFieldInfo)
        {
            m_PropertyInfo = inFieldInfo;
            m_IsStatic = inFieldInfo.GetGetMethod().IsStatic;

            MemberFlags flags = MemberFlags.Field | MemberFlags.PInterop;
            if (m_IsStatic)
                flags |= MemberFlags.Static;
            else
                flags |= MemberFlags.Instance;

            if (!inFieldInfo.CanWrite)
                flags |= MemberFlags.Readonly;

            TTypeRef tType = TConvert.TTypeFor(inFieldInfo.PropertyType);
            SetupMember(inFieldInfo.Name, tType, flags);
        }

        #region TField

        public override TValue Get(ExecutionScope inScope, ref TValue inValue)
        {
            object retVal;
            if (m_IsStatic)
            {
                retVal = m_PropertyInfo.GetValue(null);
            }
            else
            {
                retVal = m_PropertyInfo.GetValue(inValue.ToPObject());
            }

            return TConvert.ToTValue(retVal, inScope.vm.Library);
        }

        public override ExecutionStep InitializeStep(ExecutionScope inScope, ref TValue inValue)
        {
            return null;
        }

        public override void Set(ExecutionScope inScope, ref TValue inValue, TValue inNewValue)
        {
            CheckSet(inScope, ref inNewValue);
            object newVal = TConvert.ToPObject(inNewValue, m_PropertyInfo.PropertyType);
            if (m_IsStatic)
            {
                m_PropertyInfo.SetValue(null, newVal);
            }
            else
            {
                m_PropertyInfo.SetValue(inValue.ToPObject(), newVal);
            }
        }

        #endregion // TField
    }
}