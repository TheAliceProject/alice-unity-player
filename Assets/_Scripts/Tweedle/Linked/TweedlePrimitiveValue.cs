namespace Alice.Tweedle
{
    public class TweedlePrimitiveValue : TweedleValue
    {
        private TweedlePrimitiveType primitiveType;

        public override TweedleType Type
        {
            get
            {
                return primitiveType;
            }
        }
    }
}