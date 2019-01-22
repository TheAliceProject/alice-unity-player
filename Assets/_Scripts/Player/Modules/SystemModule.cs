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
            return TType.InstanceOf(instance, type);
        }

        [PInteropMethod]
        static public string getClassName(TValue instance)
        {
            return instance.Type.Name;
        }

        [PInteropMethod]
        static public TValue cast(TValue instance, TType type)
        {
            if (!instance.Type.CanCast(type)) {
                throw new TweedleRuntimeException(string.Format("Cannot cast instance of {0} to {1}.", instance.Type.Name, type.Name));
            }
            return instance.Type.Cast(ref instance, type);
        }
    }
}