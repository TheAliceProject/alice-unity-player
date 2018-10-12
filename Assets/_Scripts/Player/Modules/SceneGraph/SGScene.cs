using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System;
using System.Collections.Generic;

namespace Alice.Player.Unity {
    
    public sealed class SGScene : SGEntity {

        private List<PAction> m_ActivationListeners = new List<PAction>();

        public void AddActivationListener(PAction inListener) {
            m_ActivationListeners.Add(inListener);
        }

        public void Activate() {
            for (int i = 0, count = m_ActivationListeners.Count; i < count; ++i) {
                m_ActivationListeners[i].Call();
            }
        }
    
    }
}