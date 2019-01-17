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
        static public bool isInstanceOf(TValue instance, TType type)
        {
            return instance.Type.CanCast(type);
        }

        [PInteropMethod]
        static public string getClassName(TValue instance)
        {
            return instance.Type.Name;
        }

        [PInteropMethod]
        static public TValue cast(TValue instance, TType type)
        {
            return instance.Type.Cast(ref instance, type);
        }
    }
}