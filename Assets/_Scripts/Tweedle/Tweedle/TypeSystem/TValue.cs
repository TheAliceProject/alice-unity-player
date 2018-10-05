using System;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    public struct TValue : ITweedleExpression, IEquatable<TValue>, IComparable<TValue>
    {
        /// <summary>
        /// Concrete type representing the value.
        /// </summary>
        public TType Type { get { return m_Type; } }

        private readonly TType m_Type;
        private readonly double m_NumberValue;
        private readonly Object m_ReferenceValue;

        public TValue(TType inType)
            : this(inType, double.NaN, null)
        {
        }

        private TValue(TType inType, double inNumberValue, Object inReferenceValue)
        {
            if (inType == null)
                throw new ArgumentNullException("inType");
                
            m_Type = inType;
            m_NumberValue = inNumberValue;
            m_ReferenceValue = inReferenceValue;
        }

        #region Conversions

        public double ToDouble()
        {
            return m_Type.ConvertToDouble(ref this);
        }

        public int ToInt()
        {
            return m_Type.ConvertToInt(ref this);
        }

        public string ToTextString()
        {
            return m_Type.ConvertToString(ref this);
        }

        public bool ToBoolean()
        {
            return m_Type.ConvertToBoolean(ref this);
        }

        public Object ToPObject()
        {
            return m_Type.ConvertToPObject(ref this);
        }

        #endregion // Conversions

        #region Object Semantics

        public TMethod MethodNamed(ExecutionScope inScope, string inMethodName)
        {
            return m_Type.Method(inScope, ref this, inMethodName, MemberFlags.Instance);
        }

        public TValue Get(ExecutionScope inScope, string inFieldName)
        {
            var field = m_Type.Field(inScope, ref this, inFieldName, MemberFlags.Instance);
            if (field != null)
            {
                return field.Get(inScope, ref this);
            }
            throw new TweedleNonexistentFieldException("Unable to find field " + inFieldName);
        }

        public bool Set(ExecutionScope inScope, string inFieldName, TValue inValue)
        {
            var field = m_Type.Field(inScope, ref this, inFieldName, MemberFlags.Instance);
            if (field != null)
            {
                field.Set(inScope, ref this, inValue);
                return true;
            }
            return false;
        }

        #endregion // Object Semantics

        #region Nested Objects

        public TObject Object()
        {
            TObject obj = m_ReferenceValue as TObject;
            if (obj == null)
                obj = ((TEnum)m_ReferenceValue).Object;
            return obj;
        }

        public TArray Array()
        {
            return (TArray)m_ReferenceValue;
        }

        public TLambda Lambda()
        {
            return (TLambda)m_ReferenceValue;
        }

        public TEnum Enum()
        {
            return (TEnum)m_ReferenceValue;
        }

        public TTypeRef TypeRef()
        {
            return (TTypeRef)m_ReferenceValue;
        }

        #endregion // Internal

        #region ITweedleExpression

        // Explicit interface implementation
        TTypeRef ITweedleExpression.Type { get { return m_Type?.SelfRef; } }

        string ITweedleExpression.ToTweedle()
        {
            return m_Type == null ? "undefined" : m_Type.ToTweedle(ref this);
        }

        ExecutionStep ITweedleExpression.AsStep(ExecutionScope inScope)
        {
            return new ValueStep("", inScope, this);
        }

        #endregion // ITweedleExpression

        #region Misc

        internal double RawNumber()
        {
            return m_NumberValue;
        }

        internal T RawObject<T>() where T : class
        {
            return (T)m_ReferenceValue;
        }

        public override int GetHashCode()
        {
            return 17 * m_Type.GetHashCode(ref this) + m_Type.GetHashCode();
        }

        public int CompareTo(TValue other)
        {
            if (m_Type.LessThan(ref this, ref other))
                return -1;
            if (m_Type.Equals(ref this, ref other))
                return 0;
            return 1;
        }

        public override bool Equals(object obj)
        {
            if (obj is TValue)
                return Equals((TValue)obj);
            return false;
        }

        public bool Equals(TValue other)
        {
            if (Type == null)
                return other.Type == null;
            return m_Type.Equals(ref this, ref other);
        }

        public override string ToString()
        {
            return m_Type == null ? "undefined" : m_Type.ToTweedle(ref this);
        }

        static public bool operator ==(TValue inA, TValue inB)
        {
            return inA.Equals(inB);
        }

        static public bool operator !=(TValue inA, TValue inB)
        {
            return !inA.Equals(inB);
        }

        #endregion // Misc

        #region Defaults

        /// <summary>
        /// Undefined value.
        /// </summary>
        static public readonly TValue UNDEFINED = default(TValue);
        
        /// <summary>
        /// Null value.
        /// </summary>
        static public readonly TValue NULL = new TValue(TBuiltInTypes.NULL);

        /// <summary>
        /// True constant.
        /// </summary>
        static public readonly TValue TRUE = FromBoolean(true);

        /// <summary>
        /// False constant.
        /// </summary>
        static public readonly TValue FALSE = FromBoolean(false);

        #endregion // Defaults

        #region Factory

        /// <summary>
        /// Creates a new TValue from an integer.
        /// </summary>
        static public TValue FromInt(int inInt)
        {
            return new TValue(TBuiltInTypes.WHOLE_NUMBER, (double)inInt, null);
        }

        /// <summary>
        /// Creates a new TValue from a number.
        /// </summary>
        static public TValue FromNumber(double inNumber)
        {
            return new TValue(TBuiltInTypes.DECIMAL_NUMBER, inNumber, null);
        }

        /// <summary>
        /// Creates a new TValue from a bool.
        /// </summary>
        static public TValue FromBoolean(bool inbBoolean)
        {
            return new TValue(TBuiltInTypes.BOOLEAN, inbBoolean ? 1 : 0, null);
        }

        /// <summary>
        /// Creates a new TValue from a string.
        /// </summary>
        static public TValue FromString(string inString)
        {
            return new TValue(TBuiltInTypes.TEXT_STRING, double.NaN, inString);
        }

        /// <summary>
        /// Creates a new TValue from a type reference.
        /// </summary>
        static public TValue FromType(TTypeRef inType)
        {
            return new TValue(TBuiltInTypes.TYPE_REF, double.NaN, inType);
        }

        /// <summary>
        /// Creates a new TValue from a string.
        /// </summary>
        static public TValue FromObject(TType inType, Object inObject)
        {
            return new TValue(inType, double.NaN, inObject);
        }

        #endregion // Factory
    }

    internal class ValueStep : ExecutionStep
    {
        public ValueStep(string callStackEntry, ExecutionScope scope, TValue inTValue)
            : base(scope)
        {
            result = inTValue;
            this.callStack = scope.StackWith(callStackEntry);
        }
    }
}
