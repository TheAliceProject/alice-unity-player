using Alice.VM;

namespace Alice.Tweedle
{
	abstract class StatementStep<T> : ExecutionStep
		where T : TweedleStatement
	{
		protected T statement;
		protected TweedleFrame frame;

		public StatementStep(T statement, TweedleFrame frame, ExecutionStep blocker)
			: base(blocker)
		{
			this.statement = statement;
			this.frame = frame;
		}

		public StatementStep(T statement, TweedleFrame frame)
			: base()
		{
			this.statement = statement;
			this.frame = frame;
		}
	}
}