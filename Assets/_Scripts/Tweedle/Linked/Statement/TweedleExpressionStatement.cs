namespace Alice.Tweedle
{
	public class TweedleExpressionStatement : TweedleStatement
	{
		private TweedleExpression expression;

		public TweedleExpressionStatement(TweedleExpression expression)
		{
			this.expression = expression;
		}
	}
}