using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using UnityEngine;
using Alice.Player.Unity;
using System.Collections;

namespace Alice.Player.Modules
{
    public enum OverlappingEventPolicy {
        Enqueue = 0,
        Ignore = 1,
        Overlap = 2
    }

    [PInteropType("EventPolicy")]
    static public class EventPolicyModule
    {
        [PInteropField]
        public const int ENQUEUE = (int)OverlappingEventPolicy.Enqueue;
        [PInteropField]
        public const int IGNORE = (int)OverlappingEventPolicy.Ignore;
        [PInteropField]
        public const int OVERLAP = (int)OverlappingEventPolicy.Overlap;
    }
}