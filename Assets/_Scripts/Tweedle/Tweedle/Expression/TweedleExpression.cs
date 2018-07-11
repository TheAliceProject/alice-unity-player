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
			var expStep = AsStep(frame);
			var storeStep = new SingleInputActionNotificationStep(
					"EvaluateNow",
					frame,
					value => result = value);
			expStep.OnCompletionNotify(storeStep);
			expStep.EvaluateNow();
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

		internal abstract ExecutionStep AsStep(TweedleFrame frame);
	}
}