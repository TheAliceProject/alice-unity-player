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
        
        public enum KeyPressType{
            Normal,
            ArrowKey,
            NumPadKey
        }

        public PAction<bool, bool, int> keyListener;
        public PAction<int> arrowKeyListener;
        public OverlappingEventPolicy overlappingEventPolicy;
        public HeldKeyPolicy heldKeyPolicy;
        public KeyPressType keyType;

        private bool callActive = false;
        private float lastCheckedTime = 0f;
        private int queuedCalls = 0;
        private Routine m_routine;
        private List<Key> arrowKeys = new List<Key> {Key.LEFT, Key.RIGHT, Key.UP, Key.DOWN, Key.W, Key.A, Key.S, Key.D};

        public KeyEventListnerProxy(PAction<bool, bool, int> listener, OverlappingEventPolicy overlappingEventPolicy, HeldKeyPolicy heldKeyPolicy){
            this.keyListener = listener;
            this.arrowKeyListener = null;
            this.overlappingEventPolicy = overlappingEventPolicy;
            this.heldKeyPolicy = heldKeyPolicy;
            this.keyType = KeyPressType.Normal;
        }

        public KeyEventListnerProxy(PAction<int> listener, OverlappingEventPolicy overlappingEventPolicy, HeldKeyPolicy heldKeyPolicy){
            this.keyListener = null;
            this.arrowKeyListener = listener;
            this.overlappingEventPolicy = overlappingEventPolicy;
            this.heldKeyPolicy = heldKeyPolicy;
            this.keyType = KeyPressType.ArrowKey;
        }

        public void NotifyEvent(bool isDigit, bool isLetter, int theKey, bool keyDown)
        {
            if(keyType == KeyPressType.ArrowKey){
                if(!arrowKeys.Contains((Key)theKey))
                    return;
            }

            // Manage key downs and key ups in regards to the held key policy
            if(keyDown){
                if(heldKeyPolicy == HeldKeyPolicy.FireOnceOnPress){
                    if(keyListener != null)
                        CallEvent(isDigit, isLetter, theKey);
                    else
                        CallEvent(theKey);
                }
                else if(heldKeyPolicy == HeldKeyPolicy.FireMultiple){
                    if(!m_routine){
                        m_routine = Routine.Start(FireMultipleRoutine(isDigit, isLetter, theKey));
                    }
                }
            }
            else{ // key up
                if(heldKeyPolicy == HeldKeyPolicy.FireOnceOnRelease){
                    if(keyListener != null)
                        CallEvent(isDigit, isLetter, theKey);
                    else
                        CallEvent(theKey);
                }
                else if(m_routine){
                    m_routine.Stop();
                }
            }
        }

        public void CallEvent(bool isDigit, bool isLetter, int theKey){
            CheckPolicies();
            AsyncReturn callReturn;
            callReturn = keyListener.Call(isDigit, isLetter, theKey);
            callActive = true;
            callReturn.OnReturn(() => {
                returnedCall(isDigit, isLetter, theKey);
            });
        }

        public void CallEvent(int theKey)
        {
            CheckPolicies();
            AsyncReturn callReturn;
            callReturn = arrowKeyListener.Call(theKey);
            callActive = true;
            callReturn.OnReturn(() => {
                returnedCall(theKey);
            });
        }

        private void CheckPolicies()
        {
            if(callActive){
                if(overlappingEventPolicy == OverlappingEventPolicy.Ignore){
                    return;
                }
                else if(overlappingEventPolicy == OverlappingEventPolicy.Enqueue){
                    queuedCalls++;
                    return;
                }
            }
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

        // For queued events
        public void returnedCall(int theKey)
        {
            callActive = false;
            if(queuedCalls > 0)
            {
                queuedCalls--;
                CallEvent(theKey);
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
