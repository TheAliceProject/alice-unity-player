using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using UnityEngine;
using Alice.Player.Unity;
using System.Collections;

namespace Alice.Player.Modules
{
    public enum TextStyle {
        Plain = 0,
        Bold = 1,
        Italic = 2
    }

    [PInteropType("TextStyle")]
    static public class TextStyleModule
    {
        [PInteropField]
        public const int PLAIN = (int)TextStyle.Plain;
        [PInteropField]
        public const int BOLD = (int)TextStyle.Bold;
        [PInteropField]
        public const int ITALIC = (int)TextStyle.Italic;
    }
}