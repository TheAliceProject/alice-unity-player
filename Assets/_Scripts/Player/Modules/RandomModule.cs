using Alice.Tweedle.Interop;

namespace Alice.Player.Modules
{
    [PInteropType("Random")]
    static public class RandomModule
    {
        [PInteropMethod]
        static public int wholeNumberFrom0ToNExclusive(int n)
        {
            return UnityEngine.Random.Range(0, n);
        }

        [PInteropMethod]
        static public int wholeNumberFromAToBExclusive(int a, int b)
        {
            return UnityEngine.Random.Range(a, b);
        }

        [PInteropMethod]
        static public int wholeNumberFromAToBInclusive(int a, int b)
        {
            return UnityEngine.Random.Range(a, b+1);
        }

        [PInteropMethod]
        static public double decimalNumberInRange(double a, double b)
        {
            return UnityEngine.Random.Range((float) a, (float) b);
        }

        [PInteropMethod]
        static public bool boolean()
        {
            return UnityEngine.Random.Range(0,2) == 1;
        }
    }
}