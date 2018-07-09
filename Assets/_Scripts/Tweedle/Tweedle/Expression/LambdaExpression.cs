using System;
using System.Collections.Generic;
using System.Linq;
using Alice.VM;

namespace Alice.Tweedle
{
	public class LambdaExpression : TweedleExpression
	{
		List<TweedleRequiredParameter> parameters;
		BlockStatement body;

		public List<TweedleRequiredParameter> Parameters
		{
			get { return parameters; }
		}

		internal TweedleLambdaType LambdaType()
		{
			return (TweedleLambdaType)Type;
		}

		public BlockStatement Body
		{
			get { return body; }
		}

		// TODO Extract a better return type than void from the statements
		public LambdaExpression(List<TweedleRequiredParameter> parameters, List<TweedleStatement> statements)
			: base(new TweedleLambdaType(parameters.Select(param => param.Type).ToList(), TweedleVoidType.VOID))
		{
			this.parameters = parameters;
			body = new BlockStatement(statements);
		}

		internal override NotifyingEvaluationStep AsStep(TweedleFrame frame)
		{
			return new ContextNotifyingEvaluationStep(ToTweedle(), frame, () => new TweedleLambda(this));
		}
	}
}