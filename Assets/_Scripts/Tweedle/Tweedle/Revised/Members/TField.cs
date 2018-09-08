using System.Collections.Generic;
using Alice.Tweedle.Parse;
using Alice.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Generic get/set field
    /// </summary>
    public abstract class TField : ITypeMember, IValueHolderDeclaration
    {
        #region ITypeMember

        public string Name { get; private set; }
        public TTypeRef Type { get; private set; }
        public MemberFlags Flags { get; private set; }

        public virtual void Resolve(TweedleSystem inSystem, TType inOwnerType)
        {
            Type.Resolve(inSystem);

            if (!Type.Get().IsValidIdentifier())
                throw new TweedleLinkerException("Field type " + Type.Name + " is not a valid field type");
        }

        #endregion // ITypeMember

        #region IValueHolderDeclaration

        public virtual string ToTweedle()
        {
            return Type.ToString() + " " + Name;
        }

        #endregion // IValueHolderDeclaration

        #region Construction

        protected void SetupMember(string inName, TTypeRef inType, MemberFlags inFlags)
        {
            Name = inName;
            Type = inType;
            Flags = inFlags | MemberFlags.Field;
        }

        #endregion // Construction

        /// <summary>
        /// Returns if this is a static field.
        /// </summary>
        public bool IsStatic()
        {
            return this.HasModifiers(MemberFlags.Static);
        }

        public abstract ExecutionStep InitializeStep(ExecutionScope inScope, ref TValue inValue);
        public abstract TValue Get(ExecutionScope inScope, ref TValue inValue);
        public abstract void Set(ExecutionScope inScope, ref TValue inValue, TValue inNewValue);

        // Checks if the potential assignment is valid
        protected void CheckSet(ExecutionScope inScope, ref TValue inNewValue)
        {
            if (this.HasModifiers(MemberFlags.Readonly) && (!inScope.HasPermissions(ScopePermissions.WriteReadonlyFields) || inScope.GetThis() != inNewValue))
            {
                throw new TweedleRuntimeException("Unable to write readonly field " + Name);
            }

            TType concreteType = Type.Get(inScope);
            if (!inNewValue.Type.CanCast(Type.Get(inScope)))
            {
                throw new TweedleRuntimeException("Unable to treat " + inNewValue + " as type " + concreteType);
            }
        }

        static public readonly TField[] EMPTY_ARRAY = new TField[0];
    }
}