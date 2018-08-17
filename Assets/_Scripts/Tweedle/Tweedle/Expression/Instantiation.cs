using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class Instantiation : TweedleExpression
	{
		internal Dictionary<string, TweedleExpression> Arguments { get; }

		public Instantiation(TweedleTypeReference type, Dictionary<string, TweedleExpression> arguments)
			: base(type)
		{
			Arguments = arguments;
			if (type == null)
			{
				UnityEngine.Debug.Log("Placeholder");
			}
		}

		internal override ExecutionStep AsStep(ExecutionScope scope)
		{
			ConstructorScope ctrScope = scope.ForInstantiation(Type.AsClass(scope));
			return ctrScope.InvocationStep("Instantiation", Arguments);
		}
	}
}