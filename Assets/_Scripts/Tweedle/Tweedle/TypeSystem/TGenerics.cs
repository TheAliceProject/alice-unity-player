using System;
using System.Collections.Generic;
using Alice.Tweedle.File;
using Alice.Tweedle.Interop;
using Alice.Tweedle.VM;
using Alice.Player.Primitives;
using Alice.Player.Modules;
using Alice.Tweedle.Parse;

namespace Alice.Tweedle
{
    // Handles generic TType specializations
    static public class TGenerics
    {
        static public void Unload(TType inType)
        {
            // TODO(Alex): Clear the generics cache of all types with specializations on the given type
        }
    }
}
