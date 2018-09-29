using System.Collections;
using System.Collections.Generic;
using Alice.Utils;

namespace Alice.Tweedle
{
    /// <summary>
    /// Fixed-length array of values.
    /// </summary>
	public sealed class TArray : IEnumerable<TValue>
	{
        private ValueHolder[] m_ValueHolders;

        /// <summary>
        /// Number of elements in the array.
        /// </summary>
        public int Length
		{
			get { return m_ValueHolders.Length; }
		}

        /// <summary>
        /// Gets/sets the value at the given index.
        /// </summary>
		public TValue this[int i]
		{
			get { return m_ValueHolders[i].Value; }
            set { m_ValueHolders[i].Value = value; }
        }

        /// <summary>
        /// Creates a new array with the given element type
        /// and copies values from the given list.
        /// </summary>
		public TArray(TType inElementType, List<TValue> inValues)
		{
            m_ValueHolders = new ValueHolder[inValues.Count];
            for (int i = 0; i < m_ValueHolders.Length; ++i)
			{
                ValueHolder holder = new ValueHolder(inElementType, inValues[i]);
                m_ValueHolders[i] = holder;
            }
		}

        /// <summary>
        /// Creates a new array with the given element type
        /// and copies values from the given array.
        /// </summary>
        public TArray(TType inElementType, TValue[] inValues)
        {
            m_ValueHolders = new ValueHolder[inValues.Length];
            for (int i = 0; i < m_ValueHolders.Length; ++i)
			{
                ValueHolder holder = new ValueHolder(inElementType, inValues[i]);
                m_ValueHolders[i] = holder;
            }
        }

        /// <summary>
        /// Creates a new array with the given element type
        /// and fills with the default value for the given type.
        /// </summary>
        public TArray(TType inElementType, int inLength)
        {
            m_ValueHolders = new ValueHolder[inLength];
            for (int i = 0; i < m_ValueHolders.Length; ++i)
            {
                ValueHolder holder = new ValueHolder(inElementType);
                m_ValueHolders[i] = holder;
            }
        }

		#region IEnumerable

        public IEnumerator<TValue> GetEnumerator()
        {
            for (int i = 0; i < m_ValueHolders.Length; ++i)
                yield return m_ValueHolders[i].Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }

		#endregion // IEnumerable

        public override string ToString()
        {
            using(PooledStringBuilder psb = PooledStringBuilder.Alloc())
            {
                psb.Builder.Append('[');
                for (int i = 0; i < m_ValueHolders.Length; ++i)
                {
                    if (i > 0)
                        psb.Builder.Append(", ");
                    psb.Builder.Append(m_ValueHolders[i].Value.ToString());
                }
                psb.Builder.Append(']');

                return psb.Builder.ToString();
            }
        }
    }
}