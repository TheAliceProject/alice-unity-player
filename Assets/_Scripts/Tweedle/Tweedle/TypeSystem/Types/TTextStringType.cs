using System;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// String value type.
    /// </summary>
    public sealed class TTextStringType : TType
    {
        private TValue m_Default;
        private TField[] m_Fields;
        private TMethod[] m_Methods;

        public TTextStringType(TAssembly inAssembly)
            : base(inAssembly, "TextString")
        {
            m_Default = TValue.NULL;
            m_Fields = new TField[]
            {
                LengthField()
            };
            m_Methods = new TMethod[]
            {
                SubstringMethod(),
                ContentEqualsMethod(),
                EqualsIgnoreCaseMethod(),
                StartsWithMethod(),
                EndsWithMethod(),
                ContainsMethod()
            };
        }

        #region Custom Members

        private TField LengthField()
        {
            return new TPropertyField("length", TBuiltInTypes.WHOLE_NUMBER, MemberFlags.Readonly, (ExecutionScope inScope, ref TValue inValue) =>
            {
                return TValue.FromInt(inValue.RawObject<string>().Length);
            });
        }

        private TMethod SubstringMethod()
        {
            return new TCustomMethod("substring", MemberFlags.Instance, this,
            new TParameter[] {
                TParameter.RequiredParameter(TBuiltInTypes.WHOLE_NUMBER, "startIndex")
            }, new TParameter[] {
                TParameter.OptionalParameter(TBuiltInTypes.WHOLE_NUMBER, "length", TValue.FromInt(-1))
            }, (ExecutionScope inScope) =>
            {
                TValue _this = inScope.GetThis();
                int startIndex = inScope.GetValue("startIndex").ToInt();
                int length = inScope.GetValue("length").ToInt();
                string str = _this.ToTextString();
                if (length < 0)
                    str = str.Substring(startIndex);
                else
                    str = str.Substring(startIndex, length);
                return TValue.FromString(str);
            });
        }

        private TMethod ContentEqualsMethod() {
            return ComparisonMethod("contentEquals", (thisStr, arg) => thisStr.Equals(arg));
        }

        private TMethod EqualsIgnoreCaseMethod() {
            return ComparisonMethod("equalsIgnoreCase", (thisStr, arg) => thisStr.ToLower().Equals(arg.ToLower()));
        }

        private TMethod StartsWithMethod() {
            return ComparisonMethod("startsWith", (thisStr, arg) => thisStr.StartsWith(arg, StringComparison.Ordinal));
        }

        private TMethod EndsWithMethod() {
            return ComparisonMethod("endsWith", (thisStr, arg) => thisStr.EndsWith(arg, StringComparison.Ordinal));
        }

        private TMethod ContainsMethod() {
            return ComparisonMethod("contains", (thisStr, arg) => thisStr.Contains(arg));
        }

        private TMethod ComparisonMethod(string methodName, Func<string, string, bool> methodBody) {
            return new TCustomMethod(methodName, MemberFlags.Instance, TBuiltInTypes.BOOLEAN,
            // Used during the construction of TBuiltInTypes.TEXT_STRING, so "this" refers to the TEXT_STRING type as it is being built
            new TParameter[] { TParameter.RequiredParameter(this, "text") },
            new TParameter[] {},
            (ExecutionScope inScope) => {
                TValue _this = inScope.GetThis();
                string text = inScope.GetValue("text").ToTextString();
                string thisString = _this.ToTextString();
                return TValue.FromBoolean(methodBody(thisString, text));
            });
        }

        #endregion // Custom Members

        #region Link

        protected override void LinkImpl(TAssemblyLinkContext inContext)
        {
            base.LinkImpl(inContext);

            LinkMembers(m_Fields, inContext, this);
            LinkMembers(m_Methods, inContext, this);
        }

        #endregion // Link

        #region Object Semantics

        public override TField Field(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            return FindMember(m_Fields, inName, inFlags);
        }

        public override TMethod Method(ExecutionScope inScope, ref TValue inValue, string inName, string[] inArgNames, MemberFlags inFlags = MemberFlags.None)
        {
            return FindMember(m_Methods, inName, inFlags);
        }

        public override TMethod Constructor(ExecutionScope inScope, NamedArgument[] inArguments)
        {
            throw new TweedleConstructorUnsupportedException(this);
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

        #endregion // Object Semantics

        #region Comparison Semantics

        public override bool Equals(ref TValue inValA, ref TValue inValB)
        {
            return base.Equals(ref inValA, ref inValB)
                && inValA.RawObject<string>() == inValB.RawObject<string>();
        }

        public override bool LessThan(ref TValue inValA, ref TValue inValB)
        {
            return inValA.Type == inValB.Type
                && inValA.RawObject<string>().CompareTo(inValB.RawObject<string>()) < 0;
        }

        #endregion // Comparison Semantics

        #region Lifecycle

        public override TValue Instantiate()
        {
            return m_Default;
        }

        public TValue Instantiate(string inValue)
        {
            return TValue.FromString(inValue);
        }

        public override TValue DefaultValue()
        {
            return m_Default;
        }

        #endregion // Lifecycle

        #region Tweedle Casting

        #endregion // Tweedle Casting

        #region Conversion Semantics

        public override string ConvertToString(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue.RawObject<string>();
        }

        public override object ConvertToPObject(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue.RawObject<string>();
        }

        public override Type GetPObjectType()
        {
            return typeof(string);
        }

        #endregion // Conversion Semantics

        #region Misc

        public override int GetHashCode(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue.RawObject<string>().GetHashCode();
        }

        public override string ToTweedle(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return "\"" + inValue.RawObject<string>() + "\"";
        }

        #endregion // Misc
    }
}