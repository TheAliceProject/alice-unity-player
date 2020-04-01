using Alice.Player.Modules;
using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public abstract class SGTransformableEntity : SGEntity {
        public const string TRANSFORMATION_PROPERTY_NAME = "Transform";
        private readonly UnityEngine.Vector3 m_DefaultColliderSize = new UnityEngine.Vector3(0.1f, 0.1f, 0.1f);
        private bool m_HasEntityCollider;
        private bool m_HasMouseCollider;

        protected override void Awake() {
            base.Awake();
            RegisterPropertyDelegate(TRANSFORMATION_PROPERTY_NAME, OnTransformationPropertyChanged);
        }

        protected virtual void OnTransformationPropertyChanged(TValue inValue) {
            VantagePoint vp = inValue.RawObject<VantagePoint>();
            StoreAliceTransformation(vp);
        }

        protected virtual void StoreAliceTransformation(VantagePoint vp) {
            // convert to left-handedness
            cachedTransform.localPosition = vp.UnityPosition();
            cachedTransform.localRotation = vp.UnityRotation();
        }

        internal void UpdateVantagePointProperty(VantagePoint vp) {
            UpdateProperty(TRANSFORMATION_PROPERTY_NAME, vp.AsTValue());
        }

        public override void AddEntityCollider()
        {
            if (m_HasEntityCollider) return;
            m_HasEntityCollider = true;
            CreateEntityCollider();
        }

        protected virtual void CreateEntityCollider()
        {
            CreateMinimalBoxCollider();
        }

        public override void AddMouseCollider()
        {
            if (m_HasMouseCollider) return;
            m_HasMouseCollider = true;
            CreateMouseCollider();
        }

        protected virtual void CreateMouseCollider()
        {
            CreateMinimalBoxCollider();
        }

        private void CreateMinimalBoxCollider()
        {
            if (gameObject.GetComponent<BoxCollider>() != null) return;

            var rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.isKinematic = true;
            var boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.size = m_DefaultColliderSize;
            boxCollider.isTrigger = true;
            gameObject.AddComponent<CollisionBroadcaster>();

            // When used, this box will cover both collider types
            m_HasMouseCollider = true;
            m_HasEntityCollider = true;
        }

        protected void ResetColliderState()
        {
            if (m_HasEntityCollider)
                CreateEntityCollider();
            if (m_HasMouseCollider)
                CreateMouseCollider();
        }
    }
}