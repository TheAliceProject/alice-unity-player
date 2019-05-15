using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using UnityEngine;
using Alice.Player.Unity;
using System.Collections;

namespace Alice.Player.Modules
{
    [PInteropType("Mouse")]
    static public class MouseModule
    {
        [PInteropMethod]
        public static void addMouseEventListener(PAction listener, int eventPolicy, TValue scene) {
            var entity = SceneGraph.Current.FindEntity<SGScene>(scene);
            entity.AddMouseClickListener(listener, (OverlappingEventPolicy)eventPolicy);
        }
    }
}