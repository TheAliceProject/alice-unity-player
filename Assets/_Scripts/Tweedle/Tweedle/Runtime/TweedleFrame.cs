using System;
using System.Collections.Generic;

namespace Alice.Tweedle
{
	public class TweedleFrame
	{
		protected TweedleFrame Parent { get; }
		TweedleValue thisValue;

		Dictionary<TweedleValueHolderDeclaration, TweedleValue> localValues =
			new Dictionary<TweedleValueHolderDeclaration, TweedleValue>();

		protected Action<TweedleValue> nextStep;

		public TweedleFrame()//Tweedle.TweedleObject instance)
		{
		}

		internal TweedleFrame ParallelFrame(int count)
		{
			return new ParallelFrame(this, count);
		}

		protected TweedleFrame(TweedleFrame parent)
		{
			Parent = parent;
		}

		protected TweedleFrame(TweedleFrame parent, Action<TweedleValue> next)
		{
			Parent = parent;
			nextStep = next;
		}

		//internal SequentialFrame SequentialFrame(int count)
		//{
		//	return new SequentialFrame(this, count);
		//}

		public TweedleFrame ExecutionFrame(Action<TweedleValue> next)
		{
			return new TweedleFrame(this, next);
		}

		internal TweedleValue GetThis()
		{
			return thisValue;
		}

		internal void Next(TweedleValue val)
		{
			nextStep(val);
		}

		internal void Next()
		{
			nextStep(TweedleNull.NULL);
		}

		internal void SetLocalValue(TweedleValueHolderDeclaration declaration, TweedleValue tweedleValue)
		{
			// This will always go to the current frame
			if (declaration.Type.AcceptsType(tweedleValue.Type))
			{
				localValues.Add(declaration, tweedleValue);
			}
		}

		internal TweedleFrame PopMethod()
		{
			// Walk parents and return the caller
			throw new NotImplementedException();
		}

		internal TweedleFrame MethodCallFrame(TweedleValue target)
		{
			// target may be an instance (object or enumValue) or type (class or enum)
			return new TweedleFrame(this)
			{
				thisValue = target
			};
		}
	}

	/*internal class SequentialFrame : TweedleFrame
	{
		private int count;

		public SequentialFrame(TweedleFrame frame, int count)
            : base(frame)
		{
			this.count = count;
		}
	}*/

	internal class ParallelFrame : TweedleFrame
	{
		int count;

		public ParallelFrame(TweedleFrame frame, int count)
			: base(frame)
		{
			this.count = count;
			nextStep = ignored =>
			{
				this.count--;
				if (this.count == 0)
				{
					Parent.Next();
				}
			};
		}
	}
}
