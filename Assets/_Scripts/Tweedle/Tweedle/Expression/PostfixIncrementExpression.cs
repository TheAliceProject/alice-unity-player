namespace Alice.Tweedle
{
    class PostfixIncrementExpression : PrePostfixExpression
    {

        public PostfixIncrementExpression(TweedleExpression expression)
            : base(expression)
        {
        }

        public override TweedleValue Evaluate(VM.TweedleFrame frame)
        {
            throw new System.NotImplementedException();
        }
    }
}