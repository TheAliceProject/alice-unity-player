using Alice.Tweedle.Interop;
using System;

namespace Alice.Player.Modules
{
    [PInteropType("WholeNumber")]
    static public class WholeNumberModule
    {
        [PInteropMethod]
        static public int abs(int number)
        {
            return Math.Abs(number);
        }

        [PInteropMethod]
        static public int min(int a, int b)
        {
            return Math.Min(a, b);
        }

        [PInteropMethod]
        static public int max(int a, int b)
        {
            return Math.Max(a, b);
        }

        [PInteropMethod]
        static public int round(double decimalNumber) {
            return Convert.ToInt32(Math.Round(decimalNumber));
        }

        [PInteropMethod]
        static public int floor(double decimalNumber) {
            return Convert.ToInt32(Math.Floor(decimalNumber));
        }

        [PInteropMethod]
        static public int ceiling(double decimalNumber) {
            return Convert.ToInt32(Math.Ceiling(decimalNumber));
        }
    }
}