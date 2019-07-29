using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Player.Unity;
using System.Collections;
using BeauRoutine;

namespace Alice.Player.Modules
{
    [PInteropType("Dialog")]
    static public class DialogModule
    {
        [PInteropMethod]
        public static AsyncReturn<string> getStringFromUser(string message) {
            SceneCanvas canvas = SceneGraph.Current.Scene.GetCurrentCanvas();
            return canvas.UserInputControl.spawnStringInput(message);
        }

        [PInteropMethod]
        public static AsyncReturn<bool> getBooleanFromUser(string message) {
            SceneCanvas canvas = SceneGraph.Current.Scene.GetCurrentCanvas();
            return canvas.UserInputControl.spawnBooleanInput(message);
        }

        [PInteropMethod]
        public static AsyncReturn<double> getDoubleFromUser(string message) {
            SceneCanvas canvas = SceneGraph.Current.Scene.GetCurrentCanvas();
            return canvas.UserInputControl.spawnDoubleInput(message);
        }
    }
}