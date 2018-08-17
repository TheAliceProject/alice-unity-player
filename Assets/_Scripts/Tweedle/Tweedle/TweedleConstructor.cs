using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class TweedleConstructor : TweedleMethod
	{

		public TweedleConstructor(TweedleType type, string name, List<TweedleRequiredParameter> required, List<TweedleOptionalParameter> optional, List<TweedleStatement> body)
			: base(type, name, required, optional, body)
		{
		}

		public override bool IsStatic()
		{
			return true;
		}

		protected internal override void AddPrepSteps(InvocationScope scope, StepSequence main, Dictionary<string, TweedleExpression> arguments)
		{
			AddFieldSteps(scope, main);
			base.AddPrepSteps(scope, main, arguments);
		}

		void AddFieldSteps(ExecutionScope scope, StepSequence main)
		{
			foreach (ExecutionStep initFieldStep in ((TweedleObject)scope.GetThis()).InitializationNotifyingSteps(scope))
			{
				main.AddStep(initFieldStep);
			}
		}
	}
}