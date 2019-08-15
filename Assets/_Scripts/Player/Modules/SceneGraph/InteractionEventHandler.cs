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

        public void AddListener(CollisionEventListenerProxy listener)
        {
            m_CollisionListeners.Add(listener);
        }

        public void NotifyObjectsCollided(SGEntity object1, SGEntity object2, bool enter)
        {
            for(int i = 0; i < m_CollisionListeners.Count; i++)
            {
                m_CollisionListeners[i].NotifyEvent(object1,  object2, enter);
            }
        }
    }


}
