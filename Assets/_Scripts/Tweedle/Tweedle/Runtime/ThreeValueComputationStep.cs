using System;
using Alice.Tweedle;
using Alice.Utils;

namespace Alice.Tweedle.VM
{
    internal class ThreeValueComputationStep : ExecutionStep
    {
        ITweedleExpression exp1;
        ITweedleExpression exp2;
        ITweedleExpression exp3;

        TValue result1;
        TValue result2;
        TValue result3;

        Func<TValue, TValue, TValue, TValue> body;

        public ThreeValueComputationStep(string callStackEntry,
                                              ExecutionScope scope,
                                              ITweedleExpression exp1,
                                              ITweedleExpression exp2,
                                              ITweedleExpression exp3,
                                              Func<TValue, TValue, TValue, TValue> body)
            : base(callStackEntry, scope)
        {
            this.exp1 = exp1;
            this.exp2 = exp2;
            this.exp3 = exp3;
            this.body = body;
        }

        void QueueExpressionStep(ITweedleExpression exp, Action<TValue> handler)
        {
            var evalStep = exp.AsStep(scope);
            var storeStep = new ValueOperationStep("", scope, handler);
            evalStep.OnCompletionNotify(storeStep);
            storeStep.OnCompletionNotify(this);
            evalStep.Queue();
        }

        internal override void Execute()
        {
            if (result1 == TValue.UNDEFINED)
            {
                QueueExpressionStep(exp1, value => result1 = value);
                return;
            }
            if (result2 == TValue.UNDEFINED)
            {
                QueueExpressionStep(exp2, value => result2 = value);
                return;
            }
            if (result3 == TValue.UNDEFINED)
            {
                QueueExpressionStep(exp3, value => result3 = value);
                return;
            }
            result = body.Invoke(result1, result2, result3);
            base.Execute();
        }
    }
}