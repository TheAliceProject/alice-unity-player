using Alice.VM;

namespace Alice.Tweedle
{
	public abstract class TweedleValue : TweedleExpression
	{
		protected TweedleValue(TweedleType type)
			: base(type)
		{
		}

		internal override NotifyingEvaluationStep AsStep(NotifyingStep parent, TweedleFrame frame)
		{
			return new NotifyingValueStep(parent?.callStack, frame, parent, this);
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

		internal virtual TweedleMethod MethodNamed(TweedleFrame frame, string methodName)
		{
			throw new TweedleRuntimeException("Can not invoke method " + methodName + " on " + this);
		}

		internal virtual TweedleMethod SuperMethodNamed(TweedleFrame frame, string methodName)
		{
			throw new TweedleRuntimeException("Can not invoke super." + methodName + " on " + this);
		}

		internal virtual bool Set(string fieldName, TweedleValue value, TweedleFrame frame)
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

	class NotifyingValueStep : NotifyingEvaluationStep
	{
		public NotifyingValueStep(string callStack, TweedleFrame frame, NotifyingStep parent, TweedleValue tweedleValue)
			: base(frame, parent)
		{
			result = tweedleValue;
			this.callStack = callStack;
		}
	}
}
