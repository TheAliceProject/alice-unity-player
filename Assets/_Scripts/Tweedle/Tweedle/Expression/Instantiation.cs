using System.Collections.Generic;
using Alice.Tweedle.VM;
using Alice.Utils;

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
            TType type = Type.Get(scope);
            if (!type.CanInstantiate(scope))
            {
                throw new TweedleRuntimeException("Cannot instantiate type " + type.Name + " in this context");
            }

            ConstructorScope ctrScope = scope.InstantiationScope(type);
            return ctrScope.InvocationStep("Instantiation", m_Arguments);
        }

        public override string ToTweedle()
        {
            using(PooledStringBuilder pooledString = PooledStringBuilder.Alloc())
            {
                pooledString.Builder.Append("new ").Append(m_TypeRef).Append('(');
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