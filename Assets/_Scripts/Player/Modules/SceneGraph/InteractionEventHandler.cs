using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Alice.Player.Unity 
{
    public class InteractionEventHandler
    {
        private readonly List<CollisionEventListenerProxy> m_CollisionListeners = new List<CollisionEventListenerProxy>();
        private readonly List<ViewEventListenerProxy> m_ViewListeners = new List<ViewEventListenerProxy>();
        private readonly List<ProximityEventListenerProxy> m_ProximityListeners = new List<ProximityEventListenerProxy>();
        private readonly List<PointOfViewChangeEventListenerProxy> m_PovListeners = new List<PointOfViewChangeEventListenerProxy>();
        private readonly List<OcclusionEventListenerProxy> m_OcclusionListeners = new List<OcclusionEventListenerProxy>();
        
        
        private readonly List<OverlappingPair> m_Collisions = new List<OverlappingPair>();

        private bool m_Active;

        // Called from Update()
        public void HandleInteractionEvents()
        {
            // Check proximity listeners
            foreach (var listener in m_ProximityListeners)
            {
                listener.CheckDistances();
            }

            // Check POV change listeners
            foreach (var listener in m_PovListeners)
            {
                listener.CheckChanges();
            }

            foreach (var listener in m_OcclusionListeners)
            {
                listener.UpdateOcclusions();
            }
        }

        public void AddCollisionListener(CollisionEventListenerProxy listener)
        {
            m_CollisionListeners.Add(listener);
        }
        public void AddViewListener(ViewEventListenerProxy listener)
        {
            m_ViewListeners.Add(listener);
        }
        public void AddProximityListener(ProximityEventListenerProxy listener)
        {
            m_ProximityListeners.Add(listener);
        }
        public void AddPointOfViewChangeListener(PointOfViewChangeEventListenerProxy listener)
        {
            m_PovListeners.Add(listener);
        }
        public void AddOcclusionListener(OcclusionEventListenerProxy listener)
        {
            m_OcclusionListeners.Add(listener);
        }
        
        public void NotifyObjectsCollided(SGEntity object1, SGEntity object2, bool enter)
        {
            var parent1 = ParentOf(object1);
            var parent2 = ParentOf(object2);
            TrackOverlappingPair(object1, object2, enter);
            if (parent1 != null)
            {
                TrackOverlappingPair(parent1, object2, enter);
            }
            if (parent2 != null)
            {
                TrackOverlappingPair(object1, parent2, enter);
                if (parent1 != null)
                {
                    TrackOverlappingPair(parent1, parent2, enter);
                }
            }
        }

        private SGEntity ParentOf(SGEntity entity)
        {
            return entity is SGJoint joint ? joint.GetParentJointedModel() : null;
        }

        private void TrackOverlappingPair(SGEntity object1, SGEntity object2, bool enter)
        {
            var pair = m_Collisions.FirstOrDefault(p => p.ContainsBoth(object1, object2));
            if (pair != null)
            {
                if (pair.IsOrdered(object2, object1)) {
                    // Each collision is reported twice. Only register one ordering.
                    return;
                }
                pair.UpdateOverlaps(enter);
            }
            else
            {
                pair = new OverlappingPair(object1, object2, enter);
                m_Collisions.Add(pair);
            }

            if (!m_Active || (!pair.IsEnter() && !pair.IsExit()))
                return;

            foreach (var listener in m_CollisionListeners)
            {
                listener.NotifyEvent(object1, object2, enter);
            }
        }

        public void NotifyObjectsOccluded(SGModel foregroundObject, SGModel backgroundObject)
        {
            foreach (var listener in m_OcclusionListeners)
            {
                if (m_Active) {
                    listener.NotifyEvent(foregroundObject, backgroundObject);
                }
                else {
                    listener.UpdateOrAddOcclusion(foregroundObject, backgroundObject);
                }
            }
        }

        public void NotifyModelInView(SGModel model, bool enteredView)
        {
            foreach (var listener in m_ViewListeners)
            {
                if (m_Active) {
                    listener.NotifyEvent(model, enteredView);
                }
            }
        }

        public void StartNotifying() {
            m_Active = true;
        }

        public void StopNotifying() {
            m_Active = false;
        }
    }


}
