using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System;
using System.Collections.Generic;

namespace Alice.Player.Unity {
    
    public abstract class SGEntity : MonoBehaviour {
        private UnityEngine.Vector3 m_DefaultColliderSize = new UnityEngine.Vector3(0.1f, 0.1f, 0.1f);
        
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

        public virtual void AddCollider()
        {
            MeshRenderer[] meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
            SkinnedMeshRenderer[] skinnedMeshRenderers =transform.GetComponentsInChildren<SkinnedMeshRenderer>();

            if (!((meshRenderers == null || meshRenderers.Length <= 0) &&
                  (skinnedMeshRenderers == null ||  skinnedMeshRenderers.Length <= 0)))
            {
                foreach (MeshRenderer meshRenderer in meshRenderers)
                {
                    if (meshRenderer.transform.GetComponent<MeshCollider>() != null) continue;

                    MeshCollider meshCollider = meshRenderer.gameObject.AddComponent<MeshCollider>();
                    Rigidbody rigidBody = meshRenderer.gameObject.AddComponent<Rigidbody>(); // Rigidbody is required for collision detection
                    rigidBody.isKinematic = true;
                    meshCollider.convex = true;
                    meshCollider.isTrigger = true;
                    meshRenderer.gameObject.AddComponent<CollisionBroadcaster>();
                }

                foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
                {
                    if (skinnedMeshRenderer.transform.GetComponent<MeshCollider>() != null) continue;

                    MeshCollider meshCollider = skinnedMeshRenderer.gameObject.AddComponent<MeshCollider>();
                    Mesh colliderMesh = new Mesh();
                    skinnedMeshRenderer.BakeMesh(colliderMesh);
                    meshCollider.sharedMesh = colliderMesh;

                    meshCollider.convex = true;
                    meshCollider.isTrigger = true;
                    skinnedMeshRenderer.gameObject.AddComponent<CollisionBroadcaster>();
                }
            }
            else // Add box collider if no renderer exists
            {
                if (gameObject.GetComponent<BoxCollider>() != null) return;

                Rigidbody rigidBody = gameObject.AddComponent<Rigidbody>();
                rigidBody.isKinematic = true;
                BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                boxCollider.size = m_DefaultColliderSize;
                boxCollider.isTrigger = true;
                gameObject.AddComponent<CollisionBroadcaster>();
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
    }
}