using System;
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

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			ExecutionStep completion = new CompletionStep();
			completion.AddBlockingStep(new SingleInputStep( // TODO replace with Execution step to avoid null return
				Condition.AsStep(frame),
				condition =>
				{
					completion.AddBlockingStep((condition.ToBoolean() ? ThenBody : ElseBody).ToSequentialStep(frame));
					return TweedleNull.NULL;
				}));
			return completion;
		}
	}
}