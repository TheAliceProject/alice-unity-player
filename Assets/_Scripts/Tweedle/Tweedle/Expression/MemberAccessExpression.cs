using System;

namespace Alice.Tweedle
{
	abstract public class MemberAccessExpression : TweedleExpression
	{
		public TweedleExpression Target { get; }

		public MemberAccessExpression(TweedleExpression target)
			: base(null)
		{
			Target = target;
		}

		public override void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			Target.Evaluate(frame, value => next(value));
		}
	}
}