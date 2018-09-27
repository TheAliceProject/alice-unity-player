using UnityEngine;
using System.Collections.Generic;

namespace Alice.Player.Unity {
    public sealed class UnitySceneGraph : MonoBehaviour {
        private static UnitySceneGraph s_Current;

        public static UnitySceneGraph Current {
            get {
                if (ReferenceEquals(s_Current, null)) {
                    s_Current = FindObjectOfType<UnitySceneGraph>();
                    if (ReferenceEquals(s_Current, null)) {
                        var go = new GameObject("PropertyManager");
                        s_Current = go.AddComponent<UnitySceneGraph>();
                    }
                }
                return s_Current;
            }
        }

        public List<IPropertyTween> m_Tweens = new List<IPropertyTween>();
        private bool m_IsUpdating;

        private void Awake() {
            if (!ReferenceEquals(s_Current, null) && !ReferenceEquals(s_Current, this)) {
                Destroy(this);
                return;
            } else if (ReferenceEquals(s_Current, null)) {
                s_Current = this;
            }
        }

        private void Destroy() {
            if (ReferenceEquals(s_Current, this)) {
                s_Current = null;
            }
        }

        private void Update() {
            m_IsUpdating = true;
            // process tweens in order
            double dt = Time.deltaTime;
            for (int i = 0; i < m_Tweens.Count; ++i) {
                m_Tweens[i].Step(dt);
                if (m_Tweens[i].IsDone()) {
                    m_Tweens[i].Finish();
                    m_Tweens.RemoveAt(i);
                    --i;
                }
            }
            m_IsUpdating = false;
        }

        public void QueueTween(IPropertyTween inTween) {
            m_Tweens.Add(inTween);
        }

    }
}