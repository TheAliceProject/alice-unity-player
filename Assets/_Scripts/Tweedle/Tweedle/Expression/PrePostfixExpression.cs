namespace Alice.Tweedle
{
    public abstract class PrePostfixExpression : TweedleExpression
    {
        private TweedleExpression expression;

        public PrePostfixExpression(TweedleExpression expression)
        {
            this.expression = expression;
        }
    }
}