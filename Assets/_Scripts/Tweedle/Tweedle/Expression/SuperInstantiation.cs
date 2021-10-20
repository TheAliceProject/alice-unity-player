using System.Collections.Generic;
using Alice.Tweedle.VM;
using Alice.Utils;

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
            return superScope.InvocationStep(this, m_Arguments);
        }

        public override string ToTweedle()
        {
            using(PooledStringBuilder pooledString = PooledStringBuilder.Alloc())
            {
                pooledString.Builder.Append("super(");
                for (int i = 0; i < m_Arguments.Length; ++i)
                {
                    if (i > 0)
                    {
                        pooledString.Builder.Append(", ");
                    }

                    pooledString.Builder.Append(m_Arguments[i].Name).Append(": ").Append(m_Arguments[i].Argument.ToTweedle());
                }
                pooledString.Builder.Append(')');

                return pooledString.ToString();
            }
        }
    }
}