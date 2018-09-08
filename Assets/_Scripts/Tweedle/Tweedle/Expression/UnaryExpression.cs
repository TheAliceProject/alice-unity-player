using Alice.VM;

namespace Alice.Tweedle
{
	public abstract class UnaryExpression : TweedleExpression
	{
		protected UnaryExpression()
			: base()
		{
		}

		protected UnaryExpression(TTypeRef typeRef)
			: base(typeRef)
		{
        }

        public abstract TValue EvaluateLiteral();
    }
}