namespace Alice.Tweedle
{
    public class TweedleArray<T> where T : TweedleArrayType
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