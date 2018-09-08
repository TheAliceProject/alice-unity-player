using Alice.VM;

namespace Alice.Tweedle
{
	public class AssignmentExpression : TweedleExpression
	{
		ITweedleExpression TargetExp { get; }
		ITweedleExpression ValueExp { get; }
		internal string Identifier { get; }

		public AssignmentExpression(ITweedleExpression assigneeExp, ITweedleExpression valueExp)
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

		public override ExecutionStep AsStep(ExecutionScope scope)
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
						target.Set(scope, Identifier, value);
						return value;
					});
			}
		}

		public override string ToTweedle()
		{
			return Identifier + " <- " + ValueExp;
		}
	}
}