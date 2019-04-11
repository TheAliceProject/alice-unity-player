using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alice.Player.Unity {
    public class SceneCanvas : MonoBehaviour{
        [SerializeField]
        private SayThinkControl m_SayThinkControl = null;
        public SayThinkControl SayThinkControl { get { return m_SayThinkControl; } }
    }
}
