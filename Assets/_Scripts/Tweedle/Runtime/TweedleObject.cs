namespace Alice.Tweedle
{
    public class TweedleObject : TweedleValue
    {
        private readonly TweedleClass tweClass;

        public TweedleObject(TweedleClass aClass)
        {
            tweClass = aClass;
        }

        public TweedleValue get(TweedleField field)
        {
            return null;
        }

        public void set(TweedleField field, TweedleValue value)
        {

        }

        public TweedleValue initializeField(VM.TweedleFrame frame, TweedleField field)
        {
            TweedleValue value = null;
            set(field, value);
            return value;
        }
    }
}