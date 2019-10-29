using System.Collections.Generic;
using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Player.Modules;
using BeauRoutine;
using UnityEngine.XR;
using static Alice.Player.Unity.KeyboardEventHandler;

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
        private Queue<Key> queuedKeys = new Queue<Key>();
        private Routine m_routine;


        public KeyEventListenerProxy(PAction<int> listener, OverlappingEventPolicy overlappingEventPolicy, HeldKeyPolicy heldKeyPolicy, KeyPressType keyType){
            this.keyListener = listener;
            this.overlappingEventPolicy = overlappingEventPolicy;
            this.heldKeyPolicy = heldKeyPolicy;
            this.keyType = keyType;
        }

        public void NotifyEvent(Key theKey, KeyAction keyAction)
        {
            if (keyType == KeyPressType.ArrowKey && !KeyMap.ArrowKeys.Contains(theKey)) {
                return;
            }
            if (keyType == KeyPressType.NumPadKey && !KeyMap.NumpadKeys.Contains(theKey)) {
                return;
            }
            if(XRSettings.enabled)
            {
                if(!KeyMap.KeyboardToVRLookup.ContainsKey(theKey)){
                    return;
                }
                theKey = KeyMap.KeyboardToVRLookup[theKey];
            }

            if ((keyAction == KeyAction.Press && heldKeyPolicy != HeldKeyPolicy.FireOnceOnRelease) ||
                (keyAction == KeyAction.Repeat && heldKeyPolicy == HeldKeyPolicy.FireMultiple)||
                (keyAction == KeyAction.Release && heldKeyPolicy == HeldKeyPolicy.FireOnceOnRelease)) {
                CallEvent(theKey);
            }
        }


        private void CallEvent(Key theKey) {
            if (callActive && overlappingEventPolicy != OverlappingEventPolicy.Overlap){
                if(overlappingEventPolicy == OverlappingEventPolicy.Enqueue){
                    queuedKeys.Enqueue(theKey);
                }
                return;
            }

            callActive = true;
            AsyncReturn callReturn = keyListener.Call((int) theKey);
            callReturn.OnReturn(returnedCall);
        }

        private void returnedCall()
        {
            callActive = false;
            if(queuedKeys.Count > 0)
            {
                CallEvent(queuedKeys.Dequeue());
            }
        }
    }
}
