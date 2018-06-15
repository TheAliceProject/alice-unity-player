using System;
using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class LocalVariableDeclaration : TweedleStatement
	{
		public TweedleLocalVariable Variable { get; }
		public bool IsConstant { get; }

		public LocalVariableDeclaration(bool isConstant, TweedleLocalVariable variable)
		{
			IsConstant = isConstant;
			Variable = variable;
		}

		public override void Execute(TweedleFrame frame, Action next)
		{
			if (Variable.Initializer != null)
			{
				Variable.Initializer.Evaluate(frame,
					val =>
					{
						frame.SetLocalValue(Variable, val);
						next();
					});
			}
			else
			{
				// TODO Require initializer and eliminate NULL as invalid
				frame.SetLocalValue(Variable, TweedleNull.NULL);
				next();
			}
		}

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			if (Variable.Initializer != null)
			{
				return new DeclarationStep(this, frame, Variable.Initializer.AsStep(frame));
			}
			else
			{
				// TODO Require initializer and eliminate NULL as invalid
				frame.SetLocalValue(Variable, TweedleNull.NULL);
				return ExecutionStep.NOOP;
			}
		}
	}

	internal class DeclarationStep : StatementStep<LocalVariableDeclaration>
	{
		EvaluationStep init;

		public DeclarationStep(LocalVariableDeclaration statement, TweedleFrame frame, EvaluationStep init)
			: base(statement, frame, init)
		{
			this.init = init;
		}

		internal override bool Execute()
		{
			frame.SetLocalValue(statement.Variable, init.Result);
			return MarkCompleted();
		}
	}
}