using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using UnityEngine;
using Alice.Player.Unity;
using System.Collections;

namespace Alice.Player.Modules
{
    [PInteropType("Interaction")]
    static public class InteractionModule
    {
        public enum InteractionType
        {
            OnCollisionStart,
            OnCollisionEnd,
            OnViewEnter,
            OnViewExit,
            OnProximityEnter,
            OnProximityExit
        }

        [PInteropMethod]
        public static void addCollisionStartListener(PAction<TValue, TValue> listener, TValue[] a, TValue[] b, int overlappingEventPolicy) {
            var sceneEntity = SceneGraph.Current.Scene;
            AddEntityColliders(a, b);
            sceneEntity.AddCollisionListener(listener, (OverlappingEventPolicy)overlappingEventPolicy, ConvertToEntityArray(a), ConvertToEntityArray(b), InteractionType.OnCollisionStart);
        }

        [PInteropMethod]
        public static void addCollisionEndListener(PAction<TValue, TValue> listener, TValue[] a, TValue[] b, int overlappingEventPolicy) {
            var sceneEntity = SceneGraph.Current.Scene;
            AddEntityColliders(a, b);
            sceneEntity.AddCollisionListener(listener, (OverlappingEventPolicy)overlappingEventPolicy, ConvertToEntityArray(a), ConvertToEntityArray(b), InteractionType.OnCollisionEnd);
        }

        [PInteropMethod]
        public static void addViewEnterListener(PAction<TValue> listener, TValue[] set, int overlappingEventPolicy)
        {
            var sceneEntity = SceneGraph.Current.Scene;
            sceneEntity.AddViewEnterBroadcasters(ConvertToModelArray(set));
            sceneEntity.AddViewListener(listener, (OverlappingEventPolicy)overlappingEventPolicy, ConvertToModelArray(set), InteractionType.OnViewEnter);
        }

        [PInteropMethod]
        public static void addViewExitListener(PAction<TValue> listener, TValue[] set, int overlappingEventPolicy)
        {
            var sceneEntity = SceneGraph.Current.Scene;
            sceneEntity.AddViewEnterBroadcasters(ConvertToModelArray(set));
            sceneEntity.AddViewListener(listener, (OverlappingEventPolicy)overlappingEventPolicy, ConvertToModelArray(set), InteractionType.OnViewExit);
        }

        [PInteropMethod]
        public static void addProximityEnterListener(PAction<TValue, TValue> listener, TValue[] a, TValue[] b, int overlappingEventPolicy, float distance)
        {
            var sceneEntity = SceneGraph.Current.Scene;
            sceneEntity.AddProximityListener(listener, (OverlappingEventPolicy)overlappingEventPolicy, ConvertToEntityArray(a), ConvertToEntityArray(b), distance, InteractionType.OnProximityEnter);
        }

        [PInteropMethod]
        public static void addProximityExitListener(PAction<TValue, TValue> listener, TValue[] a, TValue[] b, int overlappingEventPolicy, float distance)
        {
            var sceneEntity = SceneGraph.Current.Scene;
            sceneEntity.AddProximityListener(listener, (OverlappingEventPolicy)overlappingEventPolicy, ConvertToEntityArray(a), ConvertToEntityArray(b), distance, InteractionType.OnProximityExit);
        }

        private static void AddEntityColliders(TValue[] a, TValue[] b)
        {
            var sceneEntity = SceneGraph.Current.Scene;
            SGEntity[] aThings = new SGEntity[a.Length];
            SGEntity[] bThings = new SGEntity[b.Length];
            for(int i = 0; i < a.Length; i++){
                aThings[i] = SceneGraph.Current.FindEntity(a[i]);
            }
            for(int i = 0; i < b.Length; i++){
                bThings[i] = SceneGraph.Current.FindEntity(b[i]);
            }
            sceneEntity.AddColliders(aThings);
            sceneEntity.AddColliders(bThings);
        }

        private static SGEntity[] ConvertToEntityArray(TValue[] tvalues)
        {
            SGEntity[] entities = new SGEntity[tvalues.Length];
            for(int i = 0; i < tvalues.Length; i++){
                entities[i] = SceneGraph.Current.FindEntity(tvalues[i]);
            }
            return entities;
        }

        private static SGModel[] ConvertToModelArray(TValue[] tvalues)
        {
            SGModel[] models = new SGModel[tvalues.Length];
            for (int i = 0; i < tvalues.Length; i++){
                models[i] = SceneGraph.Current.FindEntity<SGModel>(tvalues[i]);
            }
            return models;
        }
    }
}