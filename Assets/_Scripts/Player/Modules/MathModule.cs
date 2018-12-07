using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using UnityEngine;
using Alice.Player.Unity;
using System.Collections;
using System;

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
        public const double PI = Math.PI;
        [PInteropField]
        public const double RAD2DEG = 180.0/Math.PI;
        [PInteropField]
        public const double DEG2RAD = Math.PI/180.0;
        [PInteropField]
        public const double RAD2REV = 1/(Math.PI*2);
        [PInteropField]
        public const double REV2RAD = Math.PI*2;


        [PInteropMethod]
        static public double sqrt(double number) {
            return Math.Sqrt(number);
        }

        [PInteropMethod]
        static public double abs(double number) {
            return Math.Abs(number);
        }

        [PInteropMethod]
        static public double min(double a, double b) {
            return Math.Min(a, b);
        }

        [PInteropMethod]
        static public double max(double a, double b) {
            return Math.Max(a, b);
        }

        [PInteropMethod]
        static public double clamp(double number, double min, double max) {
            return Math.Min(Math.Max(number, min), max);
        }
    }
}