using System.Reflection;
using Alice.Tweedle.Interop;
using Alice.Tweedle.Parse;
using Alice.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Proxy/platform constant.
    /// </summary>
    public sealed class PConstant : TField
    {
        private readonly object m_Constant;
        
        public PConstant(string inName, object inConstant)
        {
            m_Constant = inConstant;
            MemberFlags flags = MemberFlags.Field | MemberFlags.PInterop | MemberFlags.Readonly | MemberFlags.Static;
            
            TTypeRef tType = TInterop.TTypeFor(inConstant?.GetType());
            SetupMember(inName, tType, flags);
        }

        #region TField

        public override TValue Get(ExecutionScope inScope, ref TValue inValue)
        {
            return TInterop.ToTValue(m_Constant, inScope.vm.Library);
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