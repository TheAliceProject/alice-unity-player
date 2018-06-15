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

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			return new ConditionalStep(this, frame, Condition.AsStep(frame));
		}
	}

	internal class ConditionalStep : StatementStep<ConditionalStatement>
	{
		EvaluationStep condition;
		ExecutionStep body;

		public ConditionalStep(ConditionalStatement statement, TweedleFrame frame, EvaluationStep condition)
			: base(statement, frame, condition)
		{
			this.condition = condition;
		}

		internal override bool Execute()
		{
			if (condition.Result.ToBoolean())
			{
				return ExecuteBodyOnceAndWait(statement.ThenBody);
			}
			else
			{
				return ExecuteBodyOnceAndWait(statement.ElseBody);
			}
		}

		bool ExecuteBodyOnceAndWait(BlockStatement statement)
		{
			if (body == null)
			{
				body = statement.ToSequentialStep(frame);
				AddBlockingStep(body);
				return false;
			}
			return true;
		}
	}
}