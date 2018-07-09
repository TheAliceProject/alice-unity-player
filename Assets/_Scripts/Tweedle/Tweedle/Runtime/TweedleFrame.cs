using System;
using System.Collections.Generic;
using Alice.VM;

namespace Alice.Tweedle
{
	public class TweedleFrame
	{
		TweedleFrame parent;
		public VirtualMachine vm;
		protected TweedleValue thisValue;
		protected internal string callStackEntry;

		Dictionary<string, ValueHolder> localValues =
			new Dictionary<string, ValueHolder>();

		public TweedleFrame(string stackEntry)
		{
			callStackEntry = stackEntry;
			vm = new VirtualMachine();
		}

		public TweedleFrame(string stackEntry, VirtualMachine vm)
		{
			this.vm = vm;
			callStackEntry = stackEntry;
		}

		protected TweedleFrame(TweedleFrame parent)
		{
			vm = parent.vm;
			this.parent = parent;
		}

		internal TweedleClass ClassNamed(string name)
		{
			return vm?.Library?.ClassNamed(name);
		}

		internal TweedleTypeDeclaration TypeNamed(string name)
		{
			return vm?.Library?.TypeNamed(name);
		}

		private TypeValue GetTypeNamed(string name)
		{
			TweedleTypeDeclaration libraryType = TypeNamed(name);
			if (libraryType != null)
				return new TypeValue(libraryType);
			// TODO Add catch for System Primitive types
			return null;
		}

		internal virtual string StackWith(string stackTop)
		{
			string stack = stackTop + "\n" + callStackEntry;
			if (parent == null)
			{
				return stack;
			}
			else
			{
				return parent.StackWith(stack);
			}
		}

		internal TweedleValue GetThis()
		{
			return thisValue;
		}

		public TweedleValue SetLocalValue(TweedleValueHolderDeclaration declaration, TweedleValue value)
		{
			localValues.Add(declaration.Name,
							new ValueHolder(declaration.Type.AsDeclaredType(this), value));
			return value;
		}

		public TweedleValue SetValue(string varName, TweedleValue value)
		{
			if (value == null)
			{
				throw new TweedleRuntimeException("Can not assign null to " + varName);
			}
			if (UpdateFrameValue(varName, value) || SetValueOnThis(varName, value))
			{
				return value;
			}
			throw new TweedleRuntimeException("Attempt to write uninitialized variable <" + varName + "> failed");
		}

		public bool UpdateFrameValue(string varName, TweedleValue value)
		{
			if (localValues.ContainsKey(varName))
			{
				// TODO handle on property objects for animation and delay
				localValues[varName].Value = value;
				return true;
			}
			else
			{
				if (parent != null)
				{
					return parent.UpdateFrameValue(varName, value);
				}
			}
			return false;
		}

		bool SetValueOnThis(string varName, TweedleValue value)
		{
			if (thisValue == null)
			{
				throw new TweedleRuntimeException("The VM is unable to write to static variables yet. Can not update <" + varName + ">");
			}
			return thisValue.Set(varName, value, this);
		}

		public TweedleValue GetValue(string varName)
		{
			TypeValue type = GetTypeNamed(varName);
			if (type != null)
			{
				return type;
			}
			if (localValues.ContainsKey(varName))
			{
				return localValues[varName].Value;
			}
			if (parent != null)
			{
				return parent.GetValue(varName);
			}
			if (thisValue != null && thisValue.HasSetField(varName))
			{
				return thisValue.Get(varName);
			}
			throw new TweedleRuntimeException("Attempt to read unassigned variable <" + varName + "> failed");
		}

		public TweedleFrame ChildFrame()
		{
			return new TweedleFrame(this);
		}

		public TweedleFrame ChildFrame(string stackEntry)
		{
			TweedleFrame child = new TweedleFrame(this);
			child.callStackEntry = stackEntry;
			return child;
		}

		internal TweedleFrame ChildFrame(string stackEntry, TweedleValueHolderDeclaration declaration, TweedleValue value)
		{
			var child = new TweedleFrame(this);
			child.SetLocalValue(declaration, value);
			child.callStackEntry = stackEntry;
			return child;
		}

		internal ConstructorFrame ForInstantiation(TweedleClass tweedleClass)
		{
			return new ConstructorFrame(this, tweedleClass);
		}

		internal MethodFrame MethodCallFrame(string methodName, bool invokeSuper)
		{
			return new MethodFrame(this, methodName, invokeSuper);
		}

		internal LambdaFrame LambdaFrame()
		{
			return new LambdaFrame(this);
		}
	}
}
