using UnityEngine;
using System.Collections.Generic;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using Alice.Tweedle;
using Alice.Tweedle.Parse;
using Alice.Tweedle.Interop;
using System;

namespace Alice.Player.Unity {
    public sealed class SceneGraph : MonoBehaviour {

        private interface IWaitReturn : IDisposable {
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

            public void Dispose() {

            }
        }

        private class FrameReturn : IWaitReturn {
            private static Stack<FrameReturn> s_Pool = new Stack<FrameReturn>(64);

            static public FrameReturn Create(AsyncReturn inReturn, int inFrames)
            {
                FrameReturn obj = null;
                if (s_Pool.Count > 0) {
                    obj = s_Pool.Pop();
                    obj.m_Duration = inFrames;
                    obj.Return = inReturn;
                } else {
                    obj = new FrameReturn(inReturn, inFrames);
                }
                obj.OnCreate();

                return obj;
            }

            private bool m_InUse = false;
            private int m_Duration;
            private int m_Frame;
            public AsyncReturn Return { get; private set; }

            public FrameReturn(AsyncReturn inReturn, int inFrames) {
                Return = inReturn;
                m_Duration = inFrames;
            }

            public bool Step(float time) {
                m_Frame++;
                return m_Frame >= m_Duration;
            }

            private void OnCreate()
            {
                Debug.Assert(!m_InUse);

                m_InUse = true;
            }

            void IDisposable.Dispose()
            {
                Debug.Assert(m_InUse);
                Return = null;
                m_Duration = 0;
                m_Frame = 0;
                s_Pool.Push(this);
                m_InUse = false;
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

        public static bool Exists { get { return !ReferenceEquals(s_Current, null); } }

        public PlayerResources InternalResources {get; private set;}
        public TextureCache TextureCache {get; private set;}
        public ModelCache ModelCache {get; private set;}

        private List<SGEntity> m_Entities = new List<SGEntity>();
        private Transform m_ModelCacheRoot;

        private List<IWaitReturn> m_WaitReturnsQueue = new List<IWaitReturn>();
        private List<IWaitReturn> m_WaitReturns = new List<IWaitReturn>();

        private bool m_IsUpdating;

        private void Awake() {
            if (!ReferenceEquals(s_Current, null) && !ReferenceEquals(s_Current, this)) {
                Destroy(this);
                return;
            } else if (ReferenceEquals(s_Current, null)) {
                s_Current = this;
            }

            m_ModelCacheRoot = new GameObject("ModelCache").transform;
            m_ModelCacheRoot.SetParent(transform, false);

            InternalResources = Resources.Load<PlayerResources>("PlayerResources");
            TextureCache = new TextureCache();
            ModelCache = new ModelCache(m_ModelCacheRoot);
        }

        private void Update() {
            m_IsUpdating = true;

            if (m_WaitReturnsQueue.Count != 0) {
                for (int i = 0, count = m_WaitReturnsQueue.Count; i < count; ++i) {
                    m_WaitReturns.Add(m_WaitReturnsQueue[i]);
                }
                m_WaitReturnsQueue.Clear();
            }

            for (int i = 0; i < m_WaitReturns.Count; ++i) {
                bool done = m_WaitReturns[i].Step(Time.time);
                if (done) {
                    m_WaitReturns[i].Return.Return();
                    m_WaitReturns[i].Dispose();
                    m_WaitReturns.RemoveAt(i);
                    --i;
                }
            }

            m_IsUpdating = false;
        }
        
        internal void QueueFrameReturn(AsyncReturn inReturn, int inFrames) {
            QueueWaitReturn(FrameReturn.Create(inReturn, inFrames));
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

        /**
        * Cleans up and destroys all scene graph entities
        */
        public void Clear() {
            for (int i = 0; i < m_Entities.Count; ++i) {
                m_Entities[i].CleanUp();
                Destroy(m_Entities[i].gameObject);
            }
            m_Entities.Clear();

            Destroy(m_ModelCacheRoot);
            
            TextureCache.Clear();
            TextureCache = null;
        }

        private void Destroy() {
            if (ReferenceEquals(s_Current, this)) {
                s_Current = null;
            }
        }
    }
}
