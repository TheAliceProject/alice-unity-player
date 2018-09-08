using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class WhileLoop : AbstractLoop
	{
		public ITweedleExpression RunCondition { get; }

		public WhileLoop(ITweedleExpression runCondition, TweedleStatement[] body) : base(body)
		{
			RunCondition = runCondition;
		}

		internal override ExecutionStep AsStepToNotify(ExecutionScope scope, ExecutionStep next)
		{
			return RunCondition.AsStep(scope).OnCompletionNotify(new WhileLoopStep(this, scope, next));
		}
	}

	internal class WhileLoopStep : LoopStep<WhileLoop>
	{
		bool shouldRunBody = false;

		public WhileLoopStep(WhileLoop statement, ExecutionScope scope, ExecutionStep next)
			: base(statement, scope, next)
		{
		}

		internal override void BlockerFinished(ExecutionStep blockingStep)
		{
			base.BlockerFinished(blockingStep);
			shouldRunBody = blockingStep.Result.ToBoolean();
		}

		internal override void Execute()
		{
			if (shouldRunBody)
			{
				var loopScope = scope.ChildScope("While loop");
				var shouldRunBodyAgain = statement.RunCondition.AsStep(scope);
				shouldRunBodyAgain.OnCompletionNotify(this);
				statement.Body.AddSequentialStep(loopScope, shouldRunBodyAgain);
			}
			else
			{
				base.Execute();
			}
		}
	}
}