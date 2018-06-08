namespace Alice.Tweedle
{
    class LogicalOrExpression : BinaryExpression
    {

        public LogicalOrExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs, TweedleTypes.BOOLEAN)
        {
        }

		protected override TweedleValue Evaluate(TweedleValue left, TweedleValue right)
		{
			return Eval((TweedlePrimitiveValue<bool>)left, (TweedlePrimitiveValue<bool>)right);
		}

		private TweedlePrimitiveValue<bool> Eval(TweedlePrimitiveValue<bool> left, TweedlePrimitiveValue<bool> right)
		{
			return TweedleTypes.BOOLEAN.Instantiate(left.Value || right.Value);
		}
	}
}