namespace Alice.Tweedle
{
	abstract public class MemberAccessExpression : TweedleExpression
	{
		TweedleExpression target;

		public TweedleExpression Target
		{
			get { return target; }
		}

		public MemberAccessExpression(TweedleExpression target)
			: base(null)
		{
			this.target = target;
		}

		public override void Evaluate(TweedleFrame frame)
		{
			target.Evaluate(frame.ExecutionFrame(
				value => frame.Next(value)));
		}
	}
}