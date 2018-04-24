using Alice.VM;

namespace Alice.Tweedle
{
    public class FieldAccess : MemberAccessExpression
    {
        private string fieldName;

        public string FieldName
        {
            get
            {
                return fieldName;
            }
        }

        public FieldAccess(TweedleExpression target, string fieldName)
            : base(target)
        {
            this.fieldName = fieldName;
        }

        override public TweedleValue Evaluate(TweedleFrame frame)
        {
            EvaluateTarget(frame);
            // TODO access the fieldName on the target.
            return null;
        }
    }
}