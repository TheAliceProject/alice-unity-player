using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using UnityEngine;
using Alice.Player.Unity;
using System.Collections;

namespace Alice.Player.Modules
{
    public enum HeldKeyPolicy {
        FireMultiple = 0,
        FireOnceOnPress = 1,
        FireOnceOnRelease = 2
    }

    [PInteropType("HeldKeyPolicy")]
    static public class HeldKeyPolicyModule
    {
        [PInteropField]
        public const int FIRE_MULTIPLE = (int)HeldKeyPolicy.FireMultiple;
        [PInteropField]
        public const int FIRE_ONCE_ON_PRESS = (int)HeldKeyPolicy.FireOnceOnPress;
        [PInteropField]
        public const int FIRE_ONCE_ON_RELEASE = (int)HeldKeyPolicy.FireOnceOnRelease;
    }
}