using System;
using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class ReturnStatement : TweedleStatement
	{
		TweedleExpression expression;

		public TweedleExpression Expression
		{
			get { return expression; }
		}

		public TweedleType Type
		{
			get { return expression.Type; }
		}

		public ReturnStatement()
		{
			expression = TweedleNull.NULL;
		}

		public ReturnStatement(TweedleExpression expression)
		{
			this.expression = expression;
		}

		public override void Execute(TweedleFrame frame, Action next)
		{
			// Ignores next since method is finished
			expression.Evaluate(frame, val => ((InvocationFrame)frame).Complete(val));
		}

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			return new ReturnStep(this, frame, expression.AsStep(frame));
		}
	}

	internal class ReturnStep : StatementStep<ReturnStatement>
	{
		EvaluationStep returnValueStep;

		public ReturnStep(ReturnStatement statement, TweedleFrame frame, EvaluationStep returnValueStep)
			: base(statement, frame, returnValueStep)
		{
			this.returnValueStep = returnValueStep;
		}

		internal override bool Execute()
		{
			// TODO Make this work
			((InvocationFrame)frame).Complete(returnValueStep.Result);
			return MarkCompleted();
		}
	}
}