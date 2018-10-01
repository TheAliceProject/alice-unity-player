using System;
using System.Reflection;
using Alice.Tweedle.Interop;
using Alice.Tweedle.Parse;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Proxy/platform constant.
    /// </summary>
    public sealed class PConstant : TField
    {
        private readonly object m_Constant;
        private bool m_IsConverted;
        private TValue m_ConvertedValue;

        public PConstant(TAssembly inAssembly, string inName, Type inType, object inConstant)
        {
            m_Constant = inConstant;
            MemberFlags flags = MemberFlags.Field | MemberFlags.PInterop | MemberFlags.Readonly | MemberFlags.Static;
            
            TTypeRef tType = TInterop.TTypeFor(inType, inAssembly);
            SetupMember(inName, tType, flags);
        }

        #region TField

        public override TValue Get(ExecutionScope inScope, ref TValue inValue)
        {
            if (!m_IsConverted)
            {
                m_ConvertedValue = TInterop.ToTValue(m_Constant, inScope);
                m_IsConverted = true;
            }
            return m_ConvertedValue;
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
            throw new TweedleRuntimeException("Cannot set constant value " + Name);
        }

        #endregion // TField
    }
}