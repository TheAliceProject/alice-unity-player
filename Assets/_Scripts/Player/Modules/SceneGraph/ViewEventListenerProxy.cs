using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Player.Modules;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public class ViewEventListenerProxy{
        
        public PAction<TValue> listener;
        public OverlappingEventPolicy policy;

        private Dictionary<SGModel, int> inViewCounts = new Dictionary<SGModel, int>();
        private List<SGModel> validModels;
        private bool onEnter = false;
        private bool callActive = false;
        private int queuedCalls = 0;

        public ViewEventListenerProxy(PAction<TValue> listener, OverlappingEventPolicy policy, SGModel[] set, InteractionModule.InteractionType interactionType)
        {
            this.listener = listener;
            this.policy = policy;
            if(interactionType == InteractionModule.InteractionType.OnViewEnter)
                this.onEnter = true;
            else if(interactionType == InteractionModule.InteractionType.OnViewExit)
                this.onEnter = false;
            else
                Debug.LogError("Invalid interaction type in ViewEventListenerProxy");
            validModels = new List<SGModel>(set);
        }

        public void NotifyEvent(SGModel obj, bool entered)
        {
            if (!inViewCounts.ContainsKey(obj))
                inViewCounts[obj] = 0;

            inViewCounts[obj] += entered ? 1 : -1;

            if(onEnter != entered)
                return;

            Debug.Log(inViewCounts[obj]);
            if(validModels.Contains(obj))
            {
                if(entered && inViewCounts[obj] == 1 || !entered && inViewCounts[obj] == 0)
                    CallEvent(obj.owner);
            }
                
        }
        
        public void CallEvent(TValue x){
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
            callReturn = listener.Call(x);
            callActive = true;
            callReturn.OnReturn(() => {
                returnedCall(x);
            });
        }

        // For queued events
        public void returnedCall(TValue x)
        {
            callActive = false;
            if(queuedCalls > 0)
            {
                queuedCalls--;
                CallEvent(x);
            }
        }
    }
}
