using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedlePrimitiveValue : TweedleValue
	{
		internal TweedlePrimitiveValue(TweedleType type) : base(type)
		{
		}
	}

	public class TweedlePrimitiveValue<T> : TweedlePrimitiveValue
	{
        public T Value
        {
            get
            {
                return value;
            }
        }

		private readonly T value;
        private readonly TweedlePrimitiveType<T> type;

		public TweedlePrimitiveValue(T value, TweedlePrimitiveType<T> type)
			: base(type)
		{
			this.value = value;
			this.type = type;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj) ||
				obj is TweedlePrimitiveValue<T> &&
				value.Equals(((TweedlePrimitiveValue<T>)obj).value) &&
				type.Equals(((TweedlePrimitiveValue<T>)obj).Type);
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