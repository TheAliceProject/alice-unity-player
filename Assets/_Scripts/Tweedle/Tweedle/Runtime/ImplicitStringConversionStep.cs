namespace Alice.Tweedle.VM {
    public class ImplicitStringConversionStep : ExecutionStep {
        TValue initialValue;

        public ImplicitStringConversionStep(ExecutionScope scope)
            : base(null, scope) {
        }

        internal override void BlockerFinished(ExecutionStep blockingStep) {
            base.BlockerFinished(blockingStep);
            initialValue = blockingStep.Result;
        }

        internal override void Execute() {
            if (initialValue.Type.IsTweedleDefinedType()) {
                new MethodCallExpression(initialValue, "toString", new NamedArgument[0])
                    .AsStep(scope)
                    .OnCompletionNotify(next)
                    .Queue();
            } else {
                result = TValue.FromString(initialValue.Type.ConvertToString(ref initialValue));
            }

            base.Execute();
        }
    }
}