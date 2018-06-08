using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

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
			Contract.Requires(arguments != null);
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
			return target =>
			{
				TweedleMethod method = target.MethodNamed(methodName);
				if (method == null)
				{
					throw new TweedleRuntimeException("No method matching " + target + "." + methodName + "()");
				}
				TweedleFrame methodFrame = frame.MethodCallFrame(target);
				method.Invoke(methodFrame, arguments);
				// TODO pop frame, invoke next operation
			};
		}
	}
}