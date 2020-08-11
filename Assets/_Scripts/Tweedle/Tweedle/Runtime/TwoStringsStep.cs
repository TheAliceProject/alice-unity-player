using System;

namespace Alice.Tweedle.VM
{
    public class TwoStringsStep : TwoValueComputationStep
    {
        public TwoStringsStep(string callStackEntry,
                                            ExecutionScope scope,
                                            ITweedleExpression exp1,
                                            ITweedleExpression exp2,
                                            Func<TValue, TValue, TValue> body)
            : base(callStackEntry, scope, exp1, exp2, body)
        {
        }

        protected override void QueueExpressionStep(ITweedleExpression exp, Action<TValue> handler)
        {
            var evalStep = exp.AsStep(scope);
            var castStep = new ImplicitStringConversionStep(scope);
            var storeStep = new ValueOperationStep(callStack, scope, handler);
            evalStep.OnCompletionNotify(castStep);
            castStep.OnCompletionNotify(storeStep);
            storeStep.OnCompletionNotify(this);
            evalStep.Queue();
        }
    }
}
