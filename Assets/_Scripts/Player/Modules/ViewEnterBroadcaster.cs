using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Player.Unity;

namespace Alice.Player.Modules
{
    public class ViewEnterBroadcaster : MonoBehaviour
    {
        private SGModel cachedModel = null;
        
        void OnBecameVisible()
        {
            SendObjectInView(this.gameObject, true);
        }

        void OnBecameInvisible()
        {
            SendObjectInView(this.gameObject, false);
        }

        private void SendObjectInView(GameObject collidedObject, bool enteredView)
        {
            if(cachedModel == null)
                cachedModel = this.gameObject.GetComponentInParent<SGModel>();

            SceneGraph.Current.Scene.ObjectInView(cachedModel, enteredView);
        }
    }

}