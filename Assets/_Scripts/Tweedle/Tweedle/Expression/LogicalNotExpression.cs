using System;
using Alice.VM;

namespace Alice.Tweedle
{
	class LogicalNotExpression : TweedleExpression
	{
		TweedleExpression expression;

		public LogicalNotExpression(TweedleExpression expression)
			: base(TweedleTypes.BOOLEAN)
		{
			this.expression = expression;
		}

		public override void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			expression.Evaluate(frame, value => next(TweedleTypes.BOOLEAN.Instantiate(!value.ToBoolean())));
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			throw new NotImplementedException();
		}
	}
}