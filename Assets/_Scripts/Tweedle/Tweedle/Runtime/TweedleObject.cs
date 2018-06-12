using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleObject : TweedleValue
	{
		readonly TweedleClass tweClass;

		internal Dictionary<string, ValueHolder> Attributes { get; }

		public TweedleObject(TweedleClass aClass)
			: base(aClass)
		{
			tweClass = aClass;
			Attributes = new Dictionary<string, ValueHolder>();
		}

		public TweedleClass GetClass()
		{
			return tweClass;
		}

		public TweedleValue Get(string fieldName)
		{
			return Attributes[fieldName].Value;
		}

		public void Set(string fieldName, TweedleValue value)
		{
			Attributes[fieldName].Value = value;
		}

		public void InitializeField(TweedleFrame frame, TweedleField field)
		{
			field.InitializeValue(frame, val =>
								  Attributes.Add(field.Name,
												 new ValueHolder(field.Type.AsDeclaredType(frame), val)));
		}

		internal override TweedleMethod MethodNamed(TweedleFrame frame, string methodName)
		{
			return tweClass.MethodNamed(frame, methodName);
		}
	}
}