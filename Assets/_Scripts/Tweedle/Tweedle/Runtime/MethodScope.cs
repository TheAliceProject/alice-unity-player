namespace Alice.Tweedle.VM
{
    class MethodScope : InvocationScope {
        bool invokeSuper;

        public MethodScope(ExecutionScope scope, string methodName, bool invokeSuper)
            : base(scope) {
            callStackEntry = methodName;
            this.invokeSuper = invokeSuper;
        }

        internal void SetThis(TValue target) {
            // target may be an instance (object or enumValue) or type (class or enum)
            thisValue = target;
            IdentifyTargetMethod();
        }

        private void IdentifyTargetMethod() {
            if (thisValue == null || thisValue == TValue.NULL) {
                throw new TweedleRuntimeException("Can not invoke " + callStackEntry + " on null.");
            }
            TType type;
            if (invokeSuper) {
                type = thisValue.Type.SuperType.Get(this);
            } else {
                type = thisValue.Type;
            }

            method = type.Method(callingScope, ref thisValue, callStackEntry);

            if (method == null)//|| !method.ExpectsArgs(callExpression.arguments))
            {
                throw new TweedleRuntimeException("No method matching " + thisValue + "." + callStackEntry + "()");
            }
        }
    }
}