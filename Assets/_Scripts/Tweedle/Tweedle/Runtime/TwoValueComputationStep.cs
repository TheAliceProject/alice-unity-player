using System;
using Alice.Tweedle;
using Alice.Utils;

namespace Alice.Tweedle.VM
{
    internal class TwoValueComputationStep : ExecutionStep
    {
        ITweedleExpression exp1;
        ITweedleExpression exp2;
        TValue result1;
        TValue result2;
        Func<TValue, TValue, TValue> body;
        bool logicalAnd, logicalOr;

        public TwoValueComputationStep(string callStackEntry,
                                                             ExecutionScope scope,
                                                             ITweedleExpression exp1,
                                                             ITweedleExpression exp2,
                                                             Func<TValue, TValue, TValue> body,
                                                             bool logicalAnd = false,
                                                             bool logicalOr = false)
            : base(scope)
        {
            using (PooledStringBuilder stackBuilder = PooledStringBuilder.Alloc(callStackEntry)) {
                
                scope.StackWith(stackBuilder.Builder);
                this.callStack = stackBuilder.ToString();
            }
            this.exp1 = exp1;
            this.exp2 = exp2;
            this.body = body;
            this.logicalAnd = logicalAnd;
            this.logicalOr = logicalOr;
        }

        void QueueExpressionStep(ITweedleExpression exp, Action<TValue> handler)
        {
            var evalStep = exp.AsStep(scope);
            var storeStep = new ValueOperationStep(callStack, scope, handler);
            evalStep.OnCompletionNotify(storeStep);
            storeStep.OnCompletionNotify(this);
            evalStep.Queue();
        }

        void QueueExpressionStepWithoutEval(ITweedleExpression exp, Action<TValue> handler)
        {
            var storeStep = new ValueOperationStep(callStack, scope, handler);
            storeStep.OnCompletionNotify(this);
            storeStep.Queue();
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
                if (logicalAnd && !result1.ToBoolean())
                {
                    result2 = TValue.FALSE;
                    result = body.Invoke(result1, result2);
                    base.Execute();
                }
                else if (logicalOr && result1.ToBoolean())
                {
                    result2 = TValue.TRUE;
                    result = body.Invoke(result1, result2);
                    base.Execute();
                }
                else
                {
                    QueueExpressionStep(exp2, value => result2 = value);
                }
                return;
            }
            result = body.Invoke(result1, result2);
            base.Execute();
        }
    }
}