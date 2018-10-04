using System;
using System.Collections.Generic;
using System.Reflection;
using Alice.Tweedle.Interop;
using Alice.Tweedle.Parse;
using Alice.Tweedle.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Proxy/platform method.
    /// </summary>
    public class PMethod : PMethodBase
    {
        public PMethod(TAssembly inAssembly, MethodInfo inMethod)
            : base(inAssembly, inMethod, inMethod.ReturnType, MemberFlags.None)
        {
        }
    }
}