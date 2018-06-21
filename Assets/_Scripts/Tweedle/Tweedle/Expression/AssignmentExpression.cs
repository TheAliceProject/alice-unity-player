using Alice.VM;

namespace Alice.Tweedle
{
	public class AssignmentExpression : TweedleExpression
	{
		TweedleExpression TargetExp { get; }
		TweedleExpression ValueExp { get; }
		internal string Identifier { get; }

		public AssignmentExpression(TweedleExpression assigneeExp, TweedleExpression valueExp)
			: base(valueExp.Type)
		{
			ValueExp = valueExp;
			if (assigneeExp is IdentifierReference)
			{
				Identifier = ((IdentifierReference)assigneeExp).Name;
				TargetExp = null;
			}
			else
			{
				if (assigneeExp is FieldAccess)
				{
					Identifier = ((FieldAccess)assigneeExp).FieldName;
					TargetExp = ((FieldAccess)assigneeExp).Target;
				}
				else
				{
					throw new TweedleLinkException("Unable to handle assigment to " + assigneeExp);
				}
			}
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			if (TargetExp == null)
			{
				return new StartStep(() =>
				{
					return new SingleInputStep(
						ValueExp.AsStep(frame),
						value => frame.SetValue(Identifier, value));
				});
			}
			else
			{
				return new StartStep(() =>
				{
					return new DoubleInputStep(
						TargetExp.AsStep(frame),
						ValueExp.AsStep(frame),
						(target, value) =>
						{
							((TweedleObject)target).Set(Identifier, value);
							return value;
						});
				});
			}
		}
	}
}