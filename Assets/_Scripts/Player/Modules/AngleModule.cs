using Alice.Tweedle.Interop;
using System;

namespace Alice.Player.Modules
{
    [PInteropType("Angle")]
    static public class AngleModule
    {
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
        static public double sin(double radians) {
            return Math.Sin(radians);
        }

        [PInteropMethod]
        static public double cos(double radians)
        {
            return Math.Cos(radians);
        }

        [PInteropMethod]
        static public double tan(double radians)
        {
            return Math.Tan(radians);
        }

        [PInteropMethod]
        static public double asin(double sin)
        {
            return Math.Asin(sin);
        }

        [PInteropMethod]
        static public double acos(double cos)
        {
            return Math.Acos(cos);
        }

        [PInteropMethod]
        static public double atan(double tan)
        {
            return Math.Atan(tan);
        }

        [PInteropMethod]
        static public double atan2(double y, double x)
        {
            return Math.Atan2(y, x);
        }
    }
}