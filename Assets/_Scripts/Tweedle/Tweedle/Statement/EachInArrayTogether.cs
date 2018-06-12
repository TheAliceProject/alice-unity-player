using System;
using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class EachInArrayTogether : TweedleStatement
	{
		TweedleLocalVariable itemVariable;
		TweedleExpression array;
		BlockStatement body;

		public TweedleLocalVariable ItemVariable
		{
			get { return itemVariable; }
		}

		public TweedleExpression Array
		{
			get { return array; }
		}

		public BlockStatement Body
		{
			get { return body; }
		}

		public EachInArrayTogether(TweedleLocalVariable itemVariable, TweedleExpression array, List<TweedleStatement> statements)
		{
			this.itemVariable = itemVariable;
			this.array = array;
			body = new BlockStatement(statements);
		}

		public override void Execute(TweedleFrame frame, Action next)
		{
			// TODO make frames for each statement with values
			body.ExecuteInParallel(frame, next);
		}
	}
}