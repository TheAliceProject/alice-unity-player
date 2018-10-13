using UnityEngine;
using System.Collections.Generic;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using Alice.Tweedle;
using Alice.Tweedle.Interop;

namespace Alice.Player.Unity {
    public sealed class SceneGraph : MonoBehaviour {

        private interface IWaitReturn {
            bool Step(float time);
            AsyncReturn Return {get;}
        }

        private struct TimeReturn : IWaitReturn {
            private float m_StartTime;
            private float m_Duration;
            public AsyncReturn Return { get; private set; }
            
            public TimeReturn(AsyncReturn inReturn, float startTime, float duration) {
                Return = inReturn;
                m_StartTime = startTime;
                m_Duration = duration;
            }

            public bool Step(float time) {
                return time - m_StartTime >= m_Duration;
            }
        }

        private class FrameReturn : IWaitReturn {
            private int m_Duration;
            private int m_Frame;
            public AsyncReturn Return { get; private set; }

            public FrameReturn(AsyncReturn inReturn, int frames) {
                Return = inReturn;
                m_Duration = frames;
            }

            public bool Step(float time) {
                m_Frame++;
                return m_Frame >= m_Duration;
            }
        }

        private static SceneGraph s_Current;

        public static SceneGraph Current {
            get {
                if (ReferenceEquals(s_Current, null)) {
                    s_Current = FindObjectOfType<SceneGraph>();
                    if (ReferenceEquals(s_Current, null)) {
                        var go = new GameObject("SceneGraph");
                        s_Current = go.AddComponent<SceneGraph>();
                    }
                }
                return s_Current;
            }
        }

        private List<SGEntity> m_Entities = new List<SGEntity>();

        private List<IWaitReturn> m_WaitReturnsQueue = new List<IWaitReturn>();
        private List<IWaitReturn> m_WaitReturns = new List<IWaitReturn>();

        private bool m_IsUpdating;

        public PlayerResources InternalResources {get; private set;}

        private void Awake() {
            if (!ReferenceEquals(s_Current, null) && !ReferenceEquals(s_Current, this)) {
                Destroy(this);
                return;
            } else if (ReferenceEquals(s_Current, null)) {
                s_Current = this;
            }

            InternalResources = Resources.Load<PlayerResources>("PlayerResources");
        }

        private void Destroy() {
            if (ReferenceEquals(s_Current, this)) {
                s_Current = null;
            }
        }

        private void Update() {
            m_IsUpdating = true;

            if (m_WaitReturnsQueue.Count != 0) {
                m_WaitReturns.AddRange(m_WaitReturnsQueue);
                m_WaitReturnsQueue.Clear();
            }

            for (int i = 0; i < m_WaitReturns.Count; ++i) {
                bool done = m_WaitReturns[i].Step(Time.time);
                if (done) {
                    m_WaitReturns[i].Return.Return();
                    m_WaitReturns.RemoveAt(i);
                    --i;
                }
            }

            m_IsUpdating = false;
        }
        
        internal void QueueFrameReturn(AsyncReturn inReturn, int inFrames) {
            QueueWaitReturn(new FrameReturn(inReturn, inFrames));
        }

        internal void QueueTimeReturn(AsyncReturn inReturn, double inSeconds) {
            QueueWaitReturn(new TimeReturn(inReturn, Time.time, (float)inSeconds));
        }

        private void QueueWaitReturn(IWaitReturn inReturn) {
            if (m_IsUpdating) {
                m_WaitReturnsQueue.Add(inReturn);
            } else {
                m_WaitReturns.Add(inReturn);
            }
        }

        internal void AddEntity(SGEntity inEntity) {
            m_Entities.Add(inEntity);
        }

        internal SGEntity FindEntity(TValue inOwner) {
            var ownerObj = inOwner.RawObject<object>();
            if (ownerObj != null) {
                for (int i = 0, count = m_Entities.Count; i < count; ++i) {
                    if (ReferenceEquals(m_Entities[i].owner.RawObject<object>(), ownerObj)) {
                        return m_Entities[i];
                    }
                }
            }
            return null;
        }

        internal T FindEntity<T>(TValue inOwner) where T : SGEntity {
            var ownerObj = inOwner.RawObject<object>();
            if (ownerObj != null) {
                for (int i = 0, count = m_Entities.Count; i < count; ++i) {
                    if (ReferenceEquals(m_Entities[i].owner.RawObject<object>(), ownerObj)) {
                        return (T)m_Entities[i];
                    }
                }
            }
            return null;
        }

        internal void BindProperty(string inName, TValue inOwner, TValue inProperty, TValue inInitValue) {
            var entity = FindEntity(inOwner);
            if (entity) {
                entity.BindProperty(inName, inProperty, inInitValue);
            }
        }

        internal void UpdateProperty(TValue inOwner, TValue inProperty, TValue inValue) {
            var entity = FindEntity(inOwner);
            if (entity) {
                entity.UpdateProperty(inProperty, inValue);
            }
        }

    }

    
}