using System;
using Alice.VM;

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

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			throw new NotImplementedException();
		}
	}
}