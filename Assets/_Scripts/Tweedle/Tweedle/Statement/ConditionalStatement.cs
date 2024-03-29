﻿using System.Collections.Generic;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public class ConditionalStatement : TweedleStatement
    {
        public ITweedleExpression Condition { get; }
        public BlockStatement ThenBody { get; }
        public BlockStatement ElseBody { get; }

        public ConditionalStatement(ITweedleExpression condition, TweedleStatement[] thenBody, TweedleStatement[] elseBody)
        {
            Condition = condition;
            ThenBody = new BlockStatement(thenBody);
            ElseBody = new BlockStatement(elseBody);
        }

        internal override ExecutionStep AsStepToNotify(ExecutionScope scope, ExecutionStep next)
        {
            var conditionStep = Condition.AsStep(scope);
            var bodyStep = new ValueOperationStep(
                    this,
                    scope,
                    value => (value.ToBoolean() ? ThenBody : ElseBody).AddSequentialStep(scope.ChildScope(), next));
            conditionStep.OnCompletionNotify(bodyStep);
            bodyStep.OnCompletionNotify(next);
            return conditionStep;
        }

        public override string ToTweedle()
        {
            return "if " + Condition.ToTweedle();
        }
    }
}