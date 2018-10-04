using System.Collections.Generic;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public class ForEachInArrayLoop : AbstractLoop
    {
        internal TLocalVariable item;
        ITweedleExpression array;

        public ForEachInArrayLoop(TLocalVariable item, ITweedleExpression array, TweedleStatement[] body)
            : base(body)
        {
            this.item = item;
            this.array = array;
        }

        internal override ExecutionStep AsStepToNotify(ExecutionScope scope, ExecutionStep next)
        {
            return array.AsStep(scope).OnCompletionNotify(new ForEachInArrayStep(this, scope, next));
        }
    }

    internal class ForEachInArrayStep : LoopStep<ForEachInArrayLoop>
    {
        TArray items;
        int index = 0;

        public ForEachInArrayStep(ForEachInArrayLoop statement, ExecutionScope scope, ExecutionStep next)
            : base(statement, scope, next)
        {
        }

        internal override void BlockerFinished(ExecutionStep blockingStep)
        {
            base.BlockerFinished(blockingStep);
            if (items == null)
            {
                items = blockingStep.Result.Array();
            }
        }

        internal override void Execute()
        {
            if (index < items.Length)
            {
                var loopScope = scope.ChildScope("ForEach loop", statement.item, items[index++]);
                statement.Body.AddSequentialStep(loopScope, this);
            }
            else
            {
                base.Execute();
            }
        }
    }
}