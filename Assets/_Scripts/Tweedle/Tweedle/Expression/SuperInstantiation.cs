using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class SuperInstantiation : TweedleExpression
	{
        private readonly NamedArgument[] m_Arguments;

        public SuperInstantiation(NamedArgument[] inArguments)
			: base(null)
		{
			m_Arguments = inArguments;
		}

		public override ExecutionStep AsStep(ExecutionScope scope)
		{
			ConstructorScope superScope = ((ConstructorScope)scope).SuperScope(m_Arguments);
			return superScope.InvocationStep("super()", m_Arguments);
		}
	}
}