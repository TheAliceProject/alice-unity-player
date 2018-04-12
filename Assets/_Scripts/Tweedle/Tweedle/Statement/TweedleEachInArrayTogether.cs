using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleEachInArrayTogether : TweedleAbstractStatementWithBody
	{
		private TweedleField item;
		private TweedleExpression array;

		public TweedleEachInArrayTogether(TweedleField item, TweedleExpression array, BlockStatement body) 
			: base(body)
		{
			this.item = item;
			this.array = array;
		}
	}
}