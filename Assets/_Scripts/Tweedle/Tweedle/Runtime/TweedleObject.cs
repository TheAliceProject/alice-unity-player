namespace Alice.Tweedle
{
    public class TweedleObject : TweedleValue
    {
        private readonly TweedleClass tweClass;

        public TweedleObject(TweedleClass aClass)
			: base(aClass)
        {
            tweClass = aClass;
        }

        public TweedleValue Get(TweedleField field)
        {
            return null;
        }

        public void Set(TweedleField field, TweedleValue value)
        {

        }

        public TweedleValue InitializeField(VM.TweedleFrame frame, TweedleField field)
        {
            TweedleValue value = null;
            Set(field, value);
            return value;
        }
    }
}