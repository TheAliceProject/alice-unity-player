using System;

namespace Alice.Tweedle
{
	public class AssignmentExpression : TweedleExpression
	{
		TweedleExpression TargetExp { get; }
		TweedleExpression ValueExp { get; }
		string Identifier { get; }

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

		public override void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			ValueExp.Evaluate(frame, value =>
			{
				if (TargetExp == null)
				{
					frame.SetValue(Identifier, value, next);
				}
				else
				{
					TargetExp.Evaluate(frame, target =>
					{
						((TweedleObject)target).Set(Identifier, value);
						next(value);
					});
				}
			});
		}
	}
}