namespace Alice.Tweedle
{
	public class AssignmentExpression : TweedleExpression
	{
		private TweedleExpression assigneeExp;
		private TweedleExpression valueExp;

		public AssignmentExpression(TweedleExpression assigneeExp, TweedleExpression valueExp)
			: base(valueExp.Type)
		{
			this.assigneeExp = assigneeExp;
			this.valueExp = valueExp;
		}

		public override void Evaluate(TweedleFrame frame)
		{
			valueExp.Evaluate(frame.ExecutionFrame(
				// TODO add assignment/write frame and handle on identifier and field
				value => assigneeExp.Evaluate(frame.ExecutionFrame(
					assignee =>
					{
						// TODO frame.SetValue(assignee, value);
						frame.Next();
					}
			))));
		}
	}
}