namespace Alice.Tweedle
{
    abstract public class StaticAccessExpression : TweedleExpression
    {
		protected TweedleTypeDeclaration target;

		public StaticAccessExpression(TweedleTypeDeclaration target)
            : base(null)
        {
            this.target = target;
        }
    }
}