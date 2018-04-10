namespace Alice.Tweedle
{
	public class TweedleConditionalStatement : TweedleStatement
	{
		private Tuple<bool, BlockStatement> ifElseIfs;
		private BlockStatement elseBody;

		public TweedleConditionalStatement(Tuple<bool, BlockStatement> ifElseIfs, BlockStatement elseBody)
		{
			this.ifElseIfs = ifElseIfs;
			this.elseBody = elseBody;
		}
	}
}