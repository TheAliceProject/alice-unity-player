using Alice.VM;

namespace Alice.Tweedle
{
	public class ThisExpression : TweedleExpression
	{
		public ThisExpression()
			: base(null)
		{
		}

		internal override NotifyingEvaluationStep AsStep(TweedleFrame frame)
		{
			return new NotifyingValueStep("this", frame, frame.GetThis());
		}
	}
}