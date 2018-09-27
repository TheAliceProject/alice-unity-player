using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System;
using System.Collections.Generic;

namespace Alice.Player.Modules {
    
    [PInteropType]
    public abstract class SGEntity : MonoBehaviour {
        
        public static T Create<T>(TValue owner, string inName = "SGEntity") where T : SGEntity {
            var go = new GameObject(inName);
            var entity = go.AddComponent<T>();
            entity.m_Owner = owner;
            return entity;
        }

        private TValue m_Owner;

        private Dictionary<string, PropertyCallbackBinding> m_PropertyBindings = new Dictionary<string, PropertyCallbackBinding>();
        private Dictionary<string, object> m_Properties = new Dictionary<string, object>();

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
        public void bindDecimalNumberProperty(string name, DecimalNumberProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void bindWholeNumberProperty(string name, WholeNumberProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void bindAngleProperty(string name, AngleProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void bindAxisAlignedBoxProperty(string name, AxisAlignedBoxProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void bindColorProperty(string name, ColorProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void bindDirectionProperty(string name, DirectionProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void bindOrientationProperty(string name, OrientationProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void bindPaintProperty(string name, PaintProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void bindPortionProperty(string name, PortionProperty property) {
            BindProperty(name, property);
        }


        [PInteropMethod]
        public void bindPositionProperty(string name, PositionProperty property) {
            BindProperty(name, property);
        }

        
        [PInteropMethod]
        public void bindScaleProperty(string name, ScaleProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void bindSizeProperty(string name, SizeProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void bindVantagePointProperty(string name, VantagePointProperty property) {
            BindProperty(name, property);
        }

        [PInteropMethod]
        public void unbindProperty(string name) {
            PropertyCallbackBinding binding;
            if (m_PropertyBindings.TryGetValue(name, out binding)) {
                UnbindProperty(name, binding.type, binding.callback);
            }
           
        }
        #endregion

        
        protected virtual void Init(Renderer inRenderer) {
            m_Renderer = inRenderer;
            m_Material = new Material(inRenderer.material);
            inRenderer.sharedMaterial = m_Material;

            RegisterPropertyBinding<Paint>(PAINT_PROPERTY_NAME, OnPaintPropertyChanged);
        }

        protected virtual void OnDestroy() {
            Destroy(m_Material);
        }

        protected void RegisterPropertyBinding<T>(string inName, PropertyBase<T>.ValueChangedDelegate inCallback) {
            if (m_PropertyBindings.ContainsKey(inName)) {
                throw new SceneGraphException(string.Format("Property \"{0}\" binding already registered.", inName));
            } else {
                m_PropertyBindings.Add(inName, new PropertyCallbackBinding() {type = typeof(T), callback = inCallback});
            }
        }

        private void BindProperty<T>(string inName, PropertyBase<T> inProperty) {

            if (m_Properties.ContainsKey(inName)) {
                throw new SceneGraphException(string.Format("Property \"{0}\" already bound.", inName));
            } else {
                
                PropertyCallbackBinding binding;
                if (m_PropertyBindings.TryGetValue(inName, out binding)) {
                    var callback = (PropertyBase<T>.ValueChangedDelegate)binding.callback;
                    callback(inProperty);
                    inProperty.OnValueChanged += callback;
                    m_Properties.Add(inName, inProperty);
                } else {
                    throw new SceneGraphException(string.Format("Property \"{0}\" cannot be bound because no valid callback has been registered.", inName));
                }
            }
        }

        private void UnbindProperty<T>(string inName, T inType, object callbackObj) {
            object propertyObj;
            if (m_Properties.TryGetValue(inName, out propertyObj)) {
                var property = (PropertyBase<T>)propertyObj;
                var callback = (PropertyBase<T>.ValueChangedDelegate)callbackObj;
                property.OnValueChanged -= callback;
                m_Properties.Remove(inName);
            }
        } 

        private void OnPaintPropertyChanged(PropertyBase<Paint> inProperty) {
            var paint = inProperty.getValue();
            m_Material.mainTexture = paint.TextureValue;
            m_Material.color = paint.TextureValue == null ? paint : UnityEngine.Color.white;
        }
    }
}