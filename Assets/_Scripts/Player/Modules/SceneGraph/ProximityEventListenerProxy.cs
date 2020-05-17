using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Player.Modules;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public class ProximityEventListenerProxy{

        private PAction<TValue, TValue> listener;
        private OverlappingEventPolicy policy;
        private Dictionary<OverlappingPair, float> pairDistances = new Dictionary<OverlappingPair, float>();
        private List<OverlappingPair> entityPairs = new List<OverlappingPair>();
        private float proximityDistance;
        private bool onEnter = false;
        private bool callActive = false;
        private int queuedCalls = 0;

        public ProximityEventListenerProxy(PAction<TValue, TValue> listener, OverlappingEventPolicy policy, SGEntity[] setA, SGEntity[] setB, float distance, InteractionModule.InteractionType interactionType)
        {
            this.listener = listener;
            this.policy = policy;
            this.proximityDistance = distance;
            if(interactionType == InteractionModule.InteractionType.OnProximityEnter)
                this.onEnter = true;
            else if(interactionType == InteractionModule.InteractionType.OnProximityExit)
                this.onEnter = false;
            else
                Debug.LogError("Invalid interaction type in ProximityEventListenerProxy");

            foreach(var entityA in setA) {
                foreach(var entityB in setB) {
                    if (entityA == entityB) continue;
                    
                    var pair = entityPairs.FirstOrDefault(p => p.ContainsBoth(entityA,entityB));
                    if (pair != null) continue;
                    
                    pair = new OverlappingPair(entityA,entityB);
                    entityPairs.Add(pair);
                    pairDistances[pair] = pair.GetDistance();
                }
            }
        }

        public void CheckDistances()
        { 
            foreach(OverlappingPair pair in entityPairs)
            {
                float currentDistance = pair.GetDistance();
                
                if (onEnter && (pairDistances[pair] > proximityDistance) && (currentDistance <= proximityDistance)){
                    CallEvent(pair.entity1.owner, pair.entity2.owner);
                }
                else if (!onEnter && (pairDistances[pair] <= proximityDistance) && (currentDistance > proximityDistance)){
                    CallEvent(pair.entity1.owner, pair.entity2.owner);
                }
                
                pairDistances[pair] = currentDistance;
            }
        }
        
        public void CallEvent(TValue x, TValue y){
            if(callActive){
                if(policy == OverlappingEventPolicy.Ignore){
                    return;
                }
                else if(policy == OverlappingEventPolicy.Enqueue){
                    queuedCalls++;
                    return;
                }
            }
            AsyncReturn callReturn;
            callReturn = listener.Call(x, y);
            callActive = true;
            callReturn.OnReturn(() => {
                returnedCall(x, y);
            });
        }

        // For queued events
        public void returnedCall(TValue x, TValue y)
        {
            callActive = false;
            if(queuedCalls > 0)
            {
                queuedCalls--;
                CallEvent(x, y);
            }
        }
    }
}
