using System.Collections.Generic;
using System.Linq;
using Alice.VM;

namespace Alice.Tweedle
{
	public class EachInArrayTogether : TweedleStatement
	{
		public TweedleLocalVariable ItemVariable { get; }
		public TweedleExpression Array { get; }
		public BlockStatement Body { get; }

		public EachInArrayTogether(TweedleLocalVariable itemVariable, TweedleExpression array, List<TweedleStatement> statements)
		{
			ItemVariable = itemVariable;
			Array = array;
			Body = new BlockStatement(statements);
		}

		internal override ExecutionStep AsStepToNotify(ExecutionScope scope, ExecutionStep next)
		{
			var arrayStep = Array.AsStep(scope);
			var bodyStep = new OperationStep(
				"EachInArrayTogether",
				scope,
				items =>
				{
					var scopes = ((TweedleArray)items).Values.Select(val => scope.ChildScope("EachInArrayTogether", ItemVariable, val)).ToList();
					Body.AddParallelSteps(scopes, next);
				});
			arrayStep.OnCompletionNotify(bodyStep);
			return arrayStep;
		}
	}
}