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

        private void OnTransformationPropertyChanged(TValue inValue) {
            VantagePoint vp = inValue.RawStruct<VantagePoint>();
            var posVal = vp.position.Value;
            var rotVal = vp.orientation.Value;
            // convert to left-handedness
            cachedTransform.localPosition = new UnityEngine.Vector3((float)posVal.X, (float)posVal.Y, -(float)posVal.Z);
            cachedTransform.localRotation = new UnityEngine.Quaternion((float)rotVal.X, (float)rotVal.Y, -(float)rotVal.Z, -(float)rotVal.W);
        }

    }
}