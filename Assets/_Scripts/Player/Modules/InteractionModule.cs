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
        public enum CollisionType
        {
            OnStart,
            OnEnd
        }

        [PInteropMethod]
        public static void addCollisionStartListener(PAction<TValue, TValue> listener, TValue[] a, TValue[] b, int overlappingEventPolicy) {
            var sceneEntity = SceneGraph.Current.Scene;
            AddEntityColliders(a, b);
            sceneEntity.AddCollisionListener(listener, (OverlappingEventPolicy)overlappingEventPolicy, ConvertToEntityArray(a), ConvertToEntityArray(b), CollisionType.OnStart);
        }

        [PInteropMethod]
        public static void addCollisionEndListener(PAction<TValue, TValue> listener, TValue[] a, TValue[] b, int overlappingEventPolicy) {
            var sceneEntity = SceneGraph.Current.Scene;
            AddEntityColliders(a, b);
            sceneEntity.AddCollisionListener(listener, (OverlappingEventPolicy)overlappingEventPolicy, ConvertToEntityArray(a), ConvertToEntityArray(b), CollisionType.OnEnd);
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
    }
}