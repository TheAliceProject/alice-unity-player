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
            if(m_MeshFilter.sharedMesh.bounds.size.x == 0.0)
                return defaultBounds;
            else
                return m_MeshFilter.sharedMesh.bounds;
        }

        protected override void SetSize(UnityEngine.Vector3 inSize) {
            inSize *= 0.585f; // Scale the text to match alice size
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
            textStrObj.transform.SetPosition(-0.5f, Axis.Y, Space.Self);
            textStrObj.transform.parent = this.transform;
            m_Renderer = textStrObj.GetComponent<MeshRenderer>();
            m_MeshFilter = textStrObj.GetComponent<MeshFilter>();
            m_ModelTransform = textStrObj.transform;
        }

        private void OnTextPropertyChanged(TValue inValue) {
            currTextStr = inValue.ToTextString();
            RefreshText();
        }
    }
}