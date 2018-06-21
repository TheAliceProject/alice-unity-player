using Alice.VM;

namespace Alice.Tweedle
{
	public class ReturnStatement : TweedleStatement
	{
		TweedleExpression expression;

		public TweedleExpression Expression
		{
			get { return expression; }
		}

		public TweedleType Type
		{
			get { return expression.Type; }
		}

		public ReturnStatement()
		{
			expression = TweedleNull.NULL;
		}

		public ReturnStatement(TweedleExpression expression)
		{
			this.expression = expression;
		}

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			return new SingleInputActionStep(
				expression.AsStep(frame),
				result => ((MethodFrame)frame).Return(result));
		}
	}
}