using System;
using Alice.VM;

namespace Alice.Tweedle
{
	public class IdentifierReference : TweedleExpression
	{
		public string Name { get; }

		public IdentifierReference(string name)
			: base(null)
		{
			Name = name;
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			return new SimpleStep(() => frame.GetValue(Name));
		}
	}
}