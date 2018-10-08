using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using UnityEngine;
using Alice.Player.Unity;
using System.Collections;

namespace Alice.Player.Modules
{
    [PInteropType("Math")]
    static public class MathModule
    {
        [PInteropField]
        public const double POSITIVE_INFINITY = double.PositiveInfinity;
        [PInteropField]
        public const double NEGATIVE_INFINITY = double.NegativeInfinity;
        [PInteropField]
        public const double PI = System.Math.PI;

        [PInteropMethod]
        static public double sqrt(double number) {
            return System.Math.Sqrt(number);
        }
        
    }
}