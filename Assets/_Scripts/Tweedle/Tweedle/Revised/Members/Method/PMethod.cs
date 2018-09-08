using System;
using System.Collections.Generic;
using System.Reflection;
using Alice.Tweedle.Interop;
using Alice.Tweedle.Parse;
using Alice.VM;

namespace Alice.Tweedle
{
    /// <summary>
    /// Proxy/platform method.
    /// </summary>
    public class PMethod : PMethodBase
    {
        public PMethod(MethodInfo inMethod)
            : base(inMethod, inMethod.ReturnType, MemberFlags.None)
        {
        }
    }
}