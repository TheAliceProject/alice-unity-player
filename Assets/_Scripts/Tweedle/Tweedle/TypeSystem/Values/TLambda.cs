using System;
using System.Collections.Generic;
using System.Reflection;
using Alice.Tweedle.Interop;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public sealed class TLambda
    {
        private readonly LambdaExpression source;
        private readonly ExecutionScope creationScope;

        public TLambda(LambdaExpression expression, ExecutionScope creationScope)
        {
            source = expression;
            this.creationScope = creationScope;
        }

        internal void AddInvocationSteps(LambdaScope scope, StepSequence main, ITweedleExpression[] arguments)
        {
            AddArgumentSteps(scope, main, arguments);
            main.AddStep(source.Body.AsSequentialStep(scope));
            main.AddStep(ResultStep(scope));
        }

        void AddArgumentSteps(LambdaScope scope, StepSequence main, ITweedleExpression[] arguments)
        {
            for (int i = 0; i < arguments.Length; i++)
            {
                TParameter param = source.Parameters[i];
                ITweedleExpression argExp = arguments[i];
                ExecutionStep argStep = argExp.AsStep(scope.callingScope);
                var storeStep = new ValueComputationStep(
                    "Arg",
                    scope.callingScope,
                    argVal => scope.SetLocalValue(param, argVal));
                argStep.OnCompletionNotify(storeStep);
                main.AddStep(argStep);
            }
        }

        ExecutionStep ResultStep(LambdaScope scope)
        {
            return new ValueComputationStep("call", scope, arg => scope.Result);
        }

        public void QueueEvaluation(TValue[] inArguments)
        {
            ITweedleExpression[] argsAsExpressions = new ITweedleExpression[inArguments.Length];
            for (int i = 0; i < argsAsExpressions.Length; ++i)
                argsAsExpressions[i] = inArguments[i];
            QueueEvaluation(argsAsExpressions);
        }

        public ExecutionScope CapturedScope
        {
            get { return creationScope; }
        }

        public AsyncReturn<TValue> QueueEvaluation(ITweedleExpression[] inArguments)
        {
            TValue thisVal = ((TLambdaType)this.source.Type.Get()).Instantiate(this);
            LambdaEvaluation eval;

            if (inArguments != null && inArguments.Length > 0)
            {
                eval = new LambdaEvaluation(thisVal, inArguments);
            }
            else
            {
                eval = new LambdaEvaluation(thisVal);
            }

            var onComplete = eval.OnCompletion();
            creationScope.vm.Queue(eval);
            return onComplete;
        }
    }
}