namespace Alice.Tweedle
{
    class LogicalNotExpression : TweedleExpression
    {
		private TweedleExpression expression;

        public LogicalNotExpression(TweedleExpression expression)
            : base(TweedleTypes.BOOLEAN)
        {
			this.expression = expression;
        }

        public override void Evaluate(TweedleFrame frame)
		{
			expression.Evaluate(frame.ExecutionFrame(
				value => frame.Next(TweedleTypes.BOOLEAN.Instantiate(!value.ToBoolean()))));
        }
    }
}