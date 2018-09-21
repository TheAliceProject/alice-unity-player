using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
	public abstract class BinaryExpression : TweedleExpression
	{
		ITweedleExpression lhs;
		ITweedleExpression rhs;

		public BinaryExpression(ITweedleExpression lhs, ITweedleExpression rhs, TType type)
			: base(type)
		{
			this.lhs = lhs;
			this.rhs = rhs;
		}

		public override ExecutionStep AsStep(ExecutionScope scope)
		{
			return new TwoValueComputationStep(ToTweedle(), scope, lhs, rhs, Evaluate);
		}

		public TValue EvaluateLiteral()
		{
            return Evaluate((TValue)lhs, (TValue)rhs);
        }

		protected abstract TValue Evaluate(TValue left, TValue right);

		public override string ToTweedle()
		{
			return lhs.ToTweedle() + " " + Operator() + " " + rhs.ToTweedle();
		}

		internal abstract string Operator();
	}
}