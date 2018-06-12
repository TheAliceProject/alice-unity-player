using System;

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
	}
}