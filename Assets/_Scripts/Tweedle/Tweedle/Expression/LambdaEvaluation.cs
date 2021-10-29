using System.Collections.Generic;
using Alice.Tweedle.Interop;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public class LambdaEvaluation : TweedleExpression
    {
        ITweedleExpression target;
        ITweedleExpression[] arguments;
        AsyncReturn<TValue> onComplete;

        private static IStackFrame setTargetStackFrame = new StaticStackFrame("Set Target");
        private static IStackFrame invocationStackFrame = new StaticStackFrame("Invocation");

        public ITweedleExpression[] Arguments
        {
            get { return arguments; }
        }

        public LambdaEvaluation(ITweedleExpression target)
        {
            this.target = target;
            arguments = new ITweedleExpression[0];
        }

        public LambdaEvaluation(ITweedleExpression target, ITweedleExpression[] arguments)
        {
            this.target = target;
            this.arguments = arguments;
        }

        public AsyncReturn<TValue> OnCompletion()
        {
            if (onComplete == null)
                onComplete = new AsyncReturn<TValue>();
            return onComplete;
        }

        public override string ToTweedle()
        {
            // TODO improve expression
            return "lambda eval ()";
        }

        public override ExecutionStep AsStep(ExecutionScope scope)
        {
            LambdaScope lambdaScope = scope.LambdaScope();
            var targetStep = target.AsStep(scope);
            var setTargetStep = new ValueOperationStep(
                setTargetStackFrame,
                lambdaScope,
                target => lambdaScope.SetLambda(target.Lambda()));
            targetStep.OnCompletionNotify(setTargetStep);

            StepSequence main = new StepSequence(this, scope);
            main.AddStep(targetStep);
            main.AddStep(new DelayedOperationStep(
                invocationStackFrame,
                lambdaScope,
                () => lambdaScope.QueueInvocationStep(main, arguments, onComplete)));

            return main;
        }
    }
}