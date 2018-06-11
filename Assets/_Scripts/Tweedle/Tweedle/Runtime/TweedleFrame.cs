using System;
using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class TweedleFrame
	{
		protected TweedleFrame Parent { get; }
		protected VirtualMachine VirtualMachine { get; }
		TweedleValue thisValue;

		Dictionary<string, ValueHolder> localValues =
			new Dictionary<string, ValueHolder>();

		internal TweedleClass ClassNamed(string name)
		{
			return VirtualMachine.Library.ClassNamed(name);
		}

		internal TweedleTypeDeclaration TypeNamed(string name)
		{
			return VirtualMachine.Library.TypeNamed(name);
		}

		protected Action<TweedleValue> nextStep;

		public TweedleFrame(VirtualMachine vm)
		{
			VirtualMachine = vm;
		}

		internal TweedleFrame ParallelFrame(int count)
		{
			return new ParallelFrame(this, count);
		}

		internal virtual ConstructorFrame ForInstantiation()
		{
			return new ConstructorFrame(this);
		}

		protected TweedleFrame(TweedleFrame parent)
		{
			VirtualMachine = parent.VirtualMachine;
			Parent = parent;
		}

		protected TweedleFrame(TweedleFrame parent, Action<TweedleValue> next)
		{
			VirtualMachine = parent.VirtualMachine;
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
			if (nextStep == null)
			{
				Parent.Next(val);

			}
			else
			{
				nextStep(val);
			}
		}

		internal virtual void Next()
		{
			Next(TweedleNull.NULL);
		}

		public void SetLocalValue(TweedleValueHolderDeclaration declaration)
		{
			declaration.InitializeValue(this.ExecutionFrame(val => SetLocalValue(declaration, val)));
		}

		public void SetLocalValue(TweedleValueHolderDeclaration declaration, TweedleValue tweedleValue)
		{
			localValues.Add(declaration.Name, new ValueHolder(declaration.Type, tweedleValue));
		}

		public void SetValue(string varName, TweedleValue value)
		{
			if (localValues.ContainsKey(varName))
			{
				localValues[varName].Value = value;
				return;
			}
			if (Parent != null)
			{
				Parent.SetValue(varName, value);
				return;
			}
			throw new TweedleRuntimeException("Attempt to write uninitialized variable <" + varName + "> failed");
		}

		public TweedleValue GetValue(string varName)
		{
			if (localValues.ContainsKey(varName))
			{
				return localValues[varName].Value;
			}
			if (Parent != null)
			{
				return Parent.GetValue(varName);
			}
			throw new TweedleRuntimeException("Attempt to read unassigned variable <" + varName + "> failed");
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

	internal class ConstructorFrame : TweedleFrame
	{
		internal TweedleObject instance;
		internal TweedleClass highestClass;

		public ConstructorFrame(TweedleFrame frame)
			: base(frame)
		{ }

		internal override ConstructorFrame ForInstantiation()
		{
			return this;
		}

		internal override void Next()
		{
			Next(instance);
		}
	}
}
