using Alice.VM;

namespace Alice.Tweedle
{
	public class ThisExpression : TweedleExpression
	{
		public ThisExpression()
			: base(null)
		{
		}

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			return new ValueStep("this", frame, frame.GetThis());
		}
	}
}