using System;
using System.Collections.Generic;
using System.Reflection;
using Alice.Tweedle.Interop;

namespace Alice.Tweedle
{
    /// <summary>
    /// Proxy class type.
    /// </summary>
    public sealed class TPObjectType : TType
    {
        private TField[] m_Fields;
        private TMethod[] m_Methods;
        private TMethod[] m_Constructors;

        private Type m_Type;

        public TPObjectType(Type inType)
            : base(TConvert.TTypeNameForType(inType), ParseBaseTypeRef(inType.BaseType))
        {
            m_Type = inType;
            
            m_Fields = ParseFields();
            m_Methods = ParseMethods();
            m_Constructors = ParseConstructors();
        }

        #region Parsing
        
        static private TTypeRef ParseBaseTypeRef(Type inBaseType)
        {
            if (inBaseType == null)
                return null;

            if (PInteropTypeAttribute.IsDefined(inBaseType))
                return TConvert.TTypeFor(inBaseType);

            return null;
        }

        private TField[] ParseFields()
        {
            List<TField> tFields = new List<TField>();
            
            var pFields = m_Type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach(var pField in pFields)
            {
                // UnityEngine.Debug.Log("Parsing field " + pField.Name);
                if (PInteropFieldAttribute.IsDefined(pField))
                    tFields.Add(new PField(pField));
            }

            var pProps = m_Type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach(var pProp in pProps)
            {
                if (PInteropFieldAttribute.IsDefined(pProp))
                    tFields.Add(new PProperty(pProp));
            }

            return tFields.ToArray();
        }

        private TMethod[] ParseMethods()
        {
            List<TMethod> tMethods = new List<TMethod>();

            var pMethods = m_Type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach(var pMethod in pMethods)
            {
                if (PInteropMethodAttribute.IsDefined(pMethod))
                    tMethods.Add(new PMethod(pMethod));
            }

            return tMethods.ToArray();
        }

        private TMethod[] ParseConstructors()
        {
            List<TMethod> tConstructors = new List<TMethod>();

            var pConstructors = m_Type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach(var pConstructor in pConstructors)
            {
                if (PInteropConstructorAttribute.IsDefined(pConstructor))
                    tConstructors.Add(new PConstructor(pConstructor));
            }
            return tConstructors.ToArray();
        }

        #endregion // Parsing

        #region Object Semantics

        public override TField Field(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            AssertValueIsTypeOrTypeRef(ref inValue);
            TField field = FindMember(m_Fields, inName, inFlags | MemberFlags.Field);
            if (field == null && SuperType != null)
                field = SuperType.Get(inScope).Field(inScope, ref inValue, inName, inFlags);
            return field;
        }

        public override TMethod Method(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            AssertValueIsTypeOrTypeRef(ref inValue);
            TMethod method = FindMember(m_Methods, inName, inFlags | MemberFlags.Method);
            if (method == null && SuperType != null)
                method = SuperType.Get(inScope).Method(inScope, ref inValue, inName, inFlags);
            return method;
        }

        public override TMethod Constructor(ExecutionScope inScope, NamedArgument[] inArguments)
        {
            return FindMethodWithArgs(m_Constructors, TMethod.ConstructorName, inArguments, MemberFlags.Instance | MemberFlags.Constructor);
        }

        public override bool IsReferenceType()
        {
            return true;
        }

        public override TField[] Fields(ExecutionScope inScope, ref TValue inValue)
        {
            return m_Fields;
        }

        public override TMethod[] Methods(ExecutionScope inScope, ref TValue inValue)
        {
            return m_Methods;
        }

        public override TMethod[] Constructors(ExecutionScope inScope)
        {
            return m_Constructors;
        }

        #endregion // Object Semantics
        
        #region Internal

        protected override void Finalize(Parse.TweedleSystem inSystem)
        {
            base.Finalize(inSystem);

            ResolveMembers(m_Fields, inSystem, this);
            ResolveMembers(m_Methods, inSystem, this);
            ResolveMembers(m_Constructors, inSystem, this);
        }

        #endregion // Internal

        #region Lifecycle

        public override bool CanInstantiateDefault()
        {
            return false;
        }

        public override TValue Instantiate()
        {
            return TValue.UNDEFINED;
        }

        public override TValue DefaultValue()
        {
            return TValue.NULL;
        }

        #endregion // Lifecycle

        public override bool Equals(ref TValue inValA, ref TValue inValB)
        {
            return base.Equals(ref inValA, ref inValB)
                && inValA.Object() == inValB.Object();
        }

        public override bool LessThan(ref TValue inValA, ref TValue inValB)
        {
            return false;
        }

        #region Tweedle Casting

        #endregion // Tweedle Casting

        #region Conversion Semantics

        public override object ConvertToPObject(ref TValue inValue)
        {
            return inValue.RawObject<object>();
        }

        #endregion // Conversion Semantics

        #region Misc

        public override bool IsValidIdentifier()
        {
            return true;
        }

        public override int GetHashCode(ref TValue inValue)
        {
            return inValue.RawObject<object>().GetHashCode();
        }

        public override string ToTweedle(ref TValue inValue)
        {
            return Name;
        }

        #endregion // Misc
    }
}