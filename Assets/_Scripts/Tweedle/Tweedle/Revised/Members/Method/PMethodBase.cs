using System;
using System.Collections.Generic;
using System.Reflection;
using Alice.Tweedle.Interop;
using Alice.Tweedle.Parse;
using Alice.VM;

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

        protected PMethodBase(MethodBase inMethodBase, Type inReturnType, MemberFlags inFlags)
        {
            m_Method = inMethodBase;

            MemberFlags flags = inFlags | MemberFlags.Method | MemberFlags.PInterop;
            if (inMethodBase.IsStatic)
                flags |= MemberFlags.Static;
            else
                flags |= MemberFlags.Instance;

            TTypeRef tReturnType = TInterop.TTypeFor(inReturnType);
            SetupMember(inMethodBase.Name, tReturnType, flags);

            ParseArguments(tReturnType);
        }

        private void ParseArguments(TTypeRef inReturnType)
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
                        TInterop.TTypeFor(param.ParameterType), param.Name, TInterop.ToTValue(param.DefaultValue)
                    );
                    optionalParams.Add(tParam);
                }
                else
                {
                    tParam = TParameter.RequiredParameter(
                        TInterop.TTypeFor(param.ParameterType), param.Name
                    );
                    requiredParams.Add(tParam);
                }
            }

            SetupSignature(inReturnType, requiredParams.ToArray(), optionalParams.ToArray());
        }

        protected override void AddBodyStep(InvocationScope inScope, StepSequence ioMain)
        {
            ioMain.AddStep(
                new DelayedOperationStep("call", inScope, () => {
                    Invoke(inScope);
                })
            );
        }

        private void Invoke(InvocationScope inScope)
        {
            for (int i = 0; i < m_ParameterNames.Length; ++i)
                m_CachedArgs[i] = TInterop.ToPObject(inScope.GetValue(m_ParameterNames[i]), m_ParameterTypes[i]);

            object thisVal;
            if ((Flags & (MemberFlags.Static | MemberFlags.Constructor)) != 0)
                thisVal = null;
            else
                thisVal = TInterop.ToPObject(inScope.GetThis());

            object retVal = Invoke(thisVal, m_CachedArgs);
            inScope.Return(TInterop.ToTValue(retVal, inScope.vm.Library));
        }

        protected virtual object Invoke(object thisVal, object[] inCachedArgs)
        {
            return m_Method.Invoke(thisVal, inCachedArgs);
        }
    }
}