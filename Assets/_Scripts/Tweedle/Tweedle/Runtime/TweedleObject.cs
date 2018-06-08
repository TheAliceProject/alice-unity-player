﻿namespace Alice.Tweedle
{
	public class TweedleObject : TweedleValue
	{
		readonly TweedleClass tweClass;

		public TweedleObject(TweedleClass aClass)
			: base(aClass)
		{
			tweClass = aClass;
		}

		public TweedleClass GetClass()
		{
			return tweClass;
		}

		public TweedleValue Get(string fieldName)
		{
			return null;
		}

		public void Set(TweedleField field, TweedleValue value)
		{

		}

		public TweedleValue InitializeField(TweedleFrame frame, TweedleField field)
		{
			TweedleValue value = null;
			Set(field, value);
			return value;
		}

		internal override TweedleMethod MethodNamed(string methodName)
		{
			return tweClass.MethodNamed(methodName);
		}
	}
}