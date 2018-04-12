namespace Alice.Tweedle
{
    class PrefixDecrementExpression : PrePostfixExpression
    {

        public PrefixDecrementExpression(TweedleExpression expression)
            : base(expression)
        {
        }

        public override TweedleValue Evaluate(VM.TweedleFrame frame)
        {
            throw new System.NotImplementedException();
        }
    }
}