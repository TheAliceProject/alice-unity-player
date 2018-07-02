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

		internal override NotifyingEvaluationStep AsStep(TweedleFrame frame)
		{
			if (TargetExp == null)
			{
				NotifyingEvaluationStep valueStep = ValueExp.AsStep(frame);
				valueStep.Notify(
					new SingleInputActionNotificationStep(
						ToTweedle(),
						frame,
						value => frame.SetValue(Identifier, value)));
				return valueStep;
			}
			else
			{
				return new DoubleInputEvalStep(
					ToTweedle(),
					frame,
					TargetExp,
					ValueExp,
					(target, value) =>
					{
						target.Set(Identifier, value, frame);
						return value;
					});
			}
		}

		internal override string ToTweedle()
		{
			return Identifier + " <- " + ValueExp;
		}
	}
}