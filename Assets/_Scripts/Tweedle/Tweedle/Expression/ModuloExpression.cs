namespace Alice.Tweedle
{
    class ModuloExpression : BinaryExpression
    {

        public ModuloExpression(TweedleExpression lhs, TweedleExpression rhs)
            : base(lhs, rhs, TweedleTypes.WHOLE_NUMBER)
        {
        }

        protected override TweedleValue Evaluate(TweedleValue left, TweedleValue right)
        {
			return Eval((TweedlePrimitiveValue<int>)left, (TweedlePrimitiveValue<int>)right);
        }

		private TweedlePrimitiveValue<int> Eval(TweedlePrimitiveValue<int> left, TweedlePrimitiveValue<int> right)
		{
			return TweedleTypes.WHOLE_NUMBER.Instantiate(left.Value % right.Value);
		}
    }
}