namespace Alice.Tweedle
{
    class PostfixDecrementExpression : PrePostfixExpression
    {

        public PostfixDecrementExpression(TweedleExpression expression)
            : base(expression)
        {
        }

        public override TweedleValue Evaluate(VM.TweedleFrame frame)
        {
            throw new System.NotImplementedException();
        }
    }
}