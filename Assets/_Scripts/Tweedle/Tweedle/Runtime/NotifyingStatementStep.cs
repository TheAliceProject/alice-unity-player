using Alice.VM;

namespace Alice.Tweedle
{
	public class NotifyingStatementStep<T> : NotifyingStep
		where T : TweedleStatement
	{
		protected T statement;

		public NotifyingStatementStep(T statement, TweedleFrame frame, NotifyingStep waiting)
			: base(frame, waiting)
		{
			this.statement = statement;
		}
	}
}