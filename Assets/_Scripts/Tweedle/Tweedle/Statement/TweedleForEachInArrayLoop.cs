namespace Alice.Tweedle
{
	public class TweedleForEachInArrayLoop : TweedleAbstractLoop
	{
		private TweedleField item;
		private TweedleExpression array;

		public TweedleForEachInArrayLoop(TweedleField item, TweedleExpression array, BlockStatement body) 
			: base(body)
		{
			this.item = item;
			this.array = array;
		}
	}
}