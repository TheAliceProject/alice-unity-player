using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Player.Modules;

namespace Alice.Player.Unity {
    public class TimeEvent{
        
        public PAction listener;
        public float frequency;
        public OverlappingEventPolicy policy;

        private bool callActive = false;
        private float lastFireTime = 0f;
        private float lastCheckedTime = 0f;
        private int queuedCalls = 0;

        ///////////////////////
        // TODO: Figure out real time vs simulated time?
        ///////////////////////

        public TimeEvent(PAction listener, float freq, OverlappingEventPolicy policy){
            this.listener = listener;
            this.frequency = freq;
            this.policy = policy;
            Debug.Log("Time event created. Policy: " + (int)policy);
        }

        public float getTimeSinceLastFire(){
            return Time.time - lastFireTime;
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
            Debug.Log(Time.time + " Event called");
            lastFireTime = Time.time;
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

        public void CheckEvent()
        {
            if(Time.time - lastCheckedTime > frequency)
            {
                lastCheckedTime = Time.time;
                CallEvent();
            }
        }
    }
}
