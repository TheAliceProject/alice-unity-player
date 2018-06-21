using System;
using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public abstract class TweedleValue : TweedleExpression
	{
		protected TweedleValue(TweedleType type)
			: base(type)
		{
		}

		internal override EvaluationStep AsStep(TweedleFrame frame)
		{
			return new ValueStep(this);
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

		internal virtual bool Set(string varName, TweedleValue value)
		{
			return false;
		}
	}

	internal class ValueStep : EvaluationStep
	{
		public ValueStep(TweedleValue tweedleValue)
		{
			result = tweedleValue;
			MarkCompleted();
		}

		internal override bool Execute()
		{
			return true;
		}
	}
}
