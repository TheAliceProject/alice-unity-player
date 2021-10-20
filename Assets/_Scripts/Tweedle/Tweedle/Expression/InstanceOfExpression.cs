using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public class InstanceOfExpression : TweedleExpression
    {
        private ITweedleExpression m_Left;

        private class InstanceOfExpressionStackFrame : IStackFrame {

            private TType m_CastedType;
            private ITweedleExpression m_Left;

            public InstanceOfExpressionStackFrame(TType castedType, ITweedleExpression left) {
                m_CastedType = castedType;
                m_Left = left;
            }
            
            public string ToStackFrame() {
                 return m_Left.ToTweedle() + " instanceOf " + m_CastedType.Name;
            }
        }

        public InstanceOfExpression(ITweedleExpression lhs, TTypeRef rhs)
            : base(rhs)
        {
            m_Left = lhs;
        }

        public override ExecutionStep AsStep(ExecutionScope scope)
        {
            TType castedType = m_TypeRef.Get(scope);

            var val = m_Left.AsStep(scope);
            val.OnCompletionNotify(new ValueComputationStep(new InstanceOfExpressionStackFrame(castedType, m_Left), scope, Evaluate));
            return val;
        }

        private TValue Evaluate(TValue inValue)
        {
            TType castedType = m_TypeRef.Get();
            return TValue.FromBoolean(TType.InstanceOf(inValue, castedType));
        }
    }
}