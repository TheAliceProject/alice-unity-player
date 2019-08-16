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

        public void AddCollisionListener(CollisionEventListenerProxy listener)
        {
            m_CollisionListeners.Add(listener);
        }
        public void AddViewListener(ViewEventListenerProxy listener)
        {
            m_ViewListeners.Add(listener);
        }

        public void NotifyObjectsCollided(SGEntity object1, SGEntity object2, bool enter)
        {
            for(int i = 0; i < m_CollisionListeners.Count; i++)
            {
                m_CollisionListeners[i].NotifyEvent(object1,  object2, enter);
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
