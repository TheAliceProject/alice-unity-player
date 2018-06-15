using System;
using Alice.VM;

namespace Alice.Tweedle
{
	public abstract class NegativeExpression : TweedleExpression
	{
		protected TweedleExpression expression;

		internal NegativeExpression(TweedleType type, TweedleExpression expression)
			: base(type)
		{
			this.expression = expression;
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			return new NegativeStep(this, expression.AsStep(frame));
		}

		internal abstract TweedleValue Negate(TweedleValue value);
	}

	public class NegativeWholeExpression : NegativeExpression
	{

		public NegativeWholeExpression(TweedleExpression expression)
			: base(TweedleTypes.WHOLE_NUMBER, expression)
		{
		}

		public override void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			expression.Evaluate(frame, value => next(Negate(value)));
		}

		internal override TweedleValue Negate(TweedleValue value)
		{
			return TweedleTypes.WHOLE_NUMBER.Instantiate(0 - value.ToInt());
		}
	}

	public class NegativeDecimalExpression : NegativeExpression
	{

		public NegativeDecimalExpression(TweedleExpression expression)
			: base(TweedleTypes.DECIMAL_NUMBER, expression)
		{
		}

		public override void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			expression.Evaluate(frame, value => next(Negate(value)));
		}

		internal override TweedleValue Negate(TweedleValue value)
		{
			return TweedleTypes.DECIMAL_NUMBER.Instantiate(0 - value.ToDouble());
		}
	}

	// TODO extract this and change Negate to a callback on the expression
	internal class NegativeStep : EvaluationStep
	{
		NegativeExpression expression;
		EvaluationStep expressionStep;

		public NegativeStep(NegativeExpression expression, EvaluationStep expressionStep)
			: base(expressionStep)
		{
			this.expression = expression;
			this.expressionStep = expressionStep;
		}

		internal override bool Execute()
		{
			result = expression.Negate(expressionStep.Result);
			return MarkCompleted();
		}
	}
}