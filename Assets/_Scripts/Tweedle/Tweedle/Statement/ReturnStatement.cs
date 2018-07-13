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

		internal override ExecutionStep AsStepToNotify(ExecutionScope scope, ExecutionStep next)
		{
			var valStep = expression.AsStep(scope);
			var returnStep = new OperationStep(
				"return " + expression.ToTweedle(),
				scope,
				result => ((InvocationScope)scope).Return(result));
			valStep.OnCompletionNotify(returnStep);
			returnStep.OnCompletionNotify(next);
			return valStep;
		}
	}
}