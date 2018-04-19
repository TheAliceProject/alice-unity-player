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

        public override TweedleValue Evaluate(VM.TweedleFrame frame)
        {
			TweedlePrimitiveValue<bool> eval = (TweedlePrimitiveValue<bool>)expression.Evaluate(frame);
			return TweedleTypes.BOOLEAN.Instantiate(!eval.Value);
        }
    }
}