using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public abstract class SGTransformableEntity : SGEntity {
        public const string TRANSFORMATION_PROPERTY_NAME = "Transform";

        protected override void Awake() {
            base.Awake();
            RegisterPropertyDelegate(TRANSFORMATION_PROPERTY_NAME, OnTransformationPropertyChanged);
        }

        protected virtual void OnTransformationPropertyChanged(TValue inValue) {
            VantagePoint vp = inValue.RawObject<VantagePoint>();
            // convert to left-handedness
            cachedTransform.localPosition = vp.UnityPosition();
            cachedTransform.localRotation = vp.UnityRotation();
        }

    }
}