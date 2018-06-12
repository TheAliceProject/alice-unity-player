using System;

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

		override public void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			next(frame.GetValue(Name));
		}
	}
}