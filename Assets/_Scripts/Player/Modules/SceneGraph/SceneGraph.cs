using UnityEngine;
using System.Collections.Generic;
using Alice.Tweedle;
using Alice.Tweedle.Interop;
using System;
using UnityEngine.XR;

namespace Alice.Player.Unity {
    public sealed class SceneGraph : MonoBehaviour {

        private SceneCanvas m_SceneCanvas;

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
        public AudioCache AudioCache { get; private set; }
        public ModelCache ModelCache {get; private set;}

        private List<SGEntity> m_Entities = new List<SGEntity>();
        private Transform m_ModelCacheRoot;

        private List<IWaitReturn> m_WaitReturnsQueue = new List<IWaitReturn>();
        private List<IWaitReturn> m_WaitReturns = new List<IWaitReturn>();

        public SGScene Scene { get; set; } 

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
            AudioCache = new AudioCache();
            ModelCache = new ModelCache(m_ModelCacheRoot);

            m_SceneCanvas = CreateCanvas();
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

        private SceneCanvas CreateCanvas()
        {
            SceneCanvas canvas = null;
            if(XRSettings.enabled)
            {
                canvas = Instantiate(InternalResources.VRSceneCanvas);
                canvas.transform.SetParent(VRControl.Rig().canvasRoot);
                InitializeVrCanvas(canvas);
            }
            else
            {
                canvas = Instantiate(InternalResources.SceneCanvas);
                AttachToScene(canvas);
            }

            return canvas;
        }

        public SceneCanvas CreateNewWorldCanvas()
        {
            // We want to spawn this in the direction the player is looking, but at a certain distance
            var canvas = Instantiate(InternalResources.WorldCanvas);
            InitializeVrCanvas(canvas);
            AttachToScene(canvas);
            return canvas;
        }

        private static void InitializeVrCanvas(SceneCanvas canvas)
        {
            var headTransform = VRControl.Rig().head;

            // Get player facing direction
            UnityEngine.Vector3 facingDirection = headTransform.position + headTransform.forward;
            // Reset height to player height
            facingDirection.y = headTransform.position.y;
            // Get direction vector of head
            UnityEngine.Vector3 directionVector = facingDirection - headTransform.position;
            // Normalize and set a certain distance away
            canvas.transform.position = headTransform.position + (directionVector.normalized * VRControl.WORLD_CANVAS_DISTANCE);

            // Rotate the canvas correctly
            canvas.transform.LookAt(headTransform);
            canvas.transform.Rotate(0f, 180f, 0f, Space.Self);
        }

        private void AttachToScene(SceneCanvas canvas)
        {
            canvas.transform.SetParent(transform);
        }

        public SceneCanvas GetCurrentCanvas()
        {
            return m_SceneCanvas;
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
                for (int i = 0; i < m_Entities.Count; i++) {
                    if (ReferenceEquals(m_Entities[i].owner.RawObject<object>(), ownerObj)) {
                        return (T)m_Entities[i];
                    }
                }
            }
            return null;
        }

        internal List<T> FindAllEntities<T>() where T : SGEntity {
            List<T> entities = new List<T>();
            for (int i = 0, count = m_Entities.Count; i < count; ++i) {
                if ((m_Entities[i] as T) != null) {
                    entities.Add((T)m_Entities[i]);
                }
            }
            return entities;
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
            }
            m_Entities.Clear();

            Destroy(m_ModelCacheRoot);
            
            TextureCache.Clear();
            AudioCache.Clear();
            TextureCache = null;
            AudioCache = null;
            Scene = null;
            Destroy();
        }

        private void Destroy() {
            if (ReferenceEquals(s_Current, this)) {
                s_Current = null;
            }
        }
    }
}
