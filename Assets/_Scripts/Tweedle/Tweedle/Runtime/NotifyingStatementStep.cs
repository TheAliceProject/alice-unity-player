using Alice.VM;

namespace Alice.Tweedle
{
	public class NotifyingStatementStep<T> : NotifyingStep
		where T : TweedleStatement
	{
		protected T statement;

		public NotifyingStatementStep(T statement, TweedleFrame frame, NotifyingStep next)
			: base(frame, next)
		{
			this.statement = statement;
		}
	}
}