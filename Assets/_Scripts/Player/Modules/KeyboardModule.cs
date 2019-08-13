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
        public static void addKeyListener(PAction<int> listener, int overlappingEventPolicy, int heldKeyPolicy) {
            var sceneEntity = SceneGraph.Current.Scene;
            sceneEntity.AddKeyListener(listener, (OverlappingEventPolicy)overlappingEventPolicy, (HeldKeyPolicy)heldKeyPolicy);
        }

        
        [PInteropMethod]
        public static void addArrowKeyPressListener(PAction<int> listener, int overlappingEventPolicy, int heldKeyPolicy) {
            var sceneEntity = SceneGraph.Current.Scene;
            sceneEntity.AddArrowKeyListener(listener, (OverlappingEventPolicy)overlappingEventPolicy, (HeldKeyPolicy)heldKeyPolicy);
        }

        
        [PInteropMethod]
        public static void addNumberKeyPressListener(PAction<int> listener, int overlappingEventPolicy, int heldKeyPolicy){
            var sceneEntity = SceneGraph.Current.Scene;
            sceneEntity.AddNumberKeyListener(listener, (OverlappingEventPolicy)overlappingEventPolicy, (HeldKeyPolicy)heldKeyPolicy);
        }

        [PInteropMethod]
        public static void moveWithArrows(TValue thing, float speed) {
            // Speed is ignored for now.
            var sceneEntity = SceneGraph.Current.Scene;
            var entityXform = SceneGraph.Current.FindEntity(thing);
            Transform objectTransform = entityXform.cachedTransform;
            sceneEntity.AddKeyMover(objectTransform);

        }
    }
}