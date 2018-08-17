using Alice.VM;

namespace Alice.Tweedle
{
	public abstract class TweedleValue : TweedleExpression
	{
		protected TweedleValue(TweedleType type)
			: base(type)
		{
		}

		internal override ExecutionStep AsStep(ExecutionScope scope)
		{
			return new ValueStep("", scope, this);
		}

		internal double ToDouble()
		{
			return Type.ValueToDouble(this);
		}

		internal int ToInt()
		{
			return Type.ValueToInt(this);
		}

		internal string ToTextString()
		{
			return Type.ValueToString(this);
		}

		internal bool ToBoolean()
		{
			return Type.ValueToBoolean(this);
		}

		internal virtual TweedleMethod MethodNamed(ExecutionScope scope, string methodName)
		{
			throw new TweedleRuntimeException("Can not invoke method " + methodName + " on " + this);
		}

		internal virtual TweedleMethod SuperMethodNamed(ExecutionScope scope, string methodName)
		{
			throw new TweedleRuntimeException("Can not invoke super." + methodName + " on " + this);
		}

		internal virtual bool Set(string fieldName, TweedleValue value, ExecutionScope scope)
		{
			return false;
		}

		internal virtual bool HasField(string fieldName)
		{
			return false;
		}

		internal virtual bool HasSetField(string fieldName)
		{
			return false;
		}

		public virtual TweedleValue Get(string fieldName)
		{
			throw new TweedleRuntimeException(this + " is not an Object. Can not access field " + fieldName);
		}
	}

	class ValueStep : ExecutionStep
	{
		public ValueStep(string callStackEntry, ExecutionScope scope, TweedleValue tweedleValue)
			: base(scope)
		{
			result = tweedleValue;
			this.callStack = scope.StackWith(callStackEntry);
		}
	}
}
