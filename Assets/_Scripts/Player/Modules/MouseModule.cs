using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using UnityEngine;
using Alice.Player.Unity;
using Alice.Player.Primitives;
using System.Collections;
using System.Collections.Generic;

namespace Alice.Player.Modules
{
    [PInteropType("Mouse")]
    static public class MouseModule
    {
        [PInteropMethod]
        public static void addClickOnScreenListener(PAction<Portion, Portion> listener, int eventPolicy, TValue scene) {
            var sceneEntity = SceneGraph.Current.Scene;
            sceneEntity.AddMouseClickOnScreenListener(listener, (OverlappingEventPolicy)eventPolicy);
        }

        [PInteropMethod]
        public static void addClickOnObjectListener(PAction<Portion, Portion, TValue> listener, /*TValue[] setOfVisuals, */ int eventPolicy, TValue scene) {
            var sceneEntity = SceneGraph.Current.Scene;
            SGModel[] models = null;
            
            //
            // Uncomment this if we want to only put colliders on certain models. Will need to be passed in an array of SThings that we should attach colliders to
            //
            /*
            if(setOfVisuals != null && setOfVisuals.Length > 0){
                models = new SGModel[setOfVisuals.Length];
                for(int i = 0; i < setOfVisuals.Length; i++){
                    models[i] = SceneGraph.Current.FindEntity<SGModel>(setOfVisuals[i]);
                }
            }
            else
            {
                */

                models = GetAllModelsBesidesGround().ToArray();
            //}
            sceneEntity.AddMouseClickOnObjectListener(listener, (OverlappingEventPolicy)eventPolicy, models);
        }

        [PInteropMethod]
        public static void addDragAdapter() {
            var sceneEntity = SceneGraph.Current.Scene;
            sceneEntity.AddMouseColliders(GetAllModelsBesidesGround().ToArray());
            SGScene.defaultModelManipulationActive = true;
        }

        private static List<SGModel> GetAllModelsBesidesGround()
        {
            // Get all models, but ignore ground and room
            List<SGModel> allModels = SceneGraph.Current.FindAllEntities<SGModel>();
            List<SGModel> toRemove = new List<SGModel>();
            foreach (SGModel model in allModels){
                if ((model as SGGround != null) || (model as SGRoom != null))
                    toRemove.Add(model);
            }
            for (int i = 0; i < toRemove.Count; i++){
                allModels.Remove(toRemove[i]);
            }
            return allModels;
        }
    }
}