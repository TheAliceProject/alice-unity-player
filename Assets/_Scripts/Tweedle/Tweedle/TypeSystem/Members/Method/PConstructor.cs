﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Alice.Tweedle.Interop;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Proxy/platform constructor.
    /// </summary>
    public class PConstructor : PMethodBase
    {
        private ConstructorInfo m_Constructor;

        public PConstructor(TAssembly inAssembly, ConstructorInfo inMethod)
            : base(inAssembly, inMethod, inMethod.DeclaringType, MemberFlags.Constructor)
        {
            m_Constructor = inMethod;
        }

        protected override object Invoke(object thisVal, object[] inCachedArgs)
        {
            return m_Constructor.Invoke(inCachedArgs);
        }
    }
}