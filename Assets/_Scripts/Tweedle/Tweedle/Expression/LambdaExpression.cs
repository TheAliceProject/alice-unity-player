using System;
using System.Collections.Generic;

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

		public BlockStatement Body
		{
			get { return body; }
		}

		public LambdaExpression(List<TweedleRequiredParameter> parameters, List<TweedleStatement> statements)
			: base(TweedleVoidType.VOID)
		{
			this.parameters = parameters;
			body = new BlockStatement(statements);
		}

		public override void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			// TODO Add parameters to frame
			//body.Execute(frame.LambdaFrame());
			throw new System.NotImplementedException();
		}
	}
}