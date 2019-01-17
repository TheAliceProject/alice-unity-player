namespace Alice.Tweedle
{
    /// <summary>
    /// Holder for a TValue.
    /// </summary>
    internal class ValueHolder
    {
        private TValue m_Value;

        /// <summary>
        /// Value type.
        /// </summary>
        public readonly TType Type;

        /// <summary>
        /// Current value.
        /// </summary>
        public TValue Value
        {
            get { return m_Value; }
            set
            {
                CheckType(Type, ref value);
                m_Value = value;
            }
        }

        /// <summary>
        /// Initializes a holder with a type and its default value.
        /// </summary>
        public ValueHolder(TType inType)
        {
            Type = inType;
            m_Value = inType.DefaultValue();
        }

        /// <summary>
        /// Initializes a holder with a type and a value.
        /// </summary>
        public ValueHolder(TType inType, TValue inValue)
        {
            CheckType(inType, ref inValue);
            Type = inType;
            m_Value = inValue;
        }

        /// <summary>
        /// Creates a duplicate value holder.
        /// </summary>
        public ValueHolder Clone()
        {
            return new ValueHolder(Type, m_Value);
        }

        private void CheckType(TType inType, ref TValue ioValue)
        {
            if (ioValue.Type == null || !ioValue.Type.CanCast(inType))
            {
                throw new TweedleRuntimeException("Unable to treat type " + ioValue.Type + " as type " + inType);
            }

            ioValue = ioValue.Type.Cast(ref ioValue, inType);
        }
    }
}