namespace Alice.Tweedle.VM
{
    class MethodScope : InvocationScope {
        bool invokeSuper;
        TType invokedType;
        string[] argNames;

        public MethodScope(ExecutionScope scope, string methodName, string[] argNames, bool invokeSuper)
            : base(scope) {
            callStackEntry = methodName;
            this.argNames = argNames;
            this.invokeSuper = invokeSuper;
        }

        internal void SetThis(TValue target) {
            // target may be an instance (object or enumValue) or type (class or enum)
            thisValue = target;
            IdentifyTargetTypeAndMethod();
        }

        private void IdentifyTargetTypeAndMethod() {
            if (thisValue == null || thisValue == TValue.NULL) {
                throw new TweedleRuntimeException("Can not invoke " + callStackEntry + " on null.");
            }

            TType startingType;
            if (invokeSuper)
            {
                startingType = callingScope.CallingType(callStackEntry);
                if (startingType == null)
                {
                    throw new TweedleRuntimeException("Call to super." + callStackEntry + "() outside of method context on " + thisValue);
                }
                startingType = startingType.SuperType.Get(this);
            } else {
                startingType = thisValue.Type;
            }

            method = startingType.Method(callingScope, ref thisValue, callStackEntry, argNames);

            if (method == null)//|| !method.ExpectsArgs(callExpression.arguments))
            {
                throw new TweedleRuntimeException("No method matching " + thisValue + "." + callStackEntry + "()");
            }
            invokedType = method.ContainingType.Get(this);
        }

        internal override TType CallingType(string methodName) {
            return callStackEntry.Equals(methodName) ? invokedType : base.CallingType(methodName);
        }
    }
}