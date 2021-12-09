using UnityEngine;
using Alice.Tweedle;
using System.Collections.Generic;
using Alice.Player.Primitives;

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
        private Dictionary<string, TObject> m_NamedProperties = new Dictionary<string, TObject>();
        private Dictionary<TObject, UpdatePropertyDelegate> m_Properties = new Dictionary<TObject, UpdatePropertyDelegate>();

        private SGEntity m_Vehicle;

        public Transform cachedTransform { get; private set; }

        public TValue owner { get; private set; }

        public SGEntity vehicle {
            get { return m_Vehicle; }
            set {
                if (value == m_Vehicle) return;
                m_Vehicle = value;

                if(value is SGJoint sgJoint) {
                    sgJoint.GetParentJointedModel().AddRider(cachedTransform);
                } else {
                    // Release a holding object
                    if(m_Vehicle.cachedTransform.name == "handHolder")
                    {
                        // The m_Vehicle.cachedTransform.parent is supposed to be SGScene
                        Destroy(m_Vehicle.cachedTransform.gameObject);
                        m_Vehicle.cachedTransform = m_Vehicle.cachedTransform.parent;
                    }
                    // Hold an object
                    if(m_Vehicle.cachedTransform.name.Contains("(SGVRHand)"))
                    {
                        Transform holder = new GameObject("handHolder").transform;

                        UnityEngine.Vector3 controllerRay = m_Vehicle.cachedTransform.parent.forward;
                        UnityEngine.Vector3 objectRay = cachedTransform.position - m_Vehicle.cachedTransform.parent.position;
                        UnityEngine.Vector3 offsetRay = objectRay - UnityEngine.Vector3.Dot(controllerRay, objectRay) * controllerRay;
                        holder.position = offsetRay + m_Vehicle.cachedTransform.parent.position;
                        holder.SetParent(m_Vehicle.cachedTransform, true);
                        holder.localEulerAngles = UnityEngine.Vector3.zero;
                        holder.localScale = UnityEngine.Vector3.one;
                        m_Vehicle.cachedTransform = holder;
                    }
                }
                cachedTransform.SetParent(m_Vehicle.cachedTransform, true);
            }
        }

        public abstract void AddEntityCollider();

        public abstract void AddMouseCollider();

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
                m_NamedProperties.Add(inName, inProperty.Object());
            } else {
                throw new SceneGraphException(string.Format("Property \"{0}\" cannot be bound because no valid callback has been registered.", inName));
            }
        }

        public void UpdateProperty(string inName, TValue inValue) {
            TObject propertyObj;
            if (m_NamedProperties.TryGetValue(inName, out propertyObj)) {
                propertyObj.Set("value", inValue);
                UpdateProperty(propertyObj, inValue);
            } else {
                throw new SceneGraphException(string.Format("Property \"{0}\" cannot be updated because no valid callback has been registered.", inName));
            }
        }

        public void UpdateProperty(TValue inProperty, TValue inValue) {
            TObject propertyObj = inProperty.Object();
            UpdateProperty(propertyObj, inValue);
        }

        private void UpdateProperty(TObject propertyObj, TValue inValue) {
            UpdatePropertyDelegate @delegate;
            if (m_Properties.TryGetValue(propertyObj, out @delegate)) {
                @delegate(inValue);
            } else {
                Debug.LogFormat("no delegate found for property type {0} of {1}", propertyObj.GetType().Name, this.GetType().Name);
            }
        }

        public void UnbindProperty(TValue inProperty) {
            m_Properties.Remove(inProperty.Object());
        }

        protected virtual void Awake() {
            cachedTransform = transform;
        }

        public abstract void CleanUp();

        public virtual VantagePoint GetLocalTransformation() {
            return VantagePoint.FromUnity(cachedTransform.localPosition, cachedTransform.localRotation);
        }

        // Helper method for those entities under transforms that are not represented by SGEntities
        protected VantagePoint GetLocalTransformationUpTo(SGEntity sgParent) {
            var parent = cachedTransform.parent;
            var cumulativeTransform = VantagePoint.FromUnity(cachedTransform.localPosition, cachedTransform.localRotation);
            while (parent != null && (parent.gameObject != sgParent.gameObject)) {
                var parentAliceTransform = VantagePoint.FromUnity(parent.localPosition, parent.localRotation);
                cumulativeTransform = parentAliceTransform.multiply(cumulativeTransform);
                parent = parent.parent;
            }
            return cumulativeTransform;
        }

        public virtual VantagePoint GetAbsoluteTransformation() {
            return VantagePoint.FromUnity(cachedTransform.position, cachedTransform.rotation);
        }
    }
}