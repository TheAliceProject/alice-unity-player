using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;

namespace Alice.Player.Unity {
    public sealed class SGGround : SGShape {
        protected override Mesh ShapeMesh { get { return SceneGraph.Current?.InternalResources?.GroundMesh; } }

        protected override void Awake() {
            base.Awake();

            // plane mesh is 10x10
            m_ModelTransform.localScale = new UnityEngine.Vector3(100, 100, 100);
            m_ModelTransform.localPosition = new UnityEngine.Vector3(0,-0.0001f,0);

            GetPropertyBlock(m_Renderer, ref m_PropertyBlock);
            m_PropertyBlock.SetVector("_MainTex_ST", new Vector4(100,100,0,0));
            m_Renderer.SetPropertyBlock(m_PropertyBlock);
        }
    }
}