using System.Collections.Generic;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public class StaticInstantiation : TweedleExpression
    {
        private TType m_TargetType;

        public StaticInstantiation(TType inType)
            : base(TBuiltInTypes.VOID)
        {
            m_TargetType = inType;
        }

        public override ExecutionStep AsStep(ExecutionScope scope)
        {
            StaticConstructorScope ctrScope = scope.StaticInstantiationScope(m_TargetType);
            return ctrScope.InvocationStep(this);
        }

        public override string ToTweedle()
        {
            return m_TargetType.ToString() + "()";
        }
    }
}