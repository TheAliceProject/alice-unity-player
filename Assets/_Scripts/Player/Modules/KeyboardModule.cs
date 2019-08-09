using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using UnityEngine;
using Alice.Player.Unity;
using System.Collections;

namespace Alice.Player.Modules
{
    [PInteropType("Keyboard")]
    static public class KeyboardModule
    {
        [PInteropMethod]
        public static void addKeyListener(PAction<bool, bool, int> listener, int overlappingEventPolicy, int heldKeyPolicy) {
            var sceneEntity = SceneGraph.Current.Scene;
            sceneEntity.AddKeyListener(listener, (OverlappingEventPolicy)overlappingEventPolicy, (HeldKeyPolicy)heldKeyPolicy);
        }

        
        [PInteropMethod]
        public static void addArrowKeyPressListener(PAction<int> listener, int overlappingEventPolicy, int heldKeyPolicy) {
            var sceneEntity = SceneGraph.Current.Scene;
            sceneEntity.AddArrowKeyListener(listener, (OverlappingEventPolicy)overlappingEventPolicy, (HeldKeyPolicy)heldKeyPolicy);
        }

         /*       
        [PInteropMethod]
        public static void addNumberKeyListener(PAction listener, int eventPolicy, TValue scene) {
            var sceneEntity = SceneGraph.Current.Scene;
        }

        [PInteropMethod]
        public static void addObjectMoverFor(TValue entity) {
            
        }
        */
    }
}