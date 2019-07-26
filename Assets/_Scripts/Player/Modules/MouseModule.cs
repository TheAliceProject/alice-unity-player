using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using UnityEngine;
using Alice.Player.Unity;
using System.Collections;
using System.Collections.Generic;

namespace Alice.Player.Modules
{
    [PInteropType("Mouse")]
    static public class MouseModule
    {
        [PInteropMethod]
        public static void addMouseEventListener(PAction listener, int eventPolicy, TValue scene) {
            var entity = SceneGraph.Current.FindEntity<SGScene>(scene);
            entity.AddMouseClickOnScreenListener(listener, (OverlappingEventPolicy)eventPolicy);
        }

        [PInteropMethod]
        public static void addMouseClickOnObjectListener(PAction listener, TValue[] targets, int eventPolicy, TValue scene) {
            var sceneEntity = SceneGraph.Current.FindEntity<SGScene>(scene);
            SGModel[] models = null;
            if(targets != null && targets.Length > 0){
                models = new SGModel[targets.Length];
                for(int i = 0; i < targets.Length; i++){
                    models[i] = SceneGraph.Current.FindEntity<SGModel>(targets[i]);
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