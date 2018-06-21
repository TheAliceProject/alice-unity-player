namespace Alice.Tweedle
{
	class MethodFrame : InvocationFrame
	{

		public MethodFrame(TweedleFrame frame)
			: base(frame)
		{
		}

		internal void SetThis(TweedleValue target)
		{
			// target may be an instance (object or enumValue) or type (class or enum)
			thisValue = target;
		}

		internal void Return(TweedleValue result)
		{
			Result = result;
			// TODO prevent further steps and return
		}
	}
}