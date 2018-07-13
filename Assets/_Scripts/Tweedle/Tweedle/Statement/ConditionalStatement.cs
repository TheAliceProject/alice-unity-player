using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class ConditionalStatement : TweedleStatement
	{
		public TweedleExpression Condition { get; }
		public BlockStatement ThenBody { get; }
		public BlockStatement ElseBody { get; }

		public ConditionalStatement(TweedleExpression condition, List<TweedleStatement> thenBody, List<TweedleStatement> elseBody)
		{
			Condition = condition;
			ThenBody = new BlockStatement(thenBody);
			ElseBody = new BlockStatement(elseBody);
		}

		internal override ExecutionStep AsStepToNotify(TweedleFrame frame, ExecutionStep next)
		{
			var conditionStep = Condition.AsStep(frame);
			var bodyStep = new OperationStep(
					"if " + Condition.ToTweedle(),
					frame,
					value => (value.ToBoolean() ? ThenBody : ElseBody).AddSequentialStep(frame, next));
			conditionStep.OnCompletionNotify(bodyStep);
			bodyStep.OnCompletionNotify(next);
			return conditionStep;
		}
	}
}