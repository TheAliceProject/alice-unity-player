using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using Alice.Player.Primitives;
using BeauRoutine;

namespace Alice.Player.Unity {
    public sealed class SGTextModel : SGModel {

        private string currTextStr = "";
        private GameObject textStrObj;
        private Renderer m_Renderer;
        private MeshFilter m_MeshFilter;
        private MaterialPropertyBlock m_PropertyBlock;
        private Bounds defaultBounds = new Bounds(UnityEngine.Vector3.zero, new UnityEngine.Vector3(1f, 1f, 1f));
        public const string TEXT_PROPERTY_NAME = "TextProperty";
        protected override string PaintTextureName { get { return MAIN_TEXTURE_SHADER_NAME; } }

        protected override Bounds GetMeshBounds() {
            const float epsilon = 1e-4f;
            if(System.Math.Abs(m_MeshFilter.sharedMesh.bounds.size.x) < epsilon)
                return defaultBounds;
            else
                return m_MeshFilter.sharedMesh.bounds;
        }

        protected override void SetSize(UnityEngine.Vector3 inSize) {
            // Scale the text to match alice size and proportions
            inSize.y *= .678f; // Constant to match 3D Text plugin with Alice text
            inSize.x = inSize.y;
            m_ModelTransform.localScale = inSize;
        }

        protected override void OnOpacityChanged() {
           ApplyOpacity(m_Renderer, ref m_PropertyBlock);
        }

        protected override void OnPaintChanged() {
            ApplyPaint(m_Renderer, ref m_PropertyBlock);
        }

        protected override void Awake() {
            base.Awake();
            RegisterPropertyDelegate(TEXT_PROPERTY_NAME, OnTextPropertyChanged);
            Create();
            CacheMeshBounds();
        }

        private void RefreshText()
        {
            if(textStrObj != null)
            {
                FlyingText.UpdateObject(textStrObj, currTextStr);
                CacheMeshBounds();
            }
        }

        public void Create()
        {
            currTextStr = "";
            textStrObj = FlyingText.GetObject(currTextStr);
            GameObject textPivot = new GameObject();
            textPivot.name = "TextPivot";
            textPivot.transform.parent = this.transform;
            textStrObj.transform.parent = textPivot.transform;
            textStrObj.transform.SetPosition(-0.654f, Axis.Y, Space.Self);
            m_Renderer = textStrObj.GetComponent<MeshRenderer>();
            m_MeshFilter = textStrObj.GetComponent<MeshFilter>();
            m_ModelTransform = textPivot.transform;
        }

        private void OnTextPropertyChanged(TValue inValue) {
            currTextStr = inValue.ToTextString();
            RefreshText();
        }
    }
}