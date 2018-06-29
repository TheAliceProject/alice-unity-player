using Alice.VM;

namespace Alice.Tweedle
{
	public class ThisExpression : TweedleExpression
	{
		public ThisExpression()
			: base(null)
		{
		}

		internal override NotifyingEvaluationStep AsStep(NotifyingStep parent, TweedleFrame frame)
		{
			return new NotifyingValueStep(frame.StackWith("this"), frame, parent, frame.GetThis());
		}
	}
}