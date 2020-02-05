using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Player.Modules;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public class MouseEventListenerProxy{
        
        public PAction<Primitives.Portion, Primitives.Portion> screenListener;
        public PAction<Primitives.Portion, Primitives.Portion, TValue> objectListener;
        public OverlappingEventPolicy policy;
        public GameObject[] targets;
        public bool onlyOnModels = false;

        private bool callActive = false;
        private int queuedCalls = 0;

        public MouseEventListenerProxy(PAction<Portion, Portion, TValue> listener, OverlappingEventPolicy policy, SGModel[] triggerTargets){
            this.objectListener = listener;
            this.screenListener = null;
            this.policy = policy;
            this.onlyOnModels = true;
            // Specify which objects we are looking for clicks on
            targets = new GameObject[triggerTargets.Length];
            for(int i = 0; i < triggerTargets.Length; i++){
                targets[i] = triggerTargets[i].gameObject;
            }
        }

        public MouseEventListenerProxy(PAction<Portion, Portion> listener, OverlappingEventPolicy policy)
        {
            this.screenListener = listener;
            this.objectListener = null;
            this.policy = policy;
        }

        public void CallEvent(Portion x, Portion y){
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
            callReturn = screenListener.Call(x, y);
            callActive = true;
            callReturn.OnReturn(() => {
                returnedCall(x, y, TValue.NULL);
            });
        }

        public void CallEvent(Portion x, Portion y, TValue model)
        {
            if (callActive)
            {
                if (policy == OverlappingEventPolicy.Ignore)
                {
                    return;
                }
                else if (policy == OverlappingEventPolicy.Enqueue)
                {
                    queuedCalls++;
                    return;
                }
            }
            AsyncReturn callReturn;
            callReturn = objectListener.Call(x, y, model);
            callActive = true;
            callReturn.OnReturn(() =>
            {
                returnedCall(x, y, model);
            });
        }

        // For queued events
        public void returnedCall(Portion x, Portion y, TValue model)
        {
            callActive = false;
            if(queuedCalls > 0)
            {
                queuedCalls--;
                if(model == TValue.NULL)
                    CallEvent(x, y);
                else
                    CallEvent(x, y, model);
            }
        }
    }
}
