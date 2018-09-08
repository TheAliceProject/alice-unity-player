using Alice.VM;

namespace Alice.Tweedle
{
	public abstract class NegativeExpression : UnaryExpression
	{
		protected ITweedleExpression expression;

		internal NegativeExpression(TType type, ITweedleExpression expression)
			: base(type)
		{
			this.expression = expression;
		}

		public override ExecutionStep AsStep(ExecutionScope scope)
		{
			var val = expression.AsStep(scope);
			val.OnCompletionNotify(new ValueComputationStep("-" + expression.ToTweedle(), scope, Negate));
			return val;
		}

		public override TValue EvaluateLiteral()
		{
            return Negate((TValue)expression);
        }

		internal abstract TValue Negate(TValue value);

		public override string ToTweedle()
		{
			return "-" + expression.ToTweedle();
		}
	}

	public class NegativeWholeExpression : NegativeExpression
	{

		public NegativeWholeExpression(ITweedleExpression expression)
			: base(TStaticTypes.WHOLE_NUMBER, expression)
		{
		}

		internal override TValue Negate(TValue value)
		{
			return TStaticTypes.WHOLE_NUMBER.Instantiate(0 - value.ToInt());
		}
	}

	public class NegativeDecimalExpression : NegativeExpression
	{

		public NegativeDecimalExpression(ITweedleExpression expression)
			: base(TStaticTypes.DECIMAL_NUMBER, expression)
		{
		}

		internal override TValue Negate(TValue value)
		{
			return TStaticTypes.DECIMAL_NUMBER.Instantiate(0 - value.ToDouble());
		}
	}
}