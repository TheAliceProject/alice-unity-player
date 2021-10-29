using System.Collections.Generic;
using Alice.Tweedle.VM;
using Alice.Utils;

namespace Alice.Tweedle
{
    public class EnumInstantiation : TweedleExpression
    {
        private readonly TEnumType m_EnumType;
        private readonly TEnumValueInitializer m_ValueInitializer;

        public EnumInstantiation(TEnumType inType, TEnumValueInitializer inValue)
            : base(inType)
        {
            m_EnumType = inType;
            m_ValueInitializer = inValue;
        }

        public override ExecutionStep AsStep(ExecutionScope scope)
        {
            ConstructorScope ctrScope = scope.EnumInstantiationScope(m_EnumType, m_ValueInitializer);
            return ctrScope.InvocationStep(this, m_ValueInitializer.Arguments);
        }

        public override string ToTweedle()
        {
            using(PooledStringBuilder pooledString = PooledStringBuilder.Alloc())
            {
                pooledString.Builder.Append(m_ValueInitializer.Name).Append('(');
                for (int i = 0; i < m_ValueInitializer.Arguments.Length; ++i)
                {
                    if (i > 0)
                    {
                        pooledString.Builder.Append(", ");
                    }

                    pooledString.Builder.Append(m_ValueInitializer.Arguments[i].Name).Append(": ")
                        .Append(m_ValueInitializer.Arguments[i].Argument.ToTweedle());
                }
                pooledString.Builder.Append(')');
                pooledString.Builder.Append(" = ").Append(m_ValueInitializer.Value);

                return pooledString.ToString();
            }
        }
    }
}