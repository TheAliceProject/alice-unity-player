using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Player.Modules;

namespace Alice.Player.Unity {
    public class MouseEventListenerProxy{
        
        public PAction listener;
        public OverlappingEventPolicy policy;

        private bool callActive = false;
        private float lastCheckedTime = 0f;
        private int queuedCalls = 0;

        ///////////////////////
        // TODO: Figure out real time vs simulated time?
        ///////////////////////

        public MouseEventListenerProxy(PAction listener, OverlappingEventPolicy policy){
            this.listener = listener;
            this.policy = policy;
            Debug.Log("Added mouse event");
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
