using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class SuperInstantiation : TweedleExpression
	{
		Dictionary<string, TweedleExpression> Arguments { get; }

		public SuperInstantiation(Dictionary<string, TweedleExpression> arguments)
			: base(null)
		{
			Arguments = arguments;
		}

		internal override ExecutionStep AsStep(ExecutionScope scope)
		{
			ConstructorScope superScope = ((ConstructorScope)scope).SuperScope(Arguments);
			return superScope.InvocationStep("super()", Arguments);
		}
	}
}