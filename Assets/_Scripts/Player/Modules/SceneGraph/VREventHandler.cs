using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;

namespace Alice.Player.Unity 
{
    public class VREventHandler
    {
        private List<VREventHandlerProxy> m_VrEventListeners = new List<VREventHandlerProxy>();

        public void AddVrListener(VREventHandlerProxy listener){
            m_VrEventListeners.Add(listener);
        }

        public void HandleVREvents(){

        }

    }
}
