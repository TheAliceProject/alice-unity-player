using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System;
using System.Collections.Generic;

namespace Alice.Player.Unity {
    
    public abstract class SGModel : SGTransformableEntity {

        static public  void CreateModelObject(Mesh inMesh, Material inMaterial, Transform inParent, out Transform outTransform, out Renderer outRenderer, out MeshFilter outFilter) {
            var go = new GameObject("Model");
            outFilter = go.AddComponent<MeshFilter>();
            outFilter.mesh = inMesh;
            outRenderer = go.AddComponent<MeshRenderer>();
            outRenderer.sharedMaterial = inMaterial;
            outTransform = go.transform;

            outTransform.SetParent(inParent, false);
            outTransform.localPosition = UnityEngine.Vector3.zero;
            outTransform.localRotation = UnityEngine.Quaternion.identity;
        }

        static public void PrepPropertyBlock(Renderer inRenderer, ref MaterialPropertyBlock ioPropertyBlock) {
            if (ioPropertyBlock == null) {
                ioPropertyBlock = new MaterialPropertyBlock();
            }

            if (inRenderer.HasPropertyBlock()) {
                inRenderer.GetPropertyBlock(ioPropertyBlock);
            }
        }

        public const string PAINT_PROPERTY_NAME = "Paint";
        public const string BACK_PAINT_PROPERTY_NAME = "BackPaint";
        public const string SIZE_PROPERTY_NAME = "Size";
        public const string OPACITY_PROPERTY_NAME = "Opacity";

        public const string MAIN_TEXTURE_SHADER_NAME = "_MainTex";
        public const string FILTER_TEXTURE_SHADER_NAME = "_FilterTex";
        public const string COLOR_SHADER_NAME = "_Color";

        
        protected Transform m_ModelTransform;
        private Bounds m_CachedMeshBounds;
        
        private Paint m_CachedPaint = Primitives.Color.WHITE;
        protected float m_CachedOpacity = 1;

        protected Bounds MeshBounds { get {return m_CachedMeshBounds; } }

        protected virtual string PaintTextureName { get { return FILTER_TEXTURE_SHADER_NAME; } }
        protected virtual Material OpaqueMaterial { get { return SceneGraph.Current?.InternalResources?.OpaqueMaterial; } }
        protected virtual Material TransparentMaterial { get { return SceneGraph.Current?.InternalResources?.TransparentMultipassMaterial; } }

        protected override void Awake() {
            base.Awake();

            RegisterPropertyDelegate(SIZE_PROPERTY_NAME, OnSizePropertyChanged);
            RegisterPropertyDelegate(PAINT_PROPERTY_NAME, OnPaintPropertyChanged);
            RegisterPropertyDelegate(OPACITY_PROPERTY_NAME, OnOpacityPropertyChanged);
        }

        public UnityEngine.Vector3 GetSize(bool inDynamic) {
            UnityEngine.Vector3 size = inDynamic ? GetMeshBounds().size : m_CachedMeshBounds.size;
            size.Scale(m_ModelTransform.localScale);
            return size;
        }

        public Bounds GetBounds(bool inDynamic) {

            Bounds bounds = inDynamic ? GetMeshBounds() : m_CachedMeshBounds;
            var scale = m_ModelTransform.localScale;

            var center = bounds.center;
            center.Scale(scale);
            var size = bounds.size;
            size.Scale(scale);

            bounds.center = center + m_ModelTransform.localPosition;
            bounds.size = size;

            return bounds;
        }

        protected void CacheMeshBounds() {
            m_CachedMeshBounds = GetMeshBounds();
        }

        protected abstract Bounds GetMeshBounds();

        private void OnSizePropertyChanged(TValue inValue) {
            SetSize(inValue.RawStruct<Size>());
        }

        protected virtual void SetSize(UnityEngine.Vector3 inSize) {
            var meshSize = m_CachedMeshBounds.size;
            m_ModelTransform.localScale = new UnityEngine.Vector3(
                meshSize.x == 0 ? 1 : inSize.x/meshSize.x,
                meshSize.y == 0 ? 1 : inSize.y/meshSize.y,
                meshSize.z == 0 ? 1 : inSize.z/meshSize.z
            );
        }

        

        private void OnPaintPropertyChanged(TValue inValue) {
            m_CachedPaint = inValue.RawObject<Paint>();  
            OnPaintChanged();
        }

        protected abstract void OnPaintChanged();

        protected void ApplyPaint(Renderer inRenderer, ref MaterialPropertyBlock inPropertyBlock) {
            PrepPropertyBlock(inRenderer, ref inPropertyBlock);

            m_CachedPaint.Apply(inPropertyBlock, m_CachedOpacity, PaintTextureName);
            inRenderer.SetPropertyBlock(inPropertyBlock);
        }

        private void OnOpacityPropertyChanged(TValue inValue) {
            m_CachedOpacity = (float)inValue.RawStruct<Portion>().Value;
            OnOpacityChanged();
        }

        protected abstract void OnOpacityChanged();

        protected void ApplyOpacity(Renderer inRenderer, ref MaterialPropertyBlock inPropertyBlock) {
             if (m_CachedOpacity < 0.004f) {
                if (inRenderer.enabled) {
                    inRenderer.enabled = false;
                }
                return;
            } else if (!inRenderer.enabled) {
                inRenderer.enabled = true;
            }

            if (m_CachedOpacity < 0.996f && inRenderer.sharedMaterial != TransparentMaterial) {
                inRenderer.sharedMaterial = TransparentMaterial;
            } else if (m_CachedOpacity >= 0.996f && inRenderer.sharedMaterial != OpaqueMaterial) {
                inRenderer.sharedMaterial = OpaqueMaterial;
            }

            PrepPropertyBlock(inRenderer, ref inPropertyBlock);

            m_CachedPaint.Apply(inPropertyBlock, m_CachedOpacity, PaintTextureName);
            inRenderer.SetPropertyBlock(inPropertyBlock);
        }

        public override void CleanUp() {}

    }
}