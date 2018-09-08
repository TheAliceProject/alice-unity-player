using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Tweedle.Modules
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