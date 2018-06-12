using System;
using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class TweedleFrame
	{
		TweedleFrame parent;
		VirtualMachine vm;
		protected TweedleValue thisValue;

		Dictionary<string, ValueHolder> localValues =
			new Dictionary<string, ValueHolder>();

        public TweedleFrame(VirtualMachine vm)
        {
            this.vm = vm;
		}

        protected TweedleFrame(TweedleFrame parent)
        {
            vm = parent.vm;
			this.parent = parent;
		}

		internal TweedleClass ClassNamed(string name)
		{
			return vm.Library.ClassNamed(name);
		}

		internal TweedleTypeDeclaration TypeNamed(string name)
		{
			return vm.Library.TypeNamed(name);
		}

        internal TweedleValue GetThis()
        {
            return thisValue;
        }

		public void SetLocalValue(TweedleValueHolderDeclaration declaration)
		{
			declaration.InitializeValue(this, val => SetLocalValue(declaration, val));
		}

		public void SetLocalValue(TweedleValueHolderDeclaration declaration, TweedleValue tweedleValue)
		{
			localValues.Add(declaration.Name,
			                new ValueHolder(declaration.Type.AsDeclaredType(this), tweedleValue));
		}

		public void SetValue(string varName, TweedleValue value, Action<TweedleValue> next)
		{
			if (localValues.ContainsKey(varName))
			{
				localValues[varName].Value = value;
				// TODO hand next in to property objects for animation and delay
				next(value);
			}
			else
			{
				if (parent != null)
				{
					parent.SetValue(varName, value, next);
				}
				else
				{
					throw new TweedleRuntimeException("Attempt to write uninitialized variable <" + varName + "> failed");
				}
			}
		}

		public TweedleValue GetValue(string varName)
		{
			if (localValues.ContainsKey(varName))
			{
				return localValues[varName].Value;
			}
			if (parent != null)
			{
				return parent.GetValue(varName);
			}
			throw new TweedleRuntimeException("Attempt to read unassigned variable <" + varName + "> failed");
		}

        public TweedleFrame ChildFrame()
        {
            return new TweedleFrame(this);
        }

        internal virtual ConstructorFrame ForInstantiation(TweedleClass tweedleClass, Action<TweedleValue> next)
        {
            return new ConstructorFrame(vm, tweedleClass, next);
        }

		internal TweedleFrame MethodCallFrame(TweedleValue target, Action<TweedleValue> next)
		{
            return new MethodFrame(vm, target, next);
		}
	}
}
