using System;
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

		internal override NotifyingEvaluationStep AsStep(NotifyingStep parent, TweedleFrame frame)
		{
			if (TargetExp == null)
			{
				return ValueExp.AsStep(
					new SingleInputActionNotificationStep(
						frame.StackWith(ToTweedle()),
						frame,
						parent,
						value => frame.SetValue(Identifier, value)),
					frame);
			}
			else
			{
				return new DoubleInputEvalStep(
					frame.StackWith(ToTweedle()),
					frame,
					parent,
					TargetExp,
					ValueExp,
					(target, value) =>
					{
						target.Set(Identifier, value, frame);
						return value;
					});
			}
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			if (TargetExp == null)
			{
				return new StartStep(frame.StackWith(ToTweedle()), () => new SingleInputStep(frame.StackWith(ToTweedle()),
					ValueExp.AsStep(frame),
					value => frame.SetValue(Identifier, value)));
			}
			else
			{
				return new StartStep(frame.StackWith(ToTweedle()), () => new DoubleInputStep(
					TargetExp.AsStep(frame),
					ValueExp.AsStep(frame),
					(target, value) =>
					{
						target.Set(Identifier, value, frame);
						return value;
					}));
			}
		}

		internal override string ToTweedle()
		{
			return Identifier + " <- " + ValueExp;
		}
	}
}