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
			return tweClass.Field(null, fieldName) != null;
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

		internal override bool Set(string fieldName, TweedleValue value, ExecutionScope scope)
		{
			TweedleField field = tweClass.Field(scope, fieldName);
			if (field != null)
			{
				if (!field.Accepts(value, scope))
				{
					throw new TweedleRuntimeException("Invalid type. Can not assign " + value + " to " + fieldName);
				}
				if (Attributes.ContainsKey(fieldName))
				{
					Attributes[fieldName].Value = value;
				}
				else
				{
					Attributes.Add(field.Name, new ValueHolder(field.Type.AsDeclaredType(scope), value));
				}
				return true;
			}
			return false;
		}

		internal override TweedleMethod MethodNamed(ExecutionScope scope, string methodName)
		{
			return tweClass.MethodNamed(scope, methodName);
		}

		internal override TweedleMethod SuperMethodNamed(ExecutionScope scope, string methodName)
		{
			return tweClass.SuperClass(scope).MethodNamed(scope, methodName);
		}

		internal IEnumerable<ExecutionStep> InitializationNotifyingSteps(ExecutionScope scope)
		{
			List<ExecutionStep> steps = new List<ExecutionStep>();
			tweClass.AddInitializationSteps(steps, scope, this);
			return steps;
		}
	}
}