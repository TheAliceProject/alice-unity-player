using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using UnityEngine;
using Alice.Player.Unity;
using System.Collections;

namespace Alice.Player.Modules
{
    public enum FontType {
        Default = 0,
        Sans_Serif = 1,
        Serif = 2,
        Monospaced = 3
    }

    [PInteropType("FontType")]
    static public class FontTypeModule
    {
        [PInteropField]
        public const int DEFAULT = (int)FontType.Default;
        [PInteropField]
        public const int SANS_SERIF = (int)FontType.Sans_Serif;
        [PInteropField]
        public const int SERIF = (int)FontType.Serif;
        [PInteropField]
        public const int MONOSPACED = (int)FontType.Monospaced;
    }
}