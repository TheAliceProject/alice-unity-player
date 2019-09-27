using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System;
using System.Collections.Generic;

namespace Alice.Player.Unity {
    
    public abstract class SGEntity : MonoBehaviour {
        
        public delegate void UpdatePropertyDelegate(TValue inValue);

        public static T Create<T>(TValue inOwner) where T : SGEntity {
            var host = new GameObject(typeof(T).Name);
            host.transform.SetParent(SceneGraph.Current.transform, false);
            var entity = host.AddComponent<T>();
            entity.owner = inOwner;
            return entity;
        }

        public static T Create<T>(TValue inOwner, GameObject inHost) where T : SGEntity {
            var entity = inHost.AddComponent<T>();
            entity.owner = inOwner;
            return entity;
        }

        private Dictionary<string, UpdatePropertyDelegate> m_PropertyBindings = new Dictionary<string, UpdatePropertyDelegate>();
        private Dictionary<object, UpdatePropertyDelegate> m_Properties = new Dictionary<object, UpdatePropertyDelegate>();

        private SGEntity m_Vehicle;

        public Transform cachedTransform { get; private set; }

        public TValue owner { get; private set; }

        public SGEntity vehicle {
            get { return m_Vehicle; }
            set {
                if (value != m_Vehicle) {
                    m_Vehicle = value;
                    
                    if(value is SGJoint){
                        Transform holder = m_Vehicle.cachedTransform.Find("holder");
                        if(holder == null){
                            holder = new GameObject("holder").transform;  
                        }
                        (value as SGJoint).GetParentJointedModel().AddToVehicleList(holder);
                        holder.SetParent(m_Vehicle?.cachedTransform);
                        holder.localPosition = UnityEngine.Vector3.zero;
                        cachedTransform.SetParent(holder, true);
                    }
                    else{
                        cachedTransform.SetParent(m_Vehicle?.cachedTransform, true);
                    }
                }
            }
        }

        public void SetName(string inName) {
            gameObject.name = string.Format("{0} ({1})", inName, GetType().Name);
        }

        protected void RegisterPropertyDelegate(string inName, UpdatePropertyDelegate inDelegate) {
            if (m_PropertyBindings.ContainsKey(inName)) {
                throw new SceneGraphException(string.Format("Property \"{0}\" binding already registered.", inName));
            } else {
                m_PropertyBindings.Add(inName, inDelegate);
            }
        }

        public void BindProperty(string inName, TValue inProperty, TValue inInitValue) {
            UpdatePropertyDelegate @delegate;
            if (m_PropertyBindings.TryGetValue(inName, out @delegate)) {

                @delegate(inInitValue);
                m_Properties.Add(inProperty.Object(), @delegate);
            } else {
                throw new SceneGraphException(string.Format("Property \"{0}\" cannot be bound because no valid callback has been registered.", inName));
            }
        }

        public void UpdateProperty(TValue inProperty, TValue inValue) {
            UpdatePropertyDelegate @delegate;
            object propertyObj = inProperty.Object();
            if (m_Properties.TryGetValue(propertyObj, out @delegate)) {
                @delegate(inValue);
            } else {
                Debug.LogFormat("no delegate found for property type {0} of {1}", inProperty.Type.Name, this.GetType().Name);
            }
        }

        public void UnbindProperty(TValue inProperty) {
            m_Properties.Remove(inProperty.Object());
        }

        protected virtual void Awake() {
            cachedTransform = transform;
        }

        public abstract void CleanUp();
    }
}