using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Alice.VM;

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

		override public void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			Target.Evaluate(frame, InvokeMethod(frame, next));
		}

		public TweedleExpression GetArg(string argName)
		{
			return arguments[argName];
		}

		Action<TweedleValue> InvokeMethod(TweedleFrame frame, Action<TweedleValue> next)
		{
			return target =>
			{
				TweedleMethod method = target.MethodNamed(frame, methodName);
				if (method == null)
				{
					throw new TweedleRuntimeException("No method matching " + target + "." + methodName + "()");
				}
				TweedleFrame methodFrame = frame.MethodCallFrame(target, next);
				method.Invoke(methodFrame, arguments);
			};
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			throw new NotImplementedException();
		}
	}
}