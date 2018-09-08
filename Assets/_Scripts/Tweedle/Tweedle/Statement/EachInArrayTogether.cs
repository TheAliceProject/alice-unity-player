using System.Collections.Generic;
using System.Linq;
using Alice.VM;

namespace Alice.Tweedle
{
	public class EachInArrayTogether : TweedleStatement
	{
		public TLocalVariable ItemVariable { get; }
		public ITweedleExpression Array { get; }
		public BlockStatement Body { get; }

		public EachInArrayTogether(TLocalVariable itemVariable, ITweedleExpression array, TweedleStatement[] statements)
		{
			ItemVariable = itemVariable;
			Array = array;
			Body = new BlockStatement(statements);
		}

		internal override ExecutionStep AsStepToNotify(ExecutionScope scope, ExecutionStep next)
		{
			var arrayStep = Array.AsStep(scope);
			var bodyStep = new ValueOperationStep(
				"EachInArrayTogether",
				scope,
				items =>
				{
					var scopes = items.Array().Select(val => scope.ChildScope("EachInArrayTogether", ItemVariable, val)).ToList();
					Body.AddParallelSteps(scopes, next);
				});
			arrayStep.OnCompletionNotify(bodyStep);
			return arrayStep;
		}
	}
}