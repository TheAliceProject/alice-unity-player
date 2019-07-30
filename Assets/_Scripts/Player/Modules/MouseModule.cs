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
            var entity = SceneGraph.Current.FindEntity<SGScene>(scene);
            entity.AddMouseClickOnScreenListener(listener, (OverlappingEventPolicy)eventPolicy);
        }

        [PInteropMethod]
        public static void addClickOnObjectListener(PAction<Portion, Portion, TValue> listener, TValue[] setOfVisuals, int eventPolicy, TValue scene) {
            var sceneEntity = SceneGraph.Current.Scene;
            SGModel[] models = null;
            
            if(setOfVisuals != null && setOfVisuals.Length > 0){
                models = new SGModel[setOfVisuals.Length];
                for(int i = 0; i < setOfVisuals.Length; i++){
                    models[i] = SceneGraph.Current.FindEntity<SGModel>(setOfVisuals[i]);
                }
            }
            else
            {
                
                // Get all models, but ignore ground and room
                List<SGModel> allModels = SceneGraph.Current.FindAllEntities<SGModel>();
                List<SGModel> toRemove = new List<SGModel>();
                foreach(SGModel model in allModels){
                    if((model as SGGround != null) || (model as SGRoom != null))
                        toRemove.Add(model);
                }
                for(int i = 0; i < toRemove.Count; i++){
                    allModels.Remove(toRemove[i]);
                }
                models = allModels.ToArray();
            }
            sceneEntity.AddMouseClickOnObjectListener(listener, (OverlappingEventPolicy)eventPolicy, models);
        }

        [PInteropMethod]
        public static void addDragAdapter() {
            SGScene.defaultModelManipulationActive = true;
        }
    }
}