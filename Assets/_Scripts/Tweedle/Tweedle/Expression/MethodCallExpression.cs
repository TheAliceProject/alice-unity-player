using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class MethodCallExpression : MemberAccessExpression
	{
		private string methodName;
		private Dictionary<string, TweedleExpression> arguments;

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

        override public TweedleValue Evaluate(TweedleFrame frame)
        {
            EvaluateTarget(frame);
            // TODO invoke the method on the target.
            return null;
        }

		public TweedleExpression GetArg(string argName)
		{
			return arguments[argName];
		}
    }
}