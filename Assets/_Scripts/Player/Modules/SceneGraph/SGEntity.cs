using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using Alice.Tweedle.Interop;
using Alice.Tweedle;

namespace Alice.Player.Modules {
    
    [PInteropType]
    public abstract class SGEntity : MonoBehaviour {
        
        public static T Create<T>(string inName = "SGEntity") where T : SGEntity {
            var go = new GameObject(inName);
            var entity = go.AddComponent<T>();
            return entity;
        }

        private Renderer m_Renderer;
        private Material m_Material;
        
        #region Interop Interfaces
        [PInteropField]
        public const string POSITION_PROPERTY_NAME = "Position";
        [PInteropField]
        public const string ORIENTATION_PROPERTY_NAME = "Rotation";
        [PInteropField]
        public const string PAINT_PROPERTY_NAME = "Paint";
        [PInteropField]
        public const string SIZE_PROPERTY_NAME = "Size";

        [PInteropMethod]
        public void bindPositionProperty(string name, PositionProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void bindOrientationProperty(string name, OrientationProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void bindDecimalNumberProperty(string name, DecimalNumberProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void bindSizeProperty(string name, SizeProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void bindPaintProperty(string name, PaintProperty property) {
            BindProperty(name, property);
        }
        #endregion


        protected virtual void Init(Renderer inRenderer) {
            m_Renderer = inRenderer;
            m_Material = new Material(inRenderer.material);
            inRenderer.sharedMaterial = m_Material;
        }

        protected virtual void OnDestroy() {
            Destroy(m_Material);
        }

        protected virtual void BindProperty<T>(string inName, PropertyBase<T> inProperty) {
            if (inName == PAINT_PROPERTY_NAME) {
                var paintProperty = inProperty as PaintProperty;
                if (paintProperty == null) {
                    throw new SceneGraphException(string.Format("Expecting PaintProperty when binding property named {0}.", inName));
                }

                OnPaintPropertyChanged(paintProperty);
                paintProperty.OnValueChanged += OnPaintPropertyChanged;
            }
        }

        private void OnPaintPropertyChanged(PropertyBase<Paint> inProperty) {
            var paint = inProperty.getValue();
            m_Material.mainTexture = paint.TextureValue;
            m_Material.color = paint.TextureValue == null ? paint : UnityEngine.Color.white;
        }
    }
}