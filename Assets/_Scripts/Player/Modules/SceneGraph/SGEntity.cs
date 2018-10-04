using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using Alice.Player.Unity;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System;
using System.Collections.Generic;

namespace Alice.Player.Modules {
    
    public abstract class SGEntity : MonoBehaviour {
        
        public delegate void UpdatePropertyDelegate(TValue inValue);

        public static T Create<T>(object owner, string inName = "SGEntity") where T : SGEntity {
            var go = new GameObject(inName);
            var entity = go.AddComponent<T>();
            entity.owner = owner;
            return entity;
        }

        public const string POSITION_PROPERTY_NAME = "Position";
        public const string ORIENTATION_PROPERTY_NAME = "Rotation";

        

        private Dictionary<string, UpdatePropertyDelegate> m_PropertyBindings = new Dictionary<string, UpdatePropertyDelegate>();
        private Dictionary<object, UpdatePropertyDelegate> m_Properties = new Dictionary<object, UpdatePropertyDelegate>();

        private SGEntity m_Vehicle;
        private Renderer m_Renderer;
        private Material m_Material;

        public object owner { get; private set; }

        public SGEntity vehicle {
            get { return m_Vehicle; }
            set {
                if (value != m_Vehicle) {
                    m_Vehicle = value;
                    transform.SetParent(m_Vehicle?.transform, true);
                }
            }
        }

        protected void RegisterPropertyDelegate<T>(string inName, UpdatePropertyDelegate inDelegate) {
            if (m_PropertyBindings.ContainsKey(inName)) {
                throw new SceneGraphException(string.Format("Property \"{0}\" binding already registered.", inName));
            } else {
                m_PropertyBindings.Add(inName, inDelegate);
            }
        }

        public void BindProperty(string inName, TValue inProperty, TValue inInitValue) {

            if (m_Properties.ContainsKey(inName)) {
                throw new SceneGraphException(string.Format("Property \"{0}\" already bound.", inName));
            } else {
                
                UpdatePropertyDelegate @delegate;
                if (m_PropertyBindings.TryGetValue(inName, out @delegate)) {

                    @delegate(inInitValue);
                    m_Properties.Add(inProperty.Object(), @delegate);
                } else {
                    throw new SceneGraphException(string.Format("Property \"{0}\" cannot be bound because no valid callback has been registered.", inName));
                }
            }
        }

        public void UpdateProperty(TValue inProperty, TValue inValue) {
            UpdatePropertyDelegate @delegate;
            object propertyObj = inProperty.Object();
            if (m_Properties.TryGetValue(propertyObj, out @delegate)) {
                @delegate(inValue);
            }
        }

        public void UnbindProperty(TValue inProperty) {
            m_Properties.Remove(inProperty.Object());
        } 
    }
}