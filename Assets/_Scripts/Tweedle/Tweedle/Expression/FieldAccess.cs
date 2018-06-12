using System;

namespace Alice.Tweedle
{
	public class FieldAccess : MemberAccessExpression
	{
		public string FieldName { get; }

		public FieldAccess(TweedleExpression target, string fieldName)
			: base(target)
		{
			FieldName = fieldName;
		}

		override public void Evaluate(TweedleFrame frame, Action<TweedleValue> next)
		{
			base.Evaluate(frame, value =>
			{
				if (value is TweedleObject)
				{
					TweedleObject obj = (TweedleObject)value;
					next(obj.Get(FieldName));
				}
				else
				{
					throw new TweedleRuntimeException(value + " is not an Object. Can not access field " + FieldName);
				}
			}
			);
		}
	}
}