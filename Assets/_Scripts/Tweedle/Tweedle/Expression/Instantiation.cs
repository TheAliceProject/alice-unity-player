using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class Instantiation : TweedleExpression
	{
        private readonly NamedArgument[] m_Arguments;

        public Instantiation(TTypeRef inType, NamedArgument[] inArguments)
			: base(inType)
		{
			m_Arguments = inArguments;
		}

		public override ExecutionStep AsStep(ExecutionScope scope)
		{
			ConstructorScope ctrScope = scope.ForInstantiation(Type.Get(scope));
			return ctrScope.InvocationStep("Instantiation", m_Arguments);
		}
	}
}