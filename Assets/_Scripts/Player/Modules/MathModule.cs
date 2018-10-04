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
        [PInteropMethod]
        static public double sqrt(double number) {
            return System.Math.Sqrt(number);
        }
        
    }
}