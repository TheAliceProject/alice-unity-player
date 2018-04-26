using Alice.VM;
using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class MethodCallExpression : MemberAccessExpression
	{
		private string methodName;
		private Dictionary<string, TweedleExpression> arguments;

		public string MethodName
		{
			get
			{
				return methodName;
			}
		}

		public MethodCallExpression(TweedleExpression target, string methodName)
            : base(target)
        {
            this.methodName = methodName;
			arguments = new Dictionary<string, TweedleExpression>();
        }

        override public TweedleValue Evaluate(TweedleFrame frame)
        {
            EvaluateTarget(frame);
            // TODO invoke the method on the target.
            return null;
        }

		public void AddArg(string argName, TweedleExpression argValue)
		{
			arguments.Add(argName, argValue);
		}

		public TweedleExpression GetArg(string argName)
		{
			return arguments[argName];
		}
    }
}