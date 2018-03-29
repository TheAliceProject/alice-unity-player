namespace Alice.Tweedle
{
    public class TweedleArray<T> : TweedleValue<TweedleArrayType> where T: TweedleType
    {
        public int Length
        {
            get { return values.Length; }
        }

        public T this[int i]
        {
            get { return values[i]; }
        }

        private readonly T[] values;

        public TweedleArray(T[] values)
        {
            this.values = values;
        }
    }
}