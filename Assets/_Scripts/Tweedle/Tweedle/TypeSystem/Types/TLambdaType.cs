using System;
using Alice.Tweedle.Interop;
using Alice.Tweedle.VM;
using Alice.Utils;

namespace Alice.Tweedle
{
    /// <summary>
    /// Array type.
    /// </summary>
    public sealed class TLambdaType : TType
    {
        public TLambdaType(TAssembly inAssembly, TLambdaSignature inSignature)
            : base(inAssembly, inSignature.ToString())
        {
            Signature = inSignature;
        }

        public readonly TLambdaSignature Signature;

        #region Link

        protected override void LinkImpl(TAssemblyLinkContext inContext)
        {
            base.LinkImpl(inContext);

            Signature.Resolve(inContext);
        }

        #endregion // Link

        #region Object Semantics

        public override TField Field(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            // TODO(Alex): Replace
            throw new System.NotImplementedException();
        }

        public override TMethod Method(ExecutionScope inScope, ref TValue inValue, string inName, MemberFlags inFlags = MemberFlags.None)
        {
            // TODO(Alex): Replace
            throw new System.NotImplementedException();
        }

        public override TMethod Constructor(ExecutionScope inScope, NamedArgument[] inArguments)
        {
            // TODO(Alex): Replace
            throw new System.NotImplementedException();
        }

        public override bool IsReferenceType()
        {
            return true;
        }

        #endregion // Object Semantics

        #region Comparison Semantics

        public override bool Equals(ref TValue inValA, ref TValue inValB)
        {
            return base.Equals(ref inValA, ref inValB)
                && inValA.Array() == inValB.Array();
        }

        public override bool LessThan(ref TValue inValA, ref TValue inValB)
        {
            return false;
        }

        #endregion // Comparison Semantics

        #region Lifecycle

        public override TValue Instantiate()
        {
            return TValue.NULL;
        }

        public TValue Instantiate(LambdaExpression inLambda, ExecutionScope inScope)
        {
            return TValue.FromObject(this, new TLambda(inLambda, inScope));
        }

        public TValue Instantiate(TLambda inLambda)
        {
            return TValue.FromObject(this, inLambda);
        }

        public override TValue DefaultValue()
        {
            return TValue.NULL;
        }

        #endregion // Lifecycle

        #region Tweedle Casting

        public override bool CanCast(TType inType)
        {
            if (base.CanCast(inType))
                return true;

            TLambdaType lambdaType = inType as TLambdaType;
            if (lambdaType != null)
            {
                if (lambdaType.Signature.ParametersMatch(Signature))
                {
                    // TODO: Re-enable this once lambda expressions have better-defined return types
                    // return Signature.ReturnType.Get().CanCast(lambdaType.Signature.ReturnType.Get());
                    return true;
                }
            }

            return false;
        }

        #endregion // Tweedle Casting

        #region Conversion Semantics

        public override string ConvertToString(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue.Lambda().ToString();
        }

        public override object ConvertToPObject(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue;
        }

        public override Type GetPObjectType()
        {
            return typeof(TValue);
        }

        #endregion // Conversion Semantics

        #region Misc

        public override int GetHashCode(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue.Lambda().GetHashCode();
        }

        public override string ToTweedle(ref TValue inValue)
        {
            AssertValueIsType(ref inValue);
            return inValue.Lambda().ToString();
        }

        #endregion // Misc
    }

    public sealed class TLambdaSignature : IEquatable<TLambdaSignature>
    {
        public readonly TTypeRef[] Parameters;
        public readonly TTypeRef ReturnType;
        
        public readonly string Name;

        public TLambdaSignature(TTypeRef[] inParameters, TTypeRef inReturnType)
        {
            Parameters = inParameters;
            ReturnType = inReturnType;

            using(PooledStringBuilder psb = PooledStringBuilder.Alloc())
            {
                psb.Builder.Append("<");
                if (Parameters.Length > 0)
                {
                    for (int i = 0; i < Parameters.Length; ++i)
                    {
                        if (i > 0)
                            psb.Builder.Append(',');
                        psb.Builder.Append(Parameters[i].Name);
                    }
                }
                else
                {
                    psb.Builder.Append("void");
                }

                psb.Builder.Append("->")
                    .Append(ReturnType.Name)
                    .Append(">");

                Name = psb.Builder.ToString();
            }
        }

        public void Resolve(TAssemblyLinkContext inContext)
        {
            ReturnType.Resolve(inContext);
            for (int i = 0; i < Parameters.Length; ++i)
                Parameters[i].Resolve(inContext);
        }

        public override bool Equals(object obj)
        {
            if (obj is TLambdaSignature)
                return Equals((TLambdaSignature)obj);
            return false;
        }

        public bool Equals(TLambdaSignature other)
        {
            if (other == null)
                return false;

            return Name == other.Name;
        }

        public bool ParametersMatch(TLambdaSignature other)
        {
            if (other == null || other.Parameters.Length != Parameters.Length)
                return false;

            for (int i = 0; i < Parameters.Length; ++i)
            {
                if (Parameters[i] != other.Parameters[i])
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}