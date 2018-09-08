using Alice.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Hard-coded property on a type, with getter and optional setter.
    /// Can be either instance or static.
    /// </summary>
    public sealed class TPropertyField : TField
    {
        public delegate TValue GetDelegate(ExecutionScope inScope, ref TValue inValue);
        public delegate void SetDelegate(ExecutionScope inScope, ref TValue inValue, TValue inNewValue);

        private GetDelegate m_Getter;
        private SetDelegate m_Setter;

        public TPropertyField(string inName, TTypeRef inType, MemberFlags inFlags, GetDelegate inGetter, SetDelegate inSetter = null)
        {
            m_Getter = inGetter;
            m_Setter = inSetter;

            MemberFlags flags = MemberFlags.None;
            if (m_Setter == null)
                flags |= MemberFlags.Readonly;
            if ((inFlags & MemberFlags.Static) == 0)
                flags |= MemberFlags.Instance;

            SetupMember(inName, inType, flags);
        }

        #region TField

        public override TValue Get(ExecutionScope inScope, ref TValue inValue)
        {
            if (m_Getter == null)
                throw new TweedleRuntimeException("No getter provided for property " + Name);

            return m_Getter(inScope, ref inValue);
        }

        public override ExecutionStep InitializeStep(ExecutionScope inScope, ref TValue inValue)
        {
            return null;
        }

        public override void Set(ExecutionScope inScope, ref TValue inValue, TValue inNewValue)
        {
            CheckSet(inScope, ref inNewValue);
            
            if (m_Setter == null)
                throw new TweedleRuntimeException("No setter provided for property " + Name);

            m_Setter(inScope, ref inNewValue, inNewValue);
        }

        #endregion // TField
    }
}