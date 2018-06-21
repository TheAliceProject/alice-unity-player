using System;
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

		internal override bool HasField(string fieldName)
		{
			return tweClass.Field(fieldName) != null;
		}

		internal override bool HasSetField(string fieldName)
		{
			return Attributes.ContainsKey(fieldName);
		}

		public override TweedleValue Get(string fieldName)
		{
			if (HasSetField(fieldName))
			{
				return Attributes[fieldName].Value;
			}
			if (HasField(fieldName))
			{
				throw new TweedleRuntimeException("Attempt to read uninitialized field" + this + "." + fieldName);
			}
			else
			{
				throw new TweedleRuntimeException("Attempt to read nonexistent field" + this + "." + fieldName);
			}
		}

		internal override bool Set(string fieldName, TweedleValue value, TweedleFrame frame)
		{
			TweedleField field = tweClass.Field(fieldName);
			if (field != null & field.Accepts(value, frame))
			{
				if (Attributes.ContainsKey(fieldName))
				{
					Attributes[fieldName].Value = value;
				}
				else
				{
					Attributes.Add(field.Name, new ValueHolder(field.Type.AsDeclaredType(frame), value));
				}
				return true;
			}
			return false;
		}

		ExecutionStep InitializeFieldStep(TweedleFrame frame, TweedleField field)
		{
			return new SingleInputActionStep(
				field.InitializerStep(frame),
				val => Attributes.Add(field.Name, new ValueHolder(field.Type.AsDeclaredType(frame), val)));
		}

		internal override TweedleMethod MethodNamed(TweedleFrame frame, string methodName)
		{
			return tweClass.MethodNamed(frame, methodName);
		}

		internal IEnumerable<ExecutionStep> InitializationSteps(ConstructorFrame frame)
		{
			List<ExecutionStep> steps = new List<ExecutionStep>();
			tweClass.AddInitializationSteps(steps, frame, this);
			return steps;
		}
	}
}