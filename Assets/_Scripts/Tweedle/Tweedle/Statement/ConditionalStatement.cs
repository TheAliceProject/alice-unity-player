using System;
using System.Collections.Generic;

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

		public override void Execute(TweedleFrame frame, Action next)
		{
			Condition.Evaluate(frame, value =>
			{
				if (value.ToBoolean())
				{
					ThenBody.ExecuteInSequence(frame, next);
				}
				else
				{
					ElseBody.ExecuteInSequence(frame, next);
				}
			});
		}
	}
}