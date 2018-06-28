using Alice.Tweedle;

namespace Alice.VM
{
	abstract class NotifyingEvaluationStep : NotifyingStep
	{
		protected TweedleValue result;
		public TweedleValue Result { get { return result; } }

		protected NotifyingEvaluationStep(TweedleFrame frame, NotifyingStep waitingStep)
			: base(frame, waitingStep)
		{
		}

		internal TweedleValue EvaluateNow()
		{
			frame.vm.AddStep(this);
			return result;
		}
	}
}