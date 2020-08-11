using System;
using Alice.Tweedle;
using Alice.Utils;

namespace Alice.Tweedle.VM
{
    public  class TwoValueComputationStep : ExecutionStep
    {
        protected ITweedleExpression exp1;
        protected ITweedleExpression exp2;
        protected TValue result1;
        protected TValue result2;
        protected Func<TValue, TValue, TValue> body;

        public TwoValueComputationStep(string callStackEntry,
                                                             ExecutionScope scope,
                                                             ITweedleExpression exp1,
                                                             ITweedleExpression exp2,
                                                             Func<TValue, TValue, TValue> body)
            : base(scope)
        {
            using (PooledStringBuilder stackBuilder = PooledStringBuilder.Alloc(callStackEntry)) {
                
                scope.StackWith(stackBuilder.Builder);
                this.callStack = stackBuilder.ToString();
            }
            this.exp1 = exp1;
            this.exp2 = exp2;
            this.body = body;
        }

        protected virtual void QueueExpressionStep(ITweedleExpression exp, Action<TValue> handler)
        {
            var evalStep = exp.AsStep(scope);
            var storeStep = new ValueOperationStep(callStack, scope, handler);
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
            result = body.Invoke(result1, result2);
            base.Execute();
        }
    }
}