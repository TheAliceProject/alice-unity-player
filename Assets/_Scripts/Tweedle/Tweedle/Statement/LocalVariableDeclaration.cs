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

		public override void Execute(TweedleFrame frame)
		{
			if (Variable.Initializer != null)
			{
				Variable.Initializer.Evaluate(frame.ExecutionFrame(
					val =>
					{
						frame.SetLocalValue(Variable, val);
						frame.Next();
					}));
			}
			else
			{
				// TODO Require initializer and eliminate NULL as invalid
				frame.SetLocalValue(Variable, TweedleNull.NULL);
				frame.Next();
			}
		}
	}
}