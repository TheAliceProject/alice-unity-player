using System;

namespace Alice.Tweedle
{
	public class TweedleFrame
	{
		protected TweedleFrame parent;
		private TweedleObject thisValue;
		//private TweedleObject messageTarget;
		//private TweedleValue returnValue = TweedleNull.NULL;

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
			this.parent = parent;
		}

		protected TweedleFrame(TweedleFrame parent, Action<TweedleValue> next)
        {
            this.parent = parent;
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

		internal TweedleObject GetThis() {
			return thisValue;
		}

		//TweedleObject GetMessageTarget() {
			//return messageTarget;
		//}

		//internal TweedleValue GetReturnValue()
		//{
		//	return returnValue;
		//}

		internal void Next(TweedleValue val)
		{
			nextStep(val);
		}

        internal void Next()
		{
			nextStep(TweedleNull.NULL);
        }

		//internal void SetReturnValue(TweedleValue tweedleValue)
		//{
		//	throw new NotImplementedException();
		//}

		internal void SetParameterValue(Type type, TweedleValue tweedleValue)
		{
			throw new NotImplementedException();
		}

		internal TweedleFrame PopMethod()
		{
			// Walk parents and return the caller
			throw new NotImplementedException();
		}

		internal TweedleFrame MethodCallFrame(TweedleMethod tweedleMethod)
		{
            return new TweedleFrame(this);
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
		private int count;

		public ParallelFrame(TweedleFrame frame, int count)
			:base(frame)
		{
			this.count = count;
			nextStep = ignored =>
			{
				this.count--;
				if (this.count == 0)
				{
					parent.Next();
				}
			};
		}

		/*private static Action<TweedleValue> CountDown()
		{
			return ignored =>
			{
				count--;
                if (count == 0)
				{
					parent.Next(ignored);
				}
			};
		}*/
	}
}
