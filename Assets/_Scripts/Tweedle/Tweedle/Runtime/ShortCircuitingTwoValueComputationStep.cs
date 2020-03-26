using System;
using Alice.Tweedle;
using Alice.Utils;

namespace Alice.Tweedle.VM
{
    public class ShortCircuitingTwoValueComputationStep : TwoValueComputationStep
    {
        TValue shortCircuitValue;
        public ShortCircuitingTwoValueComputationStep(string callStackEntry,
                                                     ExecutionScope scope,
                                                     ITweedleExpression exp1,
                                                     ITweedleExpression exp2,
                                                     Func<TValue, TValue, TValue> body,
                                                     TValue shortCircuitValue)
    : base(callStackEntry, scope, exp1, exp2, body)
        {
            this.shortCircuitValue = shortCircuitValue;
        }


        internal override void Execute()
        {
            ExecutionStep es = this as ShortCircuitingTwoValueComputationStep;
            if (result1 == TValue.UNDEFINED)
            {
                QueueExpressionStep(exp1, value => result1 = value);
                return;
            }
            if (result2 == TValue.UNDEFINED)
            {
                if (result1 == shortCircuitValue)
                {
                    result2 = shortCircuitValue;
                    base.Execute();
                }
                else
                {
                    QueueExpressionStep(exp2, value => result2 = value);
                }
                return;
            }
            base.Execute();
        }
    }
}
