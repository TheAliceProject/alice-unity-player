using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
	public class ThisExpression : TweedleExpression
	{
		public ThisExpression()
			: base(null)
		{
		}

		public override ExecutionStep AsStep(ExecutionScope scope)
		{
			return new ValueStep("this", scope, scope.GetThis());
		}
	}
}