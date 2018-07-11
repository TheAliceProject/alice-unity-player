using Alice.VM;

namespace Alice.Tweedle
{
	// TODO Abstract more of the looping behavior from the subclasses: While, Count, and Array
	public class LoopStep<T> : ExecutionStep
		where T : AbstractLoop
	{
		protected T statement;

		public LoopStep(T statement, TweedleFrame frame, ExecutionStep next)
			: base(frame, next)
		{
			this.statement = statement;
		}
	}
}