using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Player.Modules;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public class PointOfViewChangeEventListenerProxy{
        
        public PAction<TValue> listener;

        private List<SGEntity> targetEntities = new List<SGEntity>();
        private List<UnityEngine.Vector3> targetPositions = new List<UnityEngine.Vector3>();
        private bool callActive = false;

        public PointOfViewChangeEventListenerProxy(PAction<TValue> listener, SGEntity[] targets)
        {
            this.listener = listener;
            for (int i = 0; i < targets.Length; i++){
                targetEntities.Add(targets[i]);
                targetPositions.Add(targets[i].cachedTransform.position);
            }
        }

        public void CheckChanges()
        {
            for (int i = 0; i < targetEntities.Count; i++)
            {
                if(targetPositions[i] != targetEntities[i].cachedTransform.position){
                    targetPositions[i] = targetEntities[i].cachedTransform.position;
                    CallEvent(targetEntities[i].owner);
                }
            }
        }

        public void CallEvent(TValue x){
            // No overlapping event policy for this, so we ignore.
            if(callActive){
                return;
            }
            AsyncReturn callReturn;
            callReturn = listener.Call(x);
            callActive = true;
            callReturn.OnReturn(() => {
                returnedCall();
            });
        }

        // For queued events
        public void returnedCall()
        {
            callActive = false;
        }
    }
}
