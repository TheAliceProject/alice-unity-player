﻿using System.Collections.Generic;
using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Player.Modules;
using Alice.Tweedle;

namespace Alice.Player.Unity {
    public class CollisionEventListenerProxy{

        private PAction<TValue, TValue> listener;
        private OverlappingEventPolicy policy;
        private InteractionSet interactingObjects;
        private bool onEnter = false;
        private bool callActive = false;
        private int queuedCalls = 0;

        public CollisionEventListenerProxy(PAction<TValue, TValue> listener, OverlappingEventPolicy policy, SGEntity[] setA, SGEntity[] setB, InteractionModule.InteractionType interactionType)
        {
            this.listener = listener;
            this.policy = policy;
            this.interactingObjects = new InteractionSet(new List<SGEntity>(setA), new List<SGEntity>(setB));

            if (interactionType == InteractionModule.InteractionType.OnCollisionStart)
                this.onEnter = true;
            else if (interactionType == InteractionModule.InteractionType.OnCollisionEnd)
                this.onEnter = false;
            else
                Debug.LogError("Invalid interaction type in CollisionEventListenerProxy");
        }

        public void NotifyEvent(SGEntity object1, SGEntity object2, bool entered)
        {
            if(onEnter != entered)
                return;

            if (interactingObjects.set1.Contains(object1) && interactingObjects.set2.Contains(object2)) {
                CallEvent(object1.owner, object2.owner);
                return;
            }

            if(interactingObjects.set1.Contains(object2) && interactingObjects.set2.Contains(object1))
                CallEvent(object2.owner, object1.owner);
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

    public class OverlappingPair
    {
        public SGEntity entity1;
        public SGEntity entity2;

        private int numOverlaps;
        public OverlappingPair(SGEntity ent1, SGEntity ent2, bool isEnter = false)
        {
            entity1 = ent1;
            entity2 = ent2;
            numOverlaps = isEnter ? 1 : 0;
        }

        public bool ContainsBoth(SGEntity ent1, SGEntity ent2)
        {
            return ent1 == entity1 && ent2 == entity2 ||
                   ent1 == entity2 && ent2 == entity1;
        }

        public bool IsOrdered(SGEntity ent1, SGEntity ent2) {
            return ent1 == entity1 && ent2 == entity2;
        }

        public float GetDistance()
        {
            Vector3 point1, point2;
            var center1 = entity1.transform.position;
            var center2 = entity2.transform.position;
            if (entity1 is SGModel sgModel1) {
                point1 = sgModel1.GetBoundsInWorldSpace(true).ClosestPoint(center2);
            } else {
                point1 = center1;
            }
            if (entity2 is SGModel sgModel) {
                point2 = sgModel.GetBoundsInWorldSpace(true).ClosestPoint(center1);
            } else {
                point2 = center2;
            }

            var isIntersecting = Vector3.Distance(point1, center2) < Vector3.Distance(point2, center2)
                                 && Vector3.Distance(point2, center1) < Vector3.Distance(point1, center1);
            // Uncomment to see distance visualized
            // Debug.DrawLine(point1, point2, isIntersecting ? Color.blue : Color.red, 0.5f);
            var distance = Vector3.Distance(point1, point2);
            return isIntersecting ? -distance : distance;
        }

        public bool IsEnter()
        {
            return numOverlaps == 1;
        }

        public bool IsExit()
        {
            return numOverlaps == 0;
        }

        public void UpdateOverlaps(bool entered)
        {
            numOverlaps += entered ? 1 : -1;
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
