using Alice.Tweedle;

namespace Alice.VM
{
	public abstract class NotifyingEvaluationStep : NotifyingStep
	{
		protected TweedleValue result;
		public TweedleValue Result { get { return result; } }

		protected NotifyingEvaluationStep(TweedleFrame frame)
			: base(frame)
		{
		}

		internal TweedleValue EvaluateNow()
		{
			frame.vm.AddStep(this);
			return result;
		}
	}
}