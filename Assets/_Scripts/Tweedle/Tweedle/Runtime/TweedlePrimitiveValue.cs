namespace Alice.Tweedle
{
	public class TweedlePrimitiveValue<V> : TweedleValue
	{
		public V Value
		{
			get
			{
				return value;
			}
		}

		readonly V value;
		readonly TweedlePrimitiveType<V> type;

		public TweedlePrimitiveValue(V value, TweedlePrimitiveType<V> type)
			: base(type)
		{
			this.value = value;
			this.type = type;
		}

		internal override bool IsLiteral()
		{
			return true;
		}

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj) ||
				obj is TweedlePrimitiveValue<V> &&
				value.Equals(((TweedlePrimitiveValue<V>)obj).value) &&
				type.Equals(((TweedlePrimitiveValue<V>)obj).type);
		}

		public override int GetHashCode()
		{
			return 17 * value.GetHashCode() + type.GetHashCode();
			/*var hashCode = -222564697;
			hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(value);
			hashCode = hashCode * -1521134295 + EqualityComparer<TweedlePrimitiveType<T>>.Default.GetHashCode(type);
			return hashCode;*/
		}
	}
}