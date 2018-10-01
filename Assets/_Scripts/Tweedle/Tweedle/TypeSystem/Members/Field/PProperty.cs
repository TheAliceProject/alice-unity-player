using System.Reflection;
using Alice.Tweedle.Interop;
using Alice.Tweedle.Parse;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Proxy/platform property.
    /// </summary>
    public sealed class PProperty : TField
    {
        private PropertyInfo m_PropertyInfo;
        private bool m_IsStatic;

        public PProperty(TAssembly inAssembly, PropertyInfo inFieldInfo)
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

            TTypeRef tType = TInterop.TTypeFor(inFieldInfo.PropertyType, inAssembly);
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
            object newVal = TInterop.ToPObject(inNewValue, m_PropertyInfo.PropertyType, inScope);
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