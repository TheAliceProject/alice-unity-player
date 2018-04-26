namespace Alice.Tweedle
{
    public class AssignmentExpression : TweedleExpression
    {
        private TweedleExpression assigneeExp;
        private TweedleExpression valueExp;

        public AssignmentExpression(TweedleExpression assigneeExp, TweedleExpression valueExp)
            : base(valueExp.Type) {
            this.assigneeExp = assigneeExp;
            this.valueExp = valueExp;
        }

        public override TweedleValue Evaluate(TweedleFrame frame)
        {
            TweedleValue newValue = valueExp.Evaluate(frame);
            // TODO Evaluate the assignee as a target and set to the newValue
            return newValue;
        }
    }
}