using System.Collections;
using System.Collections.Generic;
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

        public void NotifyEvent(SGModel foregroundModel, SGModel backgroundModel)
        {
            OccludingPair occludingPair = null;
            for (int i = 0; i < occludingPairs.Count; i++)
            {
                if(occludingPairs[i].ContainsBoth(foregroundModel, backgroundModel))
                {
                    occludingPair = occludingPairs[i];
                    break;
                }
            }

            if(occludingPair == null)
            {
                occludingPair = new OccludingPair(foregroundModel, backgroundModel);
                occludingPairs.Add(occludingPair);
            }

            if(interactingObjects.IsThereOverlap(foregroundModel, backgroundModel))
            {
                CallEvent(foregroundModel.owner, backgroundModel.owner);
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

    public class OccludingPair
    {
        public SGModel model1;
        public SGModel model2;

        public OccludingPair(SGModel mod1, SGModel mod2)
        {
            model1 = mod1;
            model2 = mod2;
        }

        public bool ContainsBoth(SGModel mod1, SGModel mod2)
        {
            if (mod1 != model1 && mod1 != model2)
                return false;

            if (mod2 != model1 && mod2 != model2)
                return false;

            return true;
        }
    }

}
