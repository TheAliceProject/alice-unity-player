namespace Alice.Tweedle
{
	abstract public class TweedleAbstractStatementWithBody : TweedleStatement
	{
		private BlockStatement body;

		public TweedleAbstractStatementWithBody(BlockStatement body)
		{
			this.body = body;
		}
	}
}