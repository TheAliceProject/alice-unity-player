using System;
using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class MethodCallExpression : MemberAccessExpression
	{
		string methodName;
		Dictionary<string, TweedleExpression> arguments;

		public string MethodName
		{
			get { return methodName; }
		}

		public MethodCallExpression(TweedleExpression target, string methodName, Dictionary<string, TweedleExpression> arguments)
			: base(target)
		{
			this.methodName = methodName;
			this.arguments = arguments;
		}

		override public void Evaluate(TweedleFrame frame)
		{
			Target.Evaluate(frame.ExecutionFrame(InvokeMethod(frame)));
		}

		public TweedleExpression GetArg(string argName)
		{
			return arguments[argName];
		}

		Action<TweedleValue> InvokeMethod(TweedleFrame frame)
		{
			// Return an Action that takes the target of the method as a value and calls the method
			return target => { };// TODO target.InvokeMethod(methodName, arguments, frame);
		}
	}
}