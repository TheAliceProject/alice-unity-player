using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Player.Unity;
using System.Collections;
using UnityEngine.XR;
using UnityEngine;

namespace Alice.Player.Modules
{
    [PInteropType("Dialog")]
    static public class DialogModule
    {
        [PInteropMethod]
        public static AsyncReturn<string> getStringFromUser(string message) {
            SceneCanvas canvas = GetSceneCanvas();
            return canvas.UserInputControl.spawnStringInput(message);
        }

        [PInteropMethod]
        public static AsyncReturn<bool> getBooleanFromUser(string message) {
            SceneCanvas canvas = GetSceneCanvas();
            return canvas.UserInputControl.spawnBooleanInput(message);
        }

        [PInteropMethod]
        public static AsyncReturn<double> getDoubleFromUser(string message) {
            SceneCanvas canvas = GetSceneCanvas();
            return canvas.UserInputControl.spawnDoubleInput(message);
        }

        [PInteropMethod]
        public static AsyncReturn<int> getIntegerFromUser(string message) {
            SceneCanvas canvas = GetSceneCanvas();
            return canvas.UserInputControl.spawnIntegerInput(message);
        }

        [PInteropMethod]
        public static AsyncReturn<bool> spawnErrorDialog(string message)
        {
            SceneCanvas canvas = GetSceneCanvas();
            return canvas.UserInputControl.spawnErrorDialog(message);
        }

        private static SceneCanvas GetSceneCanvas()
        {
            SceneCanvas canvas;
            if (XRSettings.enabled)
            {
                canvas = SceneGraph.Current.Scene.CreateNewWorldCanvas();
                VRControl.Rig().EnablePointersForUI(true);
            }
            else
            {
                canvas = SceneGraph.Current.Scene.GetCurrentCanvas();
            }
            return canvas;
        }
    }
}