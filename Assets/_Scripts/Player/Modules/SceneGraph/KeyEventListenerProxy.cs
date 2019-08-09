using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Player.Modules;
using Alice.Tweedle;
using Alice.Player.Primitives;
using BeauRoutine;

namespace Alice.Player.Unity {
    public class KeyEventListnerProxy{
        
        public PAction<bool, bool, int> listener;
        public OverlappingEventPolicy overlappingEventPolicy;
        public HeldKeyPolicy heldKeyPolicy;

        private bool callActive = false;
        private float lastCheckedTime = 0f;
        private int queuedCalls = 0;
        private Routine m_routine;

        public KeyEventListnerProxy(PAction<bool, bool, int> listener, OverlappingEventPolicy overlappingEventPolicy, HeldKeyPolicy heldKeyPolicy){
            this.listener = listener;
            this.overlappingEventPolicy = overlappingEventPolicy;
            this.heldKeyPolicy = heldKeyPolicy;
        }

        public void NotifyEvent(bool isDigit, bool isLetter, int theKey, bool keyDown)
        {
            // Manage key downs and key ups in regards to the held key policy
            if(keyDown){
                if(heldKeyPolicy == HeldKeyPolicy.FireOnceOnPress){
                    CallEvent(isDigit, isLetter, theKey);
                }
                else if(heldKeyPolicy == HeldKeyPolicy.FireMultiple){
                    if(!m_routine){
                        m_routine = Routine.Start(FireMultipleRoutine(isDigit, isLetter, theKey));
                    }
                }
            }
            else{ // key up
                if(heldKeyPolicy == HeldKeyPolicy.FireOnceOnRelease){
                    CallEvent(isDigit, isLetter, theKey);
                }
                else if(m_routine){
                    m_routine.Stop();
                }
            }
        }

        public void CallEvent(bool isDigit, bool isLetter, int theKey){
            if(callActive){
                if(overlappingEventPolicy == OverlappingEventPolicy.Ignore){
                    return;
                }
                else if(overlappingEventPolicy == OverlappingEventPolicy.Enqueue){
                    queuedCalls++;
                    return;
                }
            }

            AsyncReturn callReturn;
            callReturn = listener.Call(isDigit, isLetter, theKey);
            callActive = true;
            callReturn.OnReturn(() => {
                returnedCall(isDigit, isLetter, theKey);
            });
        }


        // For queued events
        public void returnedCall(bool isDigit, bool isLetter, int theKey)
        {
            callActive = false;
            if(queuedCalls > 0)
            {
                queuedCalls--;
                CallEvent(isDigit, isLetter, theKey);
            }
        }

        private IEnumerator FireMultipleRoutine(bool isDigit, bool isLetter, int theKey)
        {
            while(true)
            {
                CallEvent(isDigit, isLetter, theKey);
                yield return 0.033f;
            }
        }
    }
}
