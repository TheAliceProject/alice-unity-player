using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class ConditionalStatement : TweedleStatement
	{
		TweedleExpression condition;

		public TweedleExpression Condition
		{
			get { return condition; }
		}

		BlockStatement thenBody;

		public BlockStatement ThenBody
		{
			get { return thenBody; }
		}

		BlockStatement elseBody;

		public BlockStatement ElseBody
		{
			get { return elseBody; }
		}

		public ConditionalStatement(TweedleExpression condition, List<TweedleStatement> thenBody, List<TweedleStatement> elseBody)
		{
			this.condition = condition;
			this.thenBody = new BlockStatement(thenBody);
			this.elseBody = new BlockStatement(elseBody);
		}

		public override void Execute(TweedleFrame frame)
		{
			condition.Evaluate(frame.ExecutionFrame(value => ExecuteBody(value, frame)));
		}

		void ExecuteBody(TweedleValue value, TweedleFrame frame)
		{
			if (value.ToBoolean())
			{
				thenBody.ExecuteInSequence(frame);
			}
			else
			{
				elseBody.ExecuteInSequence(frame);
			}
		}
	}
}