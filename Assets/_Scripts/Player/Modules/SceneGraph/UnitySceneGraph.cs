using UnityEngine;
using System.Collections.Generic;

namespace Alice.Player.Unity {
    public sealed class UnitySceneGraph : MonoBehaviour {
        private static UnitySceneGraph s_Instance;

        public static UnitySceneGraph Instance {
            get {
                if (ReferenceEquals(s_Instance, null)) {
                    s_Instance = FindObjectOfType<UnitySceneGraph>();
                    if (ReferenceEquals(s_Instance, null)) {
                        var go = new GameObject("PropertyManager");
                        s_Instance = go.AddComponent<UnitySceneGraph>();
                    }
                }
                return s_Instance;
            }
        }

        public List<IPropertyTween> m_Tweens = new List<IPropertyTween>();
        private bool m_IsUpdating;

        private void Awake() {
            if (!ReferenceEquals(s_Instance, null) && !ReferenceEquals(s_Instance, this)) {
                Destroy(this);
                return;
            } else if (ReferenceEquals(s_Instance, null)) {
                s_Instance = this;
            }
        }

        private void Destroy() {
            if (ReferenceEquals(s_Instance, this)) {
                s_Instance = null;
            }
        }

        private void Update() {
            m_IsUpdating = true;
            // process tweens in order
            double dt = Time.deltaTime;
            for (int i = 0; i < m_Tweens.Count; ++i) {
                if (m_Tweens[i].IsDone()) {
                    m_Tweens[i].Finish();
                    m_Tweens.RemoveAt(i);
                    --i;
                } else {
                    m_Tweens[i].Step(dt);
                }
            }
            m_IsUpdating = false;
        }

        public void QueueTween(IPropertyTween inTween) {
            m_Tweens.Add(inTween);
        }

    }
}