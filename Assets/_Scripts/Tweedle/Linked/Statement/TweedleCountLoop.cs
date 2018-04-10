namespace Alice.Tweedle
{
	public class TweedleCountLoop : TweedleAbstractLoop
	{
		private TweedleField variable;
		private TweedleExpression count;

		public TweedleCountLoop(TweedleField variable, TweedleExpression count, BlockStatement body) : base(body)
		{
			this.variable = variable;
			this.count = count;
		}
	}
}