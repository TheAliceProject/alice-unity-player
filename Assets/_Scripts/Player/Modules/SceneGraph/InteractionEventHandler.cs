using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;

namespace Alice.Player.Unity 
{
    public class InteractionEventHandler
    {
        private List<CollisionEventListenerProxy> m_CollisionListeners = new List<CollisionEventListenerProxy>();
        private List<ViewEventListenerProxy> m_ViewListeners = new List<ViewEventListenerProxy>();
        private List<ProximityEventListenerProxy> m_ProximityListeners = new List<ProximityEventListenerProxy>();
        private List<PointOfViewChangeEventListenerProxy> m_PovListeners = new List<PointOfViewChangeEventListenerProxy>();
        private List<OcclusionEventListenerProxy> m_OcclusionListeners = new List<OcclusionEventListenerProxy>();

        // Called from Update()
        public void HandleInteractionEvents()
        {
            // Check proximity listeners
            if(m_ProximityListeners.Count > 0){
                for (int i = 0; i < m_ProximityListeners.Count; i++){
                    m_ProximityListeners[i].CheckDistances();
                }
            }

            // Check POV change listeners
            if(m_PovListeners.Count > 0){
                for (int i = 0; i < m_PovListeners.Count; i++){
                    m_PovListeners[i].CheckChanges();
                }
            }

            for (int i = 0; i < m_OcclusionListeners.Count; i++)
            {
                m_OcclusionListeners[i].UpdateOcclusions();
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
            for(int i = 0; i < m_CollisionListeners.Count; i++)
            {
                m_CollisionListeners[i].NotifyEvent(object1,  object2, enter);
            }
        }

        public void NotifyObjectsOccluded(SGModel foregroundObject, SGModel backgroundObject)
        {
            for (int i = 0; i < m_OcclusionListeners.Count; i++)
            {
                m_OcclusionListeners[i].NotifyEvent(foregroundObject, backgroundObject);
            }
        }

        public void NotifyModelInView(SGModel model, bool enteredView)
        {
            for (int i = 0; i < m_ViewListeners.Count; i++)
            {
                m_ViewListeners[i].NotifyEvent(model, enteredView);
            }
        }
    }


}
