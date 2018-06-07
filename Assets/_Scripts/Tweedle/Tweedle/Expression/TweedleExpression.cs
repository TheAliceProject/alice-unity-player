namespace Alice.Tweedle
{
	public abstract class TweedleExpression
	{
		public TweedleType Type
		{
			get { return type; }
		}

		TweedleType type;

		protected TweedleExpression()
		{
			this.type = null;
		}

		protected TweedleExpression(TweedleType type)
		{
			this.type = type;
		}

		public abstract void Evaluate(TweedleFrame frame);

		public TweedleValue EvaluateNow()
		{
			TweedleValue result = null;
			Evaluate(new TweedleFrame().ExecutionFrame(val => result = val));
			return result;
		}

		internal virtual bool IsLiteral()
		{
			return false;
		}
	}
}