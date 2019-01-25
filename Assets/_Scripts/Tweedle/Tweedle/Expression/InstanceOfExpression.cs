using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public class InstanceOfExpression : TweedleExpression
    {
        private ITweedleExpression m_Left;

        public InstanceOfExpression(ITweedleExpression lhs, TTypeRef rhs)
            : base(rhs)
        {
            m_Left = lhs;
        }

        public override ExecutionStep AsStep(ExecutionScope scope)
        {
            TType castedType = m_TypeRef.Get(scope);

            var val = m_Left.AsStep(scope);
            val.OnCompletionNotify(new ValueComputationStep(m_Left.ToTweedle() + " instanceOf " + castedType.Name, scope, Evaluate));
            return val;
        }

        private TValue Evaluate(TValue inValue)
        {
            TType castedType = m_TypeRef.Get();
            return TValue.FromBoolean(TType.InstanceOf(inValue, castedType));
        }
    }
}