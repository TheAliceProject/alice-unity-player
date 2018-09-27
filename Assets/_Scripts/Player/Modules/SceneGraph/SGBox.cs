using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Modules {
    [PInteropType]
    public sealed class SGBox : SGEntity {

        private Transform m_BoxTransform;

        #region Interop Interfaces
        
        #endregion //Interop Interafaces

        private void Awake() {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            m_BoxTransform = go.transform;
            m_BoxTransform.SetParent(transform, false);
            m_BoxTransform.localPosition = UnityEngine.Vector3.zero;
            m_BoxTransform.localRotation = UnityEngine.Quaternion.identity;
            Init(go.GetComponent<MeshRenderer>());
        }

        protected override void BindProperty<T>(string inName, PropertyBase<T> inProperty) {
            if (inName == SIZE_PROPERTY_NAME) {
                var sizeProperty = inProperty as SizeProperty;
                if (sizeProperty == null) {
                    throw new SceneGraphException("Expecting SizeProperty when binding.");
                }

                OnSizePropertyChanged(sizeProperty);
                sizeProperty.OnValueChanged += OnSizePropertyChanged;
            } else {
                base.BindProperty(inName, inProperty);
            }
        }

        private void OnSizePropertyChanged(PropertyBase<Size> inProperty) {
            m_BoxTransform.localScale = inProperty.getValue();
        }

    }
}