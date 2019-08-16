using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Player.Modules;
using Alice.Tweedle;
using Alice.Player.Primitives;
using BeauRoutine;

namespace Alice.Player.Unity {
    public class KeyEventListenerProxy{
        
        public enum KeyPressType{
            Normal,
            ArrowKey,
            NumPadKey
        }

        public PAction<int> keyListener;
        public OverlappingEventPolicy overlappingEventPolicy;
        public HeldKeyPolicy heldKeyPolicy;
        public KeyPressType keyType;

        private bool callActive = false;
        private float lastCheckedTime = 0f;
        private int queuedCalls = 0;
        private Routine m_routine;
        private List<Key> arrowKeys = new List<Key> {Key.LEFT, Key.RIGHT, Key.UP, Key.DOWN, Key.W, Key.A, Key.S, Key.D};
        private List<Key> numpadKeys = new List<Key> { Key.NUMPAD0, Key.NUMPAD1, Key.NUMPAD2, Key.NUMPAD3, Key.NUMPAD4, Key.NUMPAD5, Key.NUMPAD6, Key.NUMPAD7, Key.NUMPAD8, Key.NUMPAD9,
                                                        Key.DIGIT_0, Key.DIGIT_1, Key.DIGIT_2, Key.DIGIT_3, Key.DIGIT_4, Key.DIGIT_5, Key.DIGIT_6, Key.DIGIT_7, Key.DIGIT_8, Key.DIGIT_9,  };


        public KeyEventListenerProxy(PAction<int> listener, OverlappingEventPolicy overlappingEventPolicy, HeldKeyPolicy heldKeyPolicy, KeyPressType keyType){
            this.keyListener = listener;
            this.overlappingEventPolicy = overlappingEventPolicy;
            this.heldKeyPolicy = heldKeyPolicy;
            this.keyType = keyType;
        }

        public void NotifyEvent(int theKey, bool keyDown)
        {
            if(keyType == KeyPressType.ArrowKey){
                if(!arrowKeys.Contains((Key)theKey))
                    return;
            }
            else if(keyType == KeyPressType.NumPadKey){
                if(!numpadKeys.Contains((Key)theKey))
                    return;
            }

            // Manage key downs and key ups in regards to the held key policy
            if(keyDown){
                if(heldKeyPolicy == HeldKeyPolicy.FireOnceOnPress){
                    CallEvent(theKey);
                }
                else if(heldKeyPolicy == HeldKeyPolicy.FireMultiple){
                    if(!m_routine){
                        m_routine = Routine.Start(FireMultipleRoutine(theKey));
                    }
                }
            }
            else{ // key up
                if(heldKeyPolicy == HeldKeyPolicy.FireOnceOnRelease){
                    CallEvent(theKey);
                }
                else if(m_routine){
                    m_routine.Stop();
                }
            }
        }


        public void CallEvent(int theKey)
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

            AsyncReturn callReturn;
            callReturn = keyListener.Call(theKey);
            callActive = true;
            callReturn.OnReturn(() => {
                returnedCall(theKey);
            });
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

        private IEnumerator FireMultipleRoutine(int theKey)
        {
            while(true)
            {
                CallEvent(theKey);
                yield return 0.033f;
            }
        }
    }
}
