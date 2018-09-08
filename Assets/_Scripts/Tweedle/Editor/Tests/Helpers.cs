using Alice.Tweedle;
using Alice.VM;

namespace Alice.Tweedle.Parse
{
    internal static class TweedleExpressionExtensions
    {
        internal static TValue EvaluateNow(this ITweedleExpression inExpression, ExecutionScope inScope)
        {
            TValue result = TValue.NULL;
            var expStep = inExpression.AsStep(inScope);
            var storeStep = new ValueOperationStep(
                    "EvaluateNow",
                    inScope,
                    value => result = value);
            expStep.OnCompletionNotify(storeStep);
            expStep.EvaluateNow();
            return result;
        }

        internal static TValue EvaluateNow(this ITweedleExpression inExpression)
        {
            return EvaluateNow(inExpression, new ExecutionScope("EvaluateNow"));
        }
    }
}