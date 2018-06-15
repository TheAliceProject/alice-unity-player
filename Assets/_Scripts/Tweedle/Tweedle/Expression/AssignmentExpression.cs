using System;
using System.Collections.Generic;
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

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			if (TargetExp == null)
			{
				return new AssignmentStep(frame, this, ValueExp.AsStep(frame));
			}
			else
			{
				return new AssignmentStep(frame, this, ValueExp.AsStep(frame), TargetExp.AsStep(frame));
			}
		}
	}

	internal class AssignmentStep : EvaluationStep
	{
		AssignmentExpression expression;
		EvaluationStep value;
		EvaluationStep target;
		TweedleFrame frame;

		public AssignmentStep(TweedleFrame frame, AssignmentExpression expression, EvaluationStep value, EvaluationStep target)
			: base(new List<ExecutionStep> { value, target })
		{
			this.frame = frame;
			this.expression = expression;
			this.value = value;
			this.target = target;
		}

		public AssignmentStep(TweedleFrame frame, AssignmentExpression expression, EvaluationStep value)
			: base(new List<ExecutionStep> { value })
		{
			this.frame = frame;
			this.expression = expression;
			this.value = value;
			target = null;
		}

		internal override bool Execute()
		{
			if (target == null)
			{
				frame.SetValue(expression.Identifier, value.Result);
			}
			else
			{
				((TweedleObject)target.Result).Set(expression.Identifier, value.Result);
			}
			return MarkCompleted();
		}
	}
}