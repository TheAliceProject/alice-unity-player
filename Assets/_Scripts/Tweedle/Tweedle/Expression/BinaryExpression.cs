﻿using Alice.VM;

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

		internal override NotifyingEvaluationStep AsStep(TweedleFrame frame)
		{
			return new DoubleInputEvalStep(ToTweedle(), frame, lhs, rhs, Evaluate);
		}

		protected abstract TweedleValue Evaluate(TweedleValue left, TweedleValue right);

		internal override string ToTweedle()
		{
			return lhs.ToTweedle() + " " + Operator() + " " + rhs.ToTweedle();
		}

		internal abstract string Operator();
	}
}