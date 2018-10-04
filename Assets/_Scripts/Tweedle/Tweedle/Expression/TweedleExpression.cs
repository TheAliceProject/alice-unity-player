using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public abstract class TweedleExpression : ITweedleExpression
    {
        protected readonly TTypeRef m_TypeRef;

        public TTypeRef Type { get { return m_TypeRef; } }

        protected TweedleExpression()
        {
            m_TypeRef = null;
        }

        protected TweedleExpression(TTypeRef typeRef)
        {
            m_TypeRef = typeRef;
        }

        public virtual string ToTweedle()
        {
            // TODO Override in all subclasses and make this abstract
            return ToString();
        }

        public abstract ExecutionStep AsStep(ExecutionScope scope);
    }
}