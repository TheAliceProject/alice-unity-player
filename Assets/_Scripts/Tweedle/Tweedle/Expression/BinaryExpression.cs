using System;

namespace Alice.Tweedle
{
	public abstract class BinaryExpression : TweedleExpression
	{
		TweedleExpression lhs;
		TweedleExpression rhs;

		public BinaryExpression(TweedleExpression lhs, TweedleExpression rhs, TweedleType type)
			: base(type)
		{
			this.lhs = lhs;
			this.rhs = rhs;
		}

		public override void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			lhs.Evaluate(frame,
						 left => rhs.Evaluate(frame,
											  right => next(Evaluate(left, right))));
		}

		protected abstract TweedleValue Evaluate(TweedleValue left, TweedleValue right);
	}
}