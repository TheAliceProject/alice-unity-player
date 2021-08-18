using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public class CastExpression : TweedleExpression
    {
        private ITweedleExpression m_Left;

        internal class CastExpressionStackFrame : IStackFrame {
            private TType m_CastedType;
             private ITweedleExpression m_Left;

            public CastExpressionStackFrame(TType castedType, ITweedleExpression left) {
                m_CastedType = castedType;
                m_Left = left;
            }

            public string ToStackFrame() {
                return m_Left.ToTweedle() + " as " + m_CastedType.Name;
            }
        }

        public CastExpression(ITweedleExpression lhs, TTypeRef rhs)
            : base(rhs)
        {
            m_Left = lhs;
        }

        public override ExecutionStep AsStep(ExecutionScope scope)
        {
            TType castedType = m_TypeRef.Get(scope);

            var val = m_Left.AsStep(scope);
            val.OnCompletionNotify(new ValueComputationStep(new CastExpressionStackFrame(castedType, m_Left), scope, Evaluate));
            return val;
        }

        private TValue Evaluate(TValue inValue)
        {
            TType castedType = m_TypeRef.Get();

            if (!inValue.Type.CanCastExplicitly(castedType))
            {
                throw new TweedleRuntimeException("Cannot cast type " + inValue.Type.Name + " to type " + castedType.Name);
            }

            return inValue.Type.CastExplicitly(ref inValue, castedType);
        }
    }
}