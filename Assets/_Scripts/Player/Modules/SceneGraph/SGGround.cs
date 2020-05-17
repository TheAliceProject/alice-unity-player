using System;
using Alice.Player.Modules;
using UnityEngine;

namespace Alice.Player.Unity {
    public sealed class SGGround : SGShape {
        protected override Mesh ShapeMesh { get { return SceneGraph.Current?.InternalResources?.GroundMesh; } }

        protected override void Awake() {
            base.Awake();

            // plane mesh is 10x10
            m_ModelTransform.localScale = new UnityEngine.Vector3(100, 100, 100);
            m_ModelTransform.localPosition = new UnityEngine.Vector3(0,-0.0001f,0);

            GetPropertyBlock(m_Renderer, ref m_PropertyBlock);
            m_PropertyBlock.SetVector("_MainTex_ST", new Vector4(-128,-128,-0.5f,-0.5f));
            m_Renderer.SetPropertyBlock(m_PropertyBlock);
        }

        protected override void CreateEntityCollider()
        {
            if (m_Renderer.gameObject.GetComponent<BoxCollider>() != null) return;
            var boxCollider = gameObject.AddComponent<BoxCollider>();
            var bounds = GetBounds(true);
            boxCollider.size = bounds.size;
            gameObject.AddComponent<CollisionBroadcaster>();
        }

        protected override void CreateMouseCollider()
        {
            throw new NotSupportedException("No mouse clicks recognized on the ground");
        }
    }
}