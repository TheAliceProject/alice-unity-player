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

		internal override NotifyingStep AsStepToNotify(TweedleFrame frame, NotifyingStep next)
		{
			return Condition.AsStep(
				new SingleInputActionNotificationStep(
					frame.StackWith("if " + Condition.ToTweedle()),
					frame,
					value => (value.ToBoolean() ? ThenBody : ElseBody).AddSequentialStep(next, frame),
					null),
				frame);
		}
	}
}