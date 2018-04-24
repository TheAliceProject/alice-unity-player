using Alice.VM;

namespace Alice.Tweedle
{
    public class MethodCallExpression : MemberAccessExpression
    {
        private string methodName;

        public string MethodName
        {
            get
            {
                return methodName;
            }
        }

        public MethodCallExpression(TweedleExpression target, string methodName)
            : base(target)
        {
            this.methodName = methodName;
        }

        override public TweedleValue Evaluate(TweedleFrame frame)
        {
            EvaluateTarget(frame);
            // TODO invoke the method on the target.
            return null;
        }
    }
}