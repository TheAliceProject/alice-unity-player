using Alice.VM;

namespace Alice.Tweedle
{
	public class NotifyingStatementStep<T> : ExecutionStep
		where T : TweedleStatement
	{
		protected T statement;

		public NotifyingStatementStep(T statement, TweedleFrame frame, ExecutionStep next)
			: base(frame, next)
		{
			this.statement = statement;
		}
	}
}