using System.Diagnostics;
using Alice.Tweedle.Interop;
using System.Collections;
using BeauRoutine;

namespace Alice.Player.Modules
{
    [PInteropType("Dialog")]
    static public class DialogModule
    {
        public static string returnString;

        [PInteropMethod]
        public static AsyncReturn<string> getStringFromUser(string message) {
            UnityEngine.Debug.Log("DialogModule: " + message);
            AsyncReturn<string> returnString = new AsyncReturn<string>();
            Routine.Start(GetStringThing(returnString));
            return returnString;
        }

        private static IEnumerator GetStringThing(AsyncReturn<string> returnString)
        {
            yield return 3f;
            returnString.Return("m'callback");
        }
    }
}