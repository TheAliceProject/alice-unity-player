namespace Alice.Tweedle.VM
{
    class MethodScope : InvocationScope, IStackFrame {
        bool invokeSuper;
        TType invokedType;
        string[] argNames;
        string methodName;

        public MethodScope(ExecutionScope scope, string methodName, string[] argNames, bool invokeSuper)
            : base(null, scope) {
            this.callStackEntry = this;
            this.methodName = methodName;
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
                throw new TweedleRuntimeException("Can not invoke " + methodName + " on null.");
            }

            TType startingType;
            if (invokeSuper)
            {
                startingType = callingScope.CallingType(methodName);
                if (startingType == null)
                {
                    throw new TweedleRuntimeException("Call to super." + methodName + "() outside of method context on " + thisValue);
                }
                startingType = startingType.SuperType.Get(this);
            } else {
                startingType = thisValue.Type;
            }

            method = startingType.Method(callingScope, ref thisValue, methodName, argNames);

            if (method == null)//|| !method.ExpectsArgs(callExpression.arguments))
            {
                throw new TweedleRuntimeException("No method matching " + thisValue + "." + methodName + "()");
            }
            invokedType = method.ContainingType.Get(this);
        }

        internal override TType CallingType(string methodName) {
            return this.methodName.Equals(methodName) ? invokedType : base.CallingType(methodName);
        }

        public string ToStackFrame()
        {
            return methodName;
        }
    }
}