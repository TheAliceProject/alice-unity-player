namespace Alice.Tweedle
{
	public class ArrayIndexExpression : TweedleExpression
	{
		private TweedleExpression array;
		private TweedleExpression index;

		public ArrayIndexExpression(TweedleType type, TweedleExpression array, TweedleExpression index) 
			: base(type)
		{
			this.array = array;
			this.index = index;
		}

		public override TweedleValue Evaluate(TweedleFrame frame)
		{
			throw new System.NotImplementedException();
		}
	}
}