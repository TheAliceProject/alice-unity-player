using System;
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

		public abstract void Evaluate(TweedleFrame frame, Action<TweedleValue> next);

		public TweedleValue EvaluateNow(TweedleFrame frame)
		{
			return AsStep(frame).EvaluateNow();
		}

		public TweedleValue EvaluateNow()
		{
			return EvaluateNow(new TweedleFrame(null));
		}

		internal virtual bool IsLiteral()
		{
			return false;
		}

		internal abstract EvaluationStep AsStep(TweedleFrame frame);
	}
}