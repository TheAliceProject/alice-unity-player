namespace Alice.Tweedle
{
    public class TweedleConstructorUnsupportedException : TweedleRuntimeException
    {
        public TweedleConstructorUnsupportedException(TType type)
            :base("No constructors on type " + type.Name + ".")
        {
        }
    }
}