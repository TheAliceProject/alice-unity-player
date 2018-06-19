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

		internal override ExecutionStep AsStep(TweedleFrame frame)
		{
			return new StartStep(() =>
			{
				return new SingleInputStep(
					Variable.InitializerStep(frame),
					value => frame.SetLocalValue(Variable, value));
			});
		}
	}
}