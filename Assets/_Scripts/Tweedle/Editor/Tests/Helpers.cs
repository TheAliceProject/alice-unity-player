using Alice.Tweedle;
using Alice.Tweedle.VM;

namespace Alice.Tweedle.Parse
{
    internal static class TweedleExpressionExtensions
    {
        private static IStackFrame evaluateNowStackFrame = new StaticStackFrame("EvaluateNow");

        internal static TValue EvaluateNow(this ITweedleExpression inExpression, ExecutionScope inScope)
        {
            TValue result = TValue.NULL;
            var expStep = inExpression.AsStep(inScope);
            var storeStep = new ValueOperationStep(
                    evaluateNowStackFrame,
                    inScope,
                    value => result = value);
            expStep.OnCompletionNotify(storeStep);
            expStep.EvaluateNow();
            return result;
        }

        internal static TValue EvaluateNow(this ITweedleExpression inExpression)
        {
            return EvaluateNow(inExpression, new ExecutionScope(evaluateNowStackFrame));
        }
    }
}