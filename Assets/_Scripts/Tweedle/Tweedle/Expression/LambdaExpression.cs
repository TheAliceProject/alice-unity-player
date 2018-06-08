using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class LambdaExpression : TweedleExpression
	{
		private List<TweedleRequiredParameter> parameters;
		private List<TweedleStatement> statements;

		public List<TweedleRequiredParameter> Parameters
		{
			get { return parameters; }
		}

		public List<TweedleStatement> Statements
		{
			get { return statements; }
		}

		public LambdaExpression(List<TweedleRequiredParameter> parameters, List<TweedleStatement> statements)
			: base(TweedleVoidType.VOID)
		{
			this.parameters = parameters;
			this.statements = statements;
		}

		public override TweedleValue Evaluate(TweedleFrame frame)
		{
			throw new System.NotImplementedException();
		}
	}
}