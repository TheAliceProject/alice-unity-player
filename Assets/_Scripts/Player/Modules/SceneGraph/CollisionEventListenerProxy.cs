﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Player.Modules;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public class CollisionEventListenerProxy{
        
        public PAction<TValue, TValue> listener;
        public OverlappingEventPolicy policy;
        private InteractionSet interactingObjects;

        private bool onEnter = false;
        private bool callActive = false;
        private int queuedCalls = 0;

        public CollisionEventListenerProxy(PAction<TValue, TValue> listener, OverlappingEventPolicy policy, SGEntity[] setA, SGEntity[] setB, InteractionModule.CollisionType collisionType)
        {
            this.listener = listener;
            this.policy = policy;
            this.interactingObjects = new InteractionSet(new List<SGEntity>(setA), new List<SGEntity>(setB));
            this.onEnter = collisionType == InteractionModule.CollisionType.OnStart ? true : false;
        }

        public void NotifyEvent(SGEntity object1, SGEntity object2, bool entered)
        {
            if(onEnter != entered)
                return;

            if(interactingObjects.IsThereOverlap(object1, object2))
            {
                CallEvent(object1.owner, object2.owner);
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

    public class InteractionSet
    {
        public List<SGEntity> set1;
        public List<SGEntity> set2;

        public InteractionSet(List<SGEntity> setA, List<SGEntity> setB)
        {
            set1 = setA;
            set2 = setB;
        }

        public bool IsThereOverlap(SGEntity object1, SGEntity object2)
        {
            // We want to fire this event if one object is in one set, and one is in another set
            // Otherwise, either the object is not in either set, or they are both in the same set, or in no sets, so return false
            if(set1.Contains(object1) && set2.Contains(object2))
                return true;

            if(set1.Contains(object2) && set2.Contains(object1))
                return true;

            return false;
        }
    }
}
