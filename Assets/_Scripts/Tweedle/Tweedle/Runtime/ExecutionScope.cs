using System;
using System.Collections.Generic;

namespace Alice.Tweedle.VM
{
    public class ExecutionScope
    {
        protected readonly ExecutionScope parent;
        public readonly VirtualMachine vm;
        protected TValue thisValue;
        protected internal string callStackEntry;

        protected virtual ScopePermissions GetPermissions()
        {
            return ScopePermissions.None;
        }

        internal bool HasPermissions(ScopePermissions inPermissions)
        {
            if ((GetPermissions() & inPermissions) == inPermissions)
            {
                // Early exit if permissions are located on this scope
                return true;
            }
            if (parent != null)
            {
                // Otherwise we can traverse back up the scope stack
                // Since InvocationScopes don't use parent for their calling scope,
                // this doesn't pose the risk of permissions creeping into scopes where it isn't valid
                return parent.HasPermissions(inPermissions);
            }
            return false;
        }

        Dictionary<string, ValueHolder> localValues =
            new Dictionary<string, ValueHolder>();

        public ExecutionScope(string stackEntry)
        {
            callStackEntry = stackEntry;
            vm = new VirtualMachine();
        }

        public ExecutionScope(string stackEntry, VirtualMachine vm)
        {
            this.vm = vm;
            callStackEntry = stackEntry;
        }

        protected ExecutionScope(ExecutionScope parent)
        {
            vm = parent.vm;
            this.parent = parent;
        }

        internal TType TypeNamed(string name)
        {
            return vm?.Library?.TypeNamed(name);
        }

        private TTypeRef GetTypeNamed(string name)
        {
            TType libraryType = TypeNamed(name);
            return libraryType != null ? new TTypeRef(libraryType) : null;
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

        internal TValue GetThis()
        {
            return thisValue;
        }

        public TValue SetLocalValue(TValueHolderDeclaration declaration, TValue value)
        {
            //UnityEngine.Debug.Log("Initializing " + declaration.Name + " to " + value.ToTextString());
            localValues.Add(declaration.Name,
                new ValueHolder(declaration.Type.Get(this), value));
            return value;
        }

        public TValue SetValue(string varName, TValue value)
        {
            if (value == TValue.UNDEFINED)
            {
                throw new TweedleRuntimeException("Can not assign null to " + varName);
            }
            if (UpdateScopeValue(varName, value) || SetValueOnThis(varName, value))
            {
                return value;
            }
            throw new TweedleRuntimeException("Attempt to write uninitialized variable <" + varName + "> failed");
        }

        public bool UpdateScopeValue(string varName, TValue value)
        {
            if (localValues.ContainsKey(varName))
            {
                //UnityEngine.Debug.Log("Updating " + varName + " to " + value.ToTextString());
                // TODO handle on property objects for animation and delay
                localValues[varName].Value = value;
                return true;
            }
            else
            {
                if (parent != null)
                {
                    //UnityEngine.Debug.Log("Asking parent scope to set " + varName + " to " + value.ToTextString());
                    return parent.UpdateScopeValue(varName, value);
                }
            }
            return false;
        }

        bool SetValueOnThis(string varName, TValue value)
        {
            return thisValue != TValue.UNDEFINED && thisValue.Set(this, varName, value);
        }

        public TValue GetValue(string varName)
        {
            TTypeRef type = GetTypeNamed(varName);
            if (type != null)
            {
                return TBuiltInTypes.TYPE_REF.Instantiate(type);
            }
            if (localValues.ContainsKey(varName))
            {
                //UnityEngine.Debug.Log("Reading local " + varName + " as " + localValues[varName].Value.ToTextString());
                return localValues[varName].Value;
            }
            if (parent != null)
            {
                //UnityEngine.Debug.Log("Asking parent for " + varName);
                return parent.GetValue(varName);
            }
            if (thisValue != TValue.UNDEFINED)
            {
                try
                {
                    return thisValue.Get(this, varName);
                }
                catch (TweedleNonexistentFieldException) { }
            }
            throw new TweedleRuntimeException("Attempt to read unassigned variable <" + varName + "> failed");
        }

        public ExecutionScope ChildScope()
        {
            ExecutionScope child = new ExecutionScope(this);
            child.thisValue = thisValue;
            return child;
        }

        public ExecutionScope ChildScope(string stackEntry)
        {
            ExecutionScope child = new ExecutionScope(this);
            child.callStackEntry = stackEntry;
            child.thisValue = thisValue;
            return child;
        }

        internal ExecutionScope ChildScope(string stackEntry, TValueHolderDeclaration declaration, TValue value)
        {
            var child = new ExecutionScope(this);
            child.SetLocalValue(declaration, value);
            child.callStackEntry = stackEntry;
            child.thisValue = thisValue;
            return child;
        }

        internal ConstructorScope InstantiationScope(TType inType)
        {
            return new ConstructorScope(this, inType);
        }

        internal ConstructorScope EnumInstantiationScope(TEnumType inEnumType, TEnumValueInitializer inValueInitializer)
        {
            return new ConstructorScope(this, inEnumType, inValueInitializer);
        }

        internal StaticConstructorScope StaticInstantiationScope(TType inType)
        {
            return new StaticConstructorScope(this, inType);
        }

        internal MethodScope MethodCallScope(string methodName, bool invokeSuper)
        {
            return new MethodScope(this, methodName, invokeSuper);
        }

        internal LambdaScope LambdaScope()
        {
            return new LambdaScope(this);
        }
    
        internal virtual bool ShouldExit()
        {
            return parent != null ? parent.ShouldExit() : false;
        }

        internal virtual void Return(TValue returnValue)
        {
            parent?.Return(returnValue);
        }
    }
}