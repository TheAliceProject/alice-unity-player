using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Alice.Tweedle
{
	public class StaticMethodCallExpression : StaticAccessExpression
	{
		private TweedleMethod method;
		private Dictionary<string, TweedleExpression> arguments;

		public string MethodName
		{
			get { return method.Name; }
		}

		public StaticMethodCallExpression(TweedleTypeDeclaration target, string methodName, Dictionary<string, TweedleExpression> arguments)
			: base(target)
		{
			Contract.Requires(arguments != null);
			this.arguments = arguments;
			method = target.MethodNamed(methodName);
			if (method == null || !method.IsStatic())
			{
				throw new TweedleLinkException("No method matching " + target.Name + "." + methodName + "()");
			}
		}

		override public void Evaluate(TweedleFrame frame)
		{
			method.Invoke(frame, arguments);
		}

		public TweedleExpression GetArg(string argName)
		{
			return arguments[argName];
		}
	}
}