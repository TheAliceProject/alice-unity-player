namespace Alice.Tweedle.VM {
    public class ImplicitStringConversionStep : ExecutionStep {
        TValue initialValue;

        public ImplicitStringConversionStep(ExecutionScope scope)
            : base(scope) {
        }

        internal override void BlockerFinished(ExecutionStep blockingStep) {
            base.BlockerFinished(blockingStep);
            initialValue = blockingStep.Result;
        }

        internal override void Execute() {
            if (initialValue.Type.Equals(TBuiltInTypes.TEXT_STRING)) {
                result = initialValue;
            } else {
                new MethodCallExpression(initialValue, "toString", new NamedArgument[0])
                    .AsStep(scope)
                    .OnCompletionNotify(next)
                    .Queue();
            }
            base.Execute();
        }
    }
}