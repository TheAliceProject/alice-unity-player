using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Player.Unity;

namespace Alice.Player.Modules
{

    public class CollisionBroadcaster : MonoBehaviour
    {
        private SGEntity cachedEntity;

        public void SetColliderTarget(SGEntity entity)
        {
            cachedEntity = entity;
        }

        void OnTriggerEnter(Collider other)
        {
            SendObjectCollision(other.gameObject, true);
        }

        void OnTriggerExit(Collider other)
        {
            SendObjectCollision(other.gameObject, false);
        }

        private void SendObjectCollision(GameObject collidedObject, bool onEnter)
        {
            if(cachedEntity == null)
                cachedEntity = this.gameObject.GetComponentInParent<SGEntity>();
            SGEntity otherEntity = collidedObject.GetComponentInParent<SGEntity>();
            if(otherEntity != cachedEntity)
                SceneGraph.Current.Scene.ObjectsCollided(otherEntity, cachedEntity, onEnter);
        }
    }

}