﻿using System;
using System.Collections.Generic;

namespace Alice.Tweedle.VM
{
    public class ExecutionScope
    {
        protected readonly ExecutionScope parent;
        public readonly VirtualMachine vm;
        protected TValue thisValue;
        protected internal IStackFrame callStackEntry;

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

        private Dictionary<string, ValueHolder> localValues =
            new Dictionary<string, ValueHolder>();

        public ExecutionScope(IStackFrame stackEntry)
        {
            callStackEntry = stackEntry;
            vm = new VirtualMachine();
        }

        public ExecutionScope(IStackFrame stackEntry, VirtualMachine vm)
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

        protected TTypeRef GetTypeNamed(string name)
        {
            TType libraryType = TypeNamed(name);
            return libraryType != null ? new TTypeRef(libraryType) : null;
        }

        internal virtual void StackWith(System.Text.StringBuilder stackBuilder)
        {
            stackBuilder.Append("\n");
            stackBuilder.Append(callStackEntry?.ToStackFrame());
            if (parent == null)
            {
                return;
            }
            else
            {
                parent.StackWith(stackBuilder);
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
            if (UpdateLocalValue(varName, value))
                return true;
            return UpdateParentValue(varName, value);
        }

        protected bool UpdateLocalValue(string varName, TValue value)
        {
            if (localValues.ContainsKey(varName))
            {
                //UnityEngine.Debug.Log("Updating " + varName + " to " + value.ToTextString());
                // TODO handle on property objects for animation and delay
                localValues[varName].Value = value;
                return true;
            }
            return false;
        }

        protected virtual bool UpdateParentValue(string varName, TValue value)
        {
            if (parent != null)
            {
                //UnityEngine.Debug.Log("Asking parent scope to set " + varName + " to " + value.ToTextString());
                return parent.UpdateScopeValue(varName, value);
            }

            return false;
        }

        bool SetValueOnThis(string varName, TValue value)
        {
            return thisValue != TValue.UNDEFINED && thisValue.Set(this, varName, value);
        }

        public TValue GetValue(string varName)
        {
            TValue result;
            if (TryGetTypeRef(varName, out result))
            {
                return result;
            }
            if (TryGetLocalVariable(varName, out result))
            {
                return result;
            }
            if (TryGetParentValue(varName, out result))
            {
                return result;
            }
            if (TryGetInstanceFieldValue(varName, out result))
            {
                return result;
            }
            if (TryGetStaticFieldValue(varName, out result)) {
                return result;
            }

            throw new TweedleRuntimeException("Attempt to read unassigned variable <" + varName + "> failed");
        }

        protected bool TryGetTypeRef(string varName, out TValue outValue)
        {
            TTypeRef type = GetTypeNamed(varName);
            if (type != null)
            {
                outValue = TBuiltInTypes.TYPE_REF.Instantiate(type);
                return true;
            }

            outValue = TValue.UNDEFINED;
            return false;
        }

        protected bool TryGetLocalVariable(string varName, out TValue outValue)
        {
            ValueHolder holder;
            if (localValues.TryGetValue(varName, out holder))
            {
                outValue = holder.Value;
                return true;
            }

            outValue = TValue.UNDEFINED;
            return false;
        }

        protected virtual bool TryGetParentValue(string varName, out TValue outValue)
        {
            if (parent != null)
            {
                outValue = parent.GetValue(varName);
                return true;
            }

            outValue = TValue.UNDEFINED;
            return false;
        }

        protected bool TryGetInstanceFieldValue(string varName, out TValue outValue)
        {
            if (thisValue != TValue.UNDEFINED)
            {
                try
                {
                    outValue = thisValue.Get(this, varName);
                    return true;
                }
                catch (TweedleNonexistentFieldException) { }
            }

            outValue = TValue.UNDEFINED;
            return false;
        }

        protected bool TryGetStaticFieldValue(string varName, out TValue outValue) {
            if (thisValue != TValue.UNDEFINED) {
                try {
                    TValue typeVal = TValue.FromType(thisValue.Type);
                    outValue = typeVal.Get(this, varName);
                    return true;
                } catch (TweedleNonexistentFieldException) { }
            }

            outValue = TValue.UNDEFINED;
            return false;
        }

        public ExecutionScope ChildScope()
        {
            ExecutionScope child = new ExecutionScope(this);
            child.thisValue = thisValue;
            return child;
        }

        public ExecutionScope ChildScope(IStackFrame stackEntry)
        {
            ExecutionScope child = new ExecutionScope(this);
            child.callStackEntry = stackEntry;
            child.thisValue = thisValue;
            return child;
        }

        internal ExecutionScope ChildScope(IStackFrame stackEntry, TValueHolderDeclaration declaration, TValue value)
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

        internal MethodScope MethodCallScope(string methodName, string[] inArgNames, bool invokeSuper)
        {
            return new MethodScope(this, methodName, inArgNames, invokeSuper);
        }

        internal virtual TType CallingType(string methodName)
        {
            return parent != null ? parent.CallingType(methodName) : null;
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