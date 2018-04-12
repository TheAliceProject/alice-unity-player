namespace Alice.Tweedle
{
    class LogicalNotExpression : PrePostfixExpression
    {

        public LogicalNotExpression(TweedleExpression expression)
            : base(expression)
        {
        }

        public override TweedleValue Evaluate(VM.TweedleFrame frame)
        {
            throw new System.NotImplementedException();
        }
    }
}