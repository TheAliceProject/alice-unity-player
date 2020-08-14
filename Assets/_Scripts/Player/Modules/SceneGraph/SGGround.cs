using System;
using System.Collections;
using Alice.Player.Modules;
using UnityEngine;
using Alice.Player.Primitives;
using Alice.Tweedle;

namespace Alice.Player.Unity {
    public sealed class SGGround : SGShape {

        private Renderer m_GroundBacksideRenderer;
        private Transform m_GroundBacksideTransform;
        private MaterialPropertyBlock m_GroundBacksidePropertyBlock;

        protected override Mesh ShapeMesh { get { return SceneGraph.Current?.InternalResources?.GroundMesh; } }

        protected override void Awake() {
            base.Awake();

            // plane mesh is 10x10
            m_ModelTransform.localScale = new UnityEngine.Vector3(100, 100, 100);
            m_ModelTransform.localPosition = new UnityEngine.Vector3(0, -0.0001f, 0);

            GetPropertyBlock(m_Renderer, ref m_PropertyBlock);
            m_PropertyBlock.SetVector("_MainTex_ST", new Vector4(-128, -128, -0.5f, -0.5f));
            m_Renderer.SetPropertyBlock(m_PropertyBlock);

            MeshFilter groundBacksideFilter;
            CreateModelObject(SceneGraph.Current?.InternalResources?.GroundMesh, OpaqueMaterial, transform, out m_GroundBacksideTransform, out m_GroundBacksideRenderer, out groundBacksideFilter);

            m_GroundBacksideTransform.localScale = new UnityEngine.Vector3(100, 100, 100);
            m_GroundBacksideTransform.localPosition = new UnityEngine.Vector3(0, -0.0001f, 0);
            m_GroundBacksideTransform.Rotate(new UnityEngine.Vector3(0, 0, 1), 180);

            GetPropertyBlock(m_GroundBacksideRenderer, ref m_GroundBacksidePropertyBlock);
            m_GroundBacksidePropertyBlock.SetVector("_MainTex_ST", new Vector4(-128, -128, -0.5f, -0.5f));
            m_GroundBacksideRenderer.SetPropertyBlock(m_GroundBacksidePropertyBlock);
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

        protected override void OnPaintChanged()
        {
            base.OnPaintChanged();
            ApplyPaint(m_GroundBacksideRenderer, ref m_GroundBacksidePropertyBlock);
        }
    }
}