using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Player.Modules;

namespace Alice.Player.Unity {
    public class MouseEventListenerProxy{
        
        public PAction listener;
        public OverlappingEventPolicy policy;
        public GameObject[] targets;
        public bool onlyOnModels = false;

        private bool callActive = false;
        private float lastCheckedTime = 0f;
        private int queuedCalls = 0;

        public MouseEventListenerProxy(PAction listener, OverlappingEventPolicy policy, bool isModelClick, SGModel[] triggerTargets){
            this.listener = listener;
            this.policy = policy;
            this.onlyOnModels = isModelClick;
            // Specify which objects we are looking for clicks on
            if(isModelClick)
            {
                targets = new GameObject[triggerTargets.Length];
                for(int i = 0; i < triggerTargets.Length; i++){
                    targets[i] = triggerTargets[i].gameObject;
                }
            }
        }

        public void CallEvent(){
            if(callActive){
                if(policy == OverlappingEventPolicy.Ignore){
                    return;
                }
                else if(policy == OverlappingEventPolicy.Enqueue)
                {
                    queuedCalls++;
                    return;
                }
            }
            AsyncReturn callReturn;
            callReturn = listener.Call();
            callActive = true;
            callReturn.OnReturn(() => {
                returnedCall();
            });
        }

        public void returnedCall()
        {
            callActive = false;
            if(queuedCalls > 0)
            {
                queuedCalls--;
                CallEvent();
            }
        }
    }
}
