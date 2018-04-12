namespace Alice.Tweedle
{
	public class TweedleWhileLoop : TweedleAbstractLoop
	{
		private TweedleExpression conditional;

		public TweedleWhileLoop(TweedleExpression conditional, BlockStatement body) : base(body)
		{
			this.conditional = conditional;
		}
	}
}