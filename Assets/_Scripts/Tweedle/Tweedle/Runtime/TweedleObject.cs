using System.Collections.Generic;
using Alice.VM;

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

		public TweedleValue Set(string fieldName, TweedleValue value)
		{
			Attributes[fieldName].Value = value;
			return value;
		}

		ExecutionStep InitializeFieldStep(TweedleFrame frame, TweedleField field)
		{
			return new SingleInputStep(
				field.InitializerStep(frame),
				val =>
				{
					Attributes.Add(field.Name, new ValueHolder(field.Type.AsDeclaredType(frame), val));
					return val;
				});
		}

		internal override TweedleMethod MethodNamed(TweedleFrame frame, string methodName)
		{
			return tweClass.MethodNamed(frame, methodName);
		}
	}
}