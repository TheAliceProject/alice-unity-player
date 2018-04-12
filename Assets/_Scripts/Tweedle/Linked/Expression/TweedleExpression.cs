namespace Alice.Tweedle
{
    public abstract class TweedleExpression
    {
        public abstract TweedleValue Evaluate(VM.TweedleFrame frame);
    }
}