using System.Collections;
using System.Collections.Generic;
using Alice.Tweedle.VM;
using Alice.Utils;

namespace Alice.Tweedle
{
	/// <summary>
	/// Dictionary of fields to values.
	/// </summary>
	public sealed class TObject : IEnumerable<KeyValuePair<string, TValue>>
	{
        private Dictionary<string, ValueHolder> m_Attributes;

		/// <summary>
		/// Creates an empty object.
		/// </summary>
        public TObject()
		{
            m_Attributes = new Dictionary<string, ValueHolder>();
        }

		/// <summary>
		/// Returns if the given field has been defined.
		/// </summary>
        public bool Has(string inFieldName)
		{
            return m_Attributes.ContainsKey(inFieldName);
        }

		/// <summary>
		/// Attempts to retrieve the value for the given field.
		/// Returns if the field exists.
		/// </summary>
		public bool TryGet(string inFieldName, out TValue outValue)
		{
			ValueHolder holder;
            if (!m_Attributes.TryGetValue(inFieldName, out holder))
            {
                outValue = TValue.UNDEFINED;
                return false;
            }

            outValue = holder.Value;
            return true;
        }

		/// <summary>
		/// Retrieves the value for the given field.
		/// </summary>
		public TValue Get(string inFieldName)
		{
            ValueHolder holder;
			if (!m_Attributes.TryGetValue(inFieldName, out holder))
				throw new TweedleUninitializedFieldException(this, inFieldName);

            return holder.Value;
        }

		/// <summary>
		/// Sets the value for the given field.
		/// </summary>
		public void Set(string inFieldName, TValue inValue)
		{
            ValueHolder holder;
			if (!m_Attributes.TryGetValue(inFieldName, out holder))
			{
                holder = new ValueHolder(inValue.Type, inValue);
                m_Attributes.Add(inFieldName, holder);
            }
			else
			{
                holder.Value = inValue;
            }
        }

        /// <summary>
		/// Sets the value for the given field.
		/// </summary>
		public void Set(string inFieldName, TType inType, TValue inValue)
		{
            ValueHolder holder;
			if (!m_Attributes.TryGetValue(inFieldName, out holder))
			{
                holder = new ValueHolder(inType, inValue);
                m_Attributes.Add(inFieldName, holder);
            }
			else
			{
                holder.Value = inValue;
            }
        }

		/// <summary>
		/// Gets/sets the value for the given field.
		/// </summary>
        public TValue this[string inFieldName]
		{
            get { return Get(inFieldName); }
            set { Set(inFieldName, value); }
        }

		/// <summary>
		/// Deletes all fields.
		/// </summary>
		public void Clear()
		{
            m_Attributes.Clear();
        }

		#region IEnumerable

		public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            foreach(var kv in m_Attributes)
                yield return new KeyValuePair<string, TValue>(kv.Key, kv.Value.Value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

		#endregion // IEnumerable

		public override string ToString()
        {
            using(PooledStringBuilder psb = PooledStringBuilder.Alloc())
            {
                psb.Builder.Append('{');
                int valueCount = 0;
				foreach(var kv in m_Attributes)
				{
					if (valueCount > 0)
                        psb.Builder.Append(", ");

                    psb.Builder.Append(kv.Key)
                        .Append(": ")
                        .Append(kv.Value.ToString());

                    ++valueCount;
                }

                psb.Builder.Append('}');

                return psb.Builder.ToString();
            }
        }
    }
}