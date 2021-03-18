using System;
using System.Collections.Generic;
using Alice.Tweedle.Parse;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Reference to a TType.
    /// Can be linked directly or by name.
    /// </summary>
    public sealed class TTypeRef : IEquatable<TTypeRef>, IEquatable<TType>
    {
        /// <summary>
        /// Type name.
        /// </summary>
        public readonly string Name;

        // Linked type
        private TType m_Type;
        private bool m_IsArray;

        /// <summary>
        /// Unlinked type reference.
        /// </summary>
        public TTypeRef(string inName)
        {
            Name = inName;
        }

        /// <summary>
        /// Linked type reference.
        /// </summary>
        public TTypeRef(TType inType)
        {
            Name = inType.Name;
            m_Type = inType;
        }

        /// <summary>
        /// Returns if this type reference has been resolved.
        /// </summary>
        public bool IsResolved()
        {
            return m_Type != null;
        }

        /// <summary>
        /// Returns the linked type.
        /// </summary>
        public TType Get()
        {
            if (m_Type == null)
            {
                throw new TweedleLinkException("Type " + Name + " not linked");
            }
            return m_Type;
        }

        /// <summary>
        /// Links to the type if necessary,
        /// and returns the linked type.
        /// </summary>
        public TType Get(ExecutionScope inScope)
        {
            if (m_Type == null)
            {
                m_Type = inScope.TypeNamed(Name);
                if (m_Type == null)
                    throw new TweedleLinkException("Unable to link " + Name + " - type not found");
            }
            return m_Type;
        }

        /// <summary>
        /// If unlinked, links this reference to its type and outputs the type.
        /// Returns if the reference had been unlinked.
        /// </summary>
        public bool Resolve(TAssembly inAssembly, out TType outType)
        {
            if (m_Type == null)
            {
                m_Type = outType = inAssembly.TypeNamed(Name);
                if (outType == null)
                    throw new TweedleLinkException("Unable to link " + Name + " - type not found");
                return true;
            }
            outType = m_Type;
            return false;
        }

        /// <summary>
        /// If unlinked, links the reference to its type.
        /// Returns if the reference had been unlinked.
        /// </summary>
        public bool Resolve(TAssembly inAssembly)
        {
            TType unused;
            return Resolve(inAssembly, out unused);
        }

        #region Implicit Conversions

        public static explicit operator TType(TTypeRef inRef)
        {
            return inRef?.Get();
        }

        public static implicit operator TTypeRef(TType inType)
        {
            if (inType == null)
                return null;
            return inType.SelfRef;
        }

        #endregion // Implicit Conversions

        #region IEquatable

        public bool Equals(TTypeRef other)
        {
            return !object.ReferenceEquals(other, null) && other.Name == Name;
        }

        public bool Equals(TType other)
        {
            return !object.ReferenceEquals(other, null) && other == Get();
        }

        #endregion // IEquatable
        
        #region Overrides

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            TTypeRef typeRef = obj as TTypeRef;
            if (!object.ReferenceEquals(typeRef, null))
                return Equals(typeRef);
            TType type = obj as TType;
            if (!object.ReferenceEquals(type, null))
                return Equals(type);
            return false;
        }

        public override string ToString()
        {
            return Name;
        }

        static public bool operator==(TTypeRef inLeft, TTypeRef inRight)
        {
            if (object.ReferenceEquals(inLeft, null))
                return object.ReferenceEquals(inRight, null);
            return inLeft.Equals(inRight);
        }

        static public bool operator!=(TTypeRef inLeft, TTypeRef inRight)
        {
            if (object.ReferenceEquals(inLeft, null))
                return !object.ReferenceEquals(inRight, null);
            return !inLeft.Equals(inRight);
        }

        #endregion // Overrides
    }
}