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

		protected internal override void AddPrepSteps(InvocationFrame frame, StepSequence main, Dictionary<string, TweedleExpression> arguments)
		{
			AddFieldSteps(frame, main);
			base.AddPrepSteps(frame, main, arguments);
		}

		void AddFieldSteps(TweedleFrame frame, StepSequence main)
		{
			foreach (ExecutionStep initFieldStep in ((TweedleObject)frame.GetThis()).InitializationNotifyingSteps(frame))
			{
				main.AddStep(initFieldStep);
			}
		}
	}
}