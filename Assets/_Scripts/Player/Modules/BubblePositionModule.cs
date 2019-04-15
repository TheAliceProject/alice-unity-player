using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using UnityEngine;
using Alice.Player.Unity;
using System.Collections;

namespace Alice.Player.Modules
{
    public enum BubblePosition {
        Automatic = 0,
        Left = 1,
        Center = 2,
        Right = 3
    }

    [PInteropType("BubblePosition")]
    static public class BubblePositionModule
    {
        [PInteropField]
        public const int AUTOMATIC = (int)BubblePosition.Automatic;
        [PInteropField]
        public const int LEFT = (int)BubblePosition.Left;
        [PInteropField]
        public const int CENTER = (int)BubblePosition.Center;
        [PInteropField]
        public const int RIGHT = (int)BubblePosition.Right;
    }
}