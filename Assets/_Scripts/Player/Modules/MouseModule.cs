using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Unity;
using Alice.Player.Primitives;
using System.Collections.Generic;
using System.Linq;

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
            List<SGModel> models;
            
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
            models = GetAllModels();
                RemoveBackgrounds(models);
            //}
            sceneEntity.AddMouseClickOnObjectListener(listener, (OverlappingEventPolicy)eventPolicy, models.ToArray());
        }

        [PInteropMethod]
        public static void addDragAdapter(TValue[] setOfVisuals) {
            List<SGModel> models;
            if(setOfVisuals != null && setOfVisuals.Length > 0){
                models = setOfVisuals.Select(v => SceneGraph.Current.FindEntity<SGModel>(v)).ToList();
            } else {
                models = GetAllModels();
            } 
            var moveBackground = RemoveBackgrounds(models);
            var sceneEntity = SceneGraph.Current.Scene;
            sceneEntity.AddMouseColliders(models);
            sceneEntity.ActivateDefaultModelManipulation(models, moveBackground);
        }

        private static bool RemoveBackgrounds(ICollection<SGModel> models) {
            // Remove ground and room and return true if one (or more) were found
            var toRemove = models.Where(model => (model as SGGround != null) || (model as SGRoom != null)).ToList();
            if (toRemove.Count == 0) {
                return false;
            }
            foreach (var t in toRemove) {
                models.Remove(t);
            }
            return true;
        }

        private static List<SGModel> GetAllModels() {
            return SceneGraph.Current.FindAllEntities<SGModel>();
        }
    }
}