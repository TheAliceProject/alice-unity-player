using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class DoInOrder : AbstractStatementWithBody
	{
        public DoInOrder(List<TweedleStatement> body) : base(body)
		{
		}

		public override void Execute(TweedleFrame frame)
		{
			throw new System.NotImplementedException();
		}
	}
}