using Alice.VM;

namespace Alice.Tweedle
{
	public abstract class TweedleExpression
	{
		public TweedleType Type { get; }

		protected TweedleExpression()
		{
			Type = null;
		}

		protected TweedleExpression(TweedleType type)
		{
			Type = type;
		}

		public TweedleValue EvaluateNow(TweedleFrame frame)
		{
			TweedleValue result = null;
			AsStep(
				new SingleInputActionNotificationStep(
					"EvaluateNow",
					frame,
					null,
					value => result = value),
				frame).EvaluateNow();
			return result;
		}

		public TweedleValue EvaluateNow()
		{
			return EvaluateNow(new TweedleFrame("EvaluateNow"));
		}

		internal virtual bool IsLiteral()
		{
			return false;
		}

		internal virtual string ToTweedle()
		{
			// TODO Override in all subclasses and make this abstract
			return ToString();
		}

		internal abstract EvaluationStep AsStep(TweedleFrame frame);

		internal abstract NotifyingEvaluationStep AsStep(NotifyingStep parent, TweedleFrame frame);

		internal virtual NotifyingEvaluationStep AsStep(NotifyingStep parent)
		{
			return AsStep(parent, parent.frame);
		}

		internal void AddStep(NotifyingStep parent, TweedleFrame frame)
		{
			frame.vm.AddStep(AsStep(parent, frame));
		}
	}
}