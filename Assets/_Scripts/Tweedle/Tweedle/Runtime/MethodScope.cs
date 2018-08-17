namespace Alice.Tweedle
{
	class MethodScope : InvocationScope
	{
		bool invokeSuper;

		public MethodScope(ExecutionScope scope, string methodName, bool invokeSuper)
			: base(scope)
		{
			callStackEntry = methodName;
			this.invokeSuper = invokeSuper;
		}

		internal void SetThis(TweedleValue target)
		{
			// target may be an instance (object or enumValue) or type (class or enum)
			thisValue = target;
			IdentifyTargetMethod();
		}

		private void IdentifyTargetMethod()
		{
			Method = invokeSuper ? thisValue.SuperMethodNamed(callingScope, callStackEntry) : thisValue.MethodNamed(callingScope, callStackEntry);
			if (Method == null)//|| !method.ExpectsArgs(callExpression.arguments))
			{
				throw new TweedleRuntimeException("No method matching " + thisValue + "." + callStackEntry + "()");
			}
		}
	}
}