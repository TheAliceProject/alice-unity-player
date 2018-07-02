namespace Alice.Tweedle
{
	class MethodFrame : InvocationFrame
	{
		bool invokeSuper;

		public MethodFrame(TweedleFrame frame, string methodName, bool invokeSuper)
			: base(frame)
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
			Method = invokeSuper ? thisValue.SuperMethodNamed(callingFrame, callStackEntry) : thisValue.MethodNamed(callingFrame, callStackEntry);
			if (Method == null)//|| !method.ExpectsArgs(callExpression.arguments))
			{
				throw new TweedleRuntimeException("No method matching " + thisValue + "." + callStackEntry + "()");
			}
		}

		internal void Return(TweedleValue result)
		{
			Result = result;
		}
	}
}