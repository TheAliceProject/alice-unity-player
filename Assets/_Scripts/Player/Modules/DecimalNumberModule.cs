using Alice.Tweedle.Interop;
using System;

namespace Alice.Player.Modules
{
    [PInteropType("DecimalNumber")]
    static public class DecimalNumberModule
    {
        [PInteropField]
        public const double POSITIVE_INFINITY = double.PositiveInfinity;
        [PInteropField]
        public const double NEGATIVE_INFINITY = double.NegativeInfinity;

        [PInteropMethod]
        static public double from(int wholeNumber) {
            return Convert.ToDouble(wholeNumber);
        }
        
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
        static public double round(double decimalNumber) {
            return Math.Round(decimalNumber);
        }

        [PInteropMethod]
        static public double floor(double decimalNumber) {
            return Math.Floor(decimalNumber);
        }

        [PInteropMethod]
        static public double ceiling(double decimalNumber) {
            return Math.Ceiling(decimalNumber);
        }

        [PInteropMethod]
        static public double pow(double b, double power)
        {
            return Math.Pow(b, power);
        }

        [PInteropMethod]
        static public double exp(double power)
        {
            return Math.Exp(power);
        }

        [PInteropMethod]
        static public double log(double x)
        {
            return Math.Log(x);
        }
    }
}