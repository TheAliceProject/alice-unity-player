namespace Alice.Tweedle
{
    public class TweedleObject : TweedleValue<TweedleClass>
    {
        private readonly TweedleClass twClass;

        public TweedleObject(TweedleClass aClass)
        {
            twClass = aClass;
        }

        public TweedleValue<T> get<T>(TweedleField<T> field) where T : TweedleType
        {
            return null;
        }

        public void set<T>(TweedleField<T> field, TweedleValue<T> value) where T : TweedleType
        {

        }

        public TweedleValue<T> initializeField<T>(VM.TweedleFrame frame, TweedleField<T> field) where T : TweedleType
        {
            TweedleValue<T> value = null;
            set(field, value);
            return value;
        }
    }
}