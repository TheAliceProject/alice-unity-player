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
        private List<Transform> targetTransforms = new List<Transform>();
        private bool callActive = false;

        public PointOfViewChangeEventListenerProxy(PAction<TValue> listener, SGEntity[] targets)
        {
            this.listener = listener;
            for (int i = 0; i < targets.Length; i++){
                targetEntities.Add(targets[i]);
                targetTransforms.Add(targets[i].cachedTransform);
                targets[i].cachedTransform.hasChanged = false;
            }
        }

        public void CheckChanges()
        {
            for (int i = 0; i < targetEntities.Count; i++)
            {
                if(targetTransforms[i].hasChanged){
                    targetTransforms[i].hasChanged = false;
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
