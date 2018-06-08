namespace Alice.Tweedle
{
	public class FieldAccess : MemberAccessExpression
	{
		string fieldName;

		public string FieldName
		{
			get { return fieldName; }
		}

		public FieldAccess(TweedleExpression target, string fieldName)
			: base(target)
		{
			this.fieldName = fieldName;
		}

		override public void Evaluate(TweedleFrame frame)
		{
			base.Evaluate(frame.ExecutionFrame(value =>
			{
				if (value is TweedleObject)
				{
					TweedleObject obj = (TweedleObject)value;
					frame.Next(obj.Get(fieldName));
				}
				else
				{
					throw new TweedleRuntimeException(value + " is not an Object. Can not access field " + fieldName);
				}
			}
			));
		}
	}
}