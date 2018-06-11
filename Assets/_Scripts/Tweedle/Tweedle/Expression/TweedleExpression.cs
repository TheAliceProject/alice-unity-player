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

		public abstract void Evaluate(TweedleFrame frame);

		public TweedleValue EvaluateNow(TweedleFrame frame)
		{
			TweedleValue result = null;
			Evaluate(frame.ExecutionFrame(val => result = val));
			return result;
		}

		public TweedleValue EvaluateNow()
		{
			return EvaluateNow(new TweedleFrame(null));
		}

		internal virtual bool IsLiteral()
		{
			return false;
		}
	}
}