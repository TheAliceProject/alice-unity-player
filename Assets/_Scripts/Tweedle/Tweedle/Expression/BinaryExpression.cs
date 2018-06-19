using System;
using System.Collections.Generic;
using Alice.VM;

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

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			return new DoubleInputStep(lhs.AsStep(frame),
									   rhs.AsStep(frame),
									   (l, r) => Evaluate(l, r));
		}
		public TweedleValue EvaluateStep(TweedleValue left, TweedleValue right)
		{
			return Evaluate(left, right);
		}

		protected abstract TweedleValue Evaluate(TweedleValue left, TweedleValue right);
	}
}