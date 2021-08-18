using System;
using System.Collections.Generic;
using System.Reflection;
using Alice.Tweedle.Interop;
using Alice.Tweedle.Parse;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Abstract proxy/platform method.
    /// </summary>
    public abstract class PMethodBase : TMethod
    {
        private MethodBase m_Method;
        private object[] m_CachedArgs;
        private string[] m_ParameterNames;
        private Type[] m_ParameterTypes;

        protected PMethodBase(TAssembly inAssembly, MethodBase inMethodBase, Type inReturnType, MemberFlags inFlags)
        {
            m_Method = inMethodBase;

            MemberFlags flags = inFlags | MemberFlags.Method | MemberFlags.PInterop;
            if (inMethodBase.IsStatic)
                flags |= MemberFlags.Static;
            else
                flags |= MemberFlags.Instance;

            TTypeRef tReturnType = TInterop.TTypeFor(inReturnType, inAssembly);
            SetupMember(inMethodBase.Name, tReturnType, flags);

            ParseArguments(inAssembly, tReturnType);
        }

        private void ParseArguments(TAssembly inAssembly, TTypeRef inReturnType)
        {
            var parameters = m_Method.GetParameters();
            m_CachedArgs = new object[parameters.Length];
            m_ParameterNames = new string[parameters.Length];
            m_ParameterTypes = new Type[parameters.Length];

            List<TParameter> requiredParams = new List<TParameter>();
            List<TParameter> optionalParams = new List<TParameter>();

            for (int i = 0; i < parameters.Length; ++i)
            {
                ParameterInfo param = parameters[i];
                m_ParameterNames[i] = param.Name;
                m_ParameterTypes[i] = param.ParameterType;

                TParameter tParam;
                if (param.IsOptional)
                {
                    tParam = TParameter.OptionalParameter(
                        TInterop.TTypeFor(param.ParameterType, inAssembly), param.Name, TInterop.ToTValueConst(param.DefaultValue)
                    );
                    optionalParams.Add(tParam);
                }
                else
                {
                    tParam = TParameter.RequiredParameter(
                        TInterop.TTypeFor(param.ParameterType, inAssembly), param.Name
                    );
                    requiredParams.Add(tParam);
                }
            }

            SetupParameters(requiredParams.ToArray(), optionalParams.ToArray());
        }

        protected override void AddBodyStep(InvocationScope inScope, StepSequence ioMain)
        {
            ioMain.AddStep(
                new DelayedOperationStep(this, inScope, () => {
                    object thisVal = PrepForInvoke(inScope);
                    object retVal = Invoke(thisVal, m_CachedArgs);
                    ReturnValue(inScope, retVal);
                })
            );
        }

        protected object PrepForInvoke(InvocationScope inScope)
        {
            for (int i = 0; i < m_ParameterNames.Length; ++i)
                m_CachedArgs[i] = TInterop.ToPObject(inScope.GetValue(m_ParameterNames[i]), m_ParameterTypes[i], inScope);

            object thisVal;
            if ((Flags & (MemberFlags.Static | MemberFlags.Constructor)) != 0)
                thisVal = null;
            else
                thisVal = TInterop.ToPObject(inScope.GetThis(), m_Method.DeclaringType, inScope);

            return thisVal;
        }

        protected object[] GetCachedArgs()
        {
            return m_CachedArgs;
        }

        protected void ReturnValue(InvocationScope inScope, object inValue)
        {
            inScope.Return(TInterop.ToTValue(inValue, inScope));
        }

        protected virtual object Invoke(object thisVal, object[] inCachedArgs)
        {
            return m_Method.Invoke(thisVal, inCachedArgs);
        }
    }
}