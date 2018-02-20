namespace Alice.Tweedle
{
    public class TweedleArray<T> : TweedleValue<TweedleArrayType<T>> where T: TweedleType
    {
        public int Length
        {
            get { return values.Length; }
        }

        public T this[int i]
        {
            get { return values[i]; }
        }

        public readonly T[] values;

        public TweedleArray(T[] values)
        {
            this.values = values;
        }
    }
}