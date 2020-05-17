using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Player.Modules;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public class OcclusionEventListenerProxy
    {
        private PAction<TValue, TValue> listener;
        private OverlappingEventPolicy policy;
        private InteractionSet interactingObjects;
        private bool onEnter = false;
        private bool callActive = false;
        private List<OccludingPair> occludingPairs = new List<OccludingPair>();
        private int queuedCalls = 0;

        public OcclusionEventListenerProxy(PAction<TValue, TValue> listener, OverlappingEventPolicy policy, SGModel[] setA, SGModel[] setB, InteractionModule.InteractionType interactionType)
        {
            this.listener = listener;
            this.policy = policy;
            this.interactingObjects = new InteractionSet(new List<SGEntity>(setA), new List<SGEntity>(setB));

            if (interactionType == InteractionModule.InteractionType.OnOcclusionStart)
                this.onEnter = true;
            else if (interactionType == InteractionModule.InteractionType.OnOcclusionEnd)
                this.onEnter = false;
            else
                Debug.LogError("Invalid interaction type in OcclusionEventListenerProxy");
        }

        public void UpdateOcclusions()
        {
            for (int i = occludingPairs.Count - 1; i >= 0; i--){
                var pair = occludingPairs[i];
                if (pair.DecrementFrameCount()) continue;
                
                if(!onEnter && interactingObjects.IsThereOverlap(pair.model1, pair.model2)){
                    CallEvent(pair.model1.owner, pair.model2.owner);
                }
                occludingPairs.Remove(pair);
            }
            
        }

        public void NotifyEvent(SGModel foregroundModel, SGModel backgroundModel) {
            if (UpdateOrAddOcclusion(foregroundModel, backgroundModel) &&
                onEnter &&
                interactingObjects.IsThereOverlap(foregroundModel, backgroundModel)) {
                CallEvent(foregroundModel.owner, backgroundModel.owner);
            }
        }

        public bool UpdateOrAddOcclusion(SGModel foregroundModel, SGModel backgroundModel) {
            foreach (var pair in occludingPairs.Where(pair => pair.ContainsBoth(foregroundModel, backgroundModel))) {
                pair.ResetFrameCount();
                return false;
            }
            occludingPairs.Add(new OccludingPair(foregroundModel, backgroundModel));
            return true;
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

    public class OccludingPair
    {
        public readonly SGModel model1;
        public readonly SGModel model2;
        private int occludingFrame = 2;

        public OccludingPair(SGModel mod1, SGModel mod2)
        {
            model1 = mod1;
            model2 = mod2;
        }

        public bool ContainsBoth(SGModel mod1, SGModel mod2) {
            return (mod1 == model1 || mod1 == model2) &&
                   (mod2 == model1 || mod2 == model2);
        }

        public bool DecrementFrameCount() {
            return occludingFrame-- > 0;
        }

        public void ResetFrameCount() {
            occludingFrame = 2;
        }
    }

}
