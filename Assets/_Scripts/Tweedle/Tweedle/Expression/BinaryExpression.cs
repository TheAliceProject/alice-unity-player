namespace Alice.Tweedle
{
	public abstract class BinaryExpression : TweedleExpression
	{
		private TweedleExpression lhs;
		private TweedleExpression rhs;

		public BinaryExpression(TweedleExpression lhs, TweedleExpression rhs, TweedleType type)
			: base(type)
		{
			this.lhs = lhs;
			this.rhs = rhs;
		}

		public override void Evaluate(TweedleFrame frame)
		{
			lhs.Evaluate(frame.ExecutionFrame(
				left => rhs.Evaluate(frame.ExecutionFrame(
					right => frame.Next(Evaluate(left, right))
				))));
		}

		protected abstract TweedleValue Evaluate(TweedleValue left, TweedleValue right);
	}
}