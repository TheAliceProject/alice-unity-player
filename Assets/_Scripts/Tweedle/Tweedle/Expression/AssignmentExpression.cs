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

		internal override ExecutionStep AsStep(ExecutionScope scope)
		{
			if (TargetExp == null)
			{
				ExecutionStep valueStep = ValueExp.AsStep(scope);
				valueStep.OnCompletionNotify(
					new ValueComputationStep(
						ToTweedle(),
						scope,
						value => scope.SetValue(Identifier, value)));
				return valueStep;
			}
			else
			{
				return new TwoValueComputationStep(
					ToTweedle(),
					scope,
					TargetExp,
					ValueExp,
					(target, value) =>
					{
						target.Set(Identifier, value, scope);
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