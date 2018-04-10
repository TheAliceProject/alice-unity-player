namespace Alice.Tweedle
{
	public class TweedleReturnStatement : TweedleStatement
	{
		private TweedleType type;
		private TweedleExpression expression;

		public TweedleReturnStatement(TweedleType type, TweedleExpression expression)
		{
			this.type = type;
			this.expression = expression;
		}
	}
}