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

		internal override void AddStep(NotifyingStep parent, TweedleFrame frame)
		{
			Condition.AddStep(
				new SingleInputActionNotificationStep(
					frame.StackWith("if " + Condition.ToTweedle()),
					frame,
					null,
					value => (value.ToBoolean() ? ThenBody : ElseBody).AddSequentialStep(parent, frame)),
				frame);
		}

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			ExecutionStep completion = new CompletionStep();
			completion.AddBlockingStep(new SingleInputActionStep(frame.StackWith("if " + Condition.ToTweedle()),
				Condition.AsStep(frame),
				condition => completion.AddBlockingStep((condition.ToBoolean() ? ThenBody : ElseBody).ToSequentialStep(frame))));
			return completion;
		}
	}
}