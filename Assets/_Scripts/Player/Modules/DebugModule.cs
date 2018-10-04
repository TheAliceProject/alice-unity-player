using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System;

namespace Alice.Player.Modules
{
    [PInteropType("Debug")]
    static public class DebugModule
    {
        static private Stopwatch s_Stopwatch = new Stopwatch();
        static private string s_TimingLabel = "";

        [PInteropMethod]
        static public void log(string @string)
        {
            UnityEngine.Debug.Log(@string);
        }

        [PInteropMethod]
        static public void dump(TValue @object)
        {
            UnityEngine.Debug.Log(@object.ToString());
        }

        [PInteropMethod]
        static public int[] generateArray(int start, int end)
        {
            int length = Math.Abs(end - start) + 1;
            int dir = end > start ? 1 : -1;
            int[] array = new int[length];
            for (int i = 0; i < length; ++i)
                array[i] = start + i * dir;
            return array;
        }

        [PInteropMethod]
        static public int getRange(int[] inArray)
        {
            int min = int.MaxValue;
            int max = int.MinValue;

            for (int i = 0; i < inArray.Length; ++i)
            {
                int val = inArray[i];
                if (val < min)
                    min = val;
                if (val > max)
                    max = val;
            }

            return max - min;
        }

        [PInteropMethod]
        static public int getRangeFromTweedle(TValue array)
        {
            int min = int.MaxValue;
            int max = int.MinValue;

            TArray arr = array.Array();

            for (int i = 0; i < arr.Length; ++i)
            {
                int val = arr[i].ToInt();
                if (val < min)
                    min = val;
                if (val > max)
                    max = val;
            }

            return max - min;
        }

        [PInteropMethod]
        static public void beginTiming(string label = "Timing")
        {
            s_TimingLabel = label;
            s_Stopwatch.Start();
        }

        [PInteropMethod]
        static public double endTiming()
        {
            s_Stopwatch.Stop();
            double ms = s_Stopwatch.Elapsed.TotalMilliseconds;
            UnityEngine.Debug.Log(s_TimingLabel + ": " + ms + "ms");
            return ms;
        }

        [PInteropField]
        static public string timingLabel
        {
            get { return s_TimingLabel; }
            set { s_TimingLabel = value; }
        }
    }
}