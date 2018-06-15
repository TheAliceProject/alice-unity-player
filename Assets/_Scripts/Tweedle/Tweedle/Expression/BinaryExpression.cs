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

		public override void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			lhs.Evaluate(frame,
						 left => rhs.Evaluate(frame,
											  right => next(Evaluate(left, right))));
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			return new BinaryStep(this, lhs.AsStep(frame), rhs.AsStep(frame));
		}
		public TweedleValue EvaluateStep(TweedleValue left, TweedleValue right)
		{
			return Evaluate(left, right);
		}

		protected abstract TweedleValue Evaluate(TweedleValue left, TweedleValue right);
	}

	internal class BinaryStep : EvaluationStep
	{
		BinaryExpression expression;
		EvaluationStep lhs;
		EvaluationStep rhs;

		public BinaryStep(BinaryExpression expression, EvaluationStep lhs, EvaluationStep rhs)
			: base(new List<ExecutionStep> { lhs, rhs })
		{
			this.expression = expression;
			this.lhs = lhs;
			this.rhs = rhs;
		}

		internal override bool Execute()
		{
			result = expression.EvaluateStep(lhs.Result, rhs.Result);
			return MarkCompleted();
		}
	}
}