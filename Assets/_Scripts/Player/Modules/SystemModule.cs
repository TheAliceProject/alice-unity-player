using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System;

namespace Alice.Player.Modules
{
    [PInteropType("System")]
    static public class SystemModule
    {
        [PInteropMethod]
        static public string getClassName(TValue instance)
        {
            return instance.Type.Name;
        }
    }
}