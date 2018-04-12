namespace Alice.Tweedle
{
    class BitwiseNotExpression : PrePostfixExpression
    {

        public BitwiseNotExpression(TweedleExpression expression)
            : base(expression)
        {
        }

        public override TweedleValue Evaluate(VM.TweedleFrame frame)
        {
            throw new System.NotImplementedException();
        }
    }
}