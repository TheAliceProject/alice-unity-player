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

        protected Renderer m_Renderer;
        protected MeshFilter m_MeshFilter;
        protected Transform m_ModelTransform;
        protected Bounds m_MeshBounds;
        protected MaterialPropertyBlock m_PropertyBlock;
        private Paint m_CachedPaint = Primitives.Color.WHITE;
        protected float m_CachedOpacity = 1;

        protected override void Awake() {
            base.Awake();

            RegisterPropertyDelegate(SIZE_PROPERTY_NAME, OnSizePropertyChanged);
            RegisterPropertyDelegate(PAINT_PROPERTY_NAME, OnPaintPropertyChanged);
            RegisterPropertyDelegate(OPACITY_PROPERTY_NAME, OnOpacityPropertyChanged);
        }

        public UnityEngine.Vector3 GetSize(bool inDynamic) {
            UnityEngine.Vector3 size;
            if (inDynamic && m_Renderer is SkinnedMeshRenderer) {
                var skinnedRenderer = (SkinnedMeshRenderer)m_Renderer;
                size = skinnedRenderer.localBounds.size;
            } else {
                size = m_MeshBounds.size;
            }

            size.Scale(m_ModelTransform.localScale);
            return size;
        }

        public Bounds GetBounds(bool inDynamic) {
            
            Bounds bounds;
            if (inDynamic && m_Renderer is SkinnedMeshRenderer) {
                var skinnedRenderer = (SkinnedMeshRenderer)m_Renderer;
                bounds = skinnedRenderer.localBounds;
            } else {
                bounds = m_MeshBounds;
            }

            var scale = m_ModelTransform.localScale;

            var center = bounds.center;
            center.Scale(scale);
            var size = bounds.size;
            size.Scale(scale);

            bounds.center = center + m_ModelTransform.localPosition;
            bounds.size = size;

            return bounds;
        }

        public virtual void CacheMeshBounds() {
            if (m_Renderer is SkinnedMeshRenderer) {
                var skinnedRenderer = (SkinnedMeshRenderer)m_Renderer;
                // make sure the skinned mesh renderers local bounds get updated
                skinnedRenderer.updateWhenOffscreen = true;
                m_MeshBounds = skinnedRenderer.localBounds;
            } else  {
                m_MeshBounds = m_MeshFilter.sharedMesh.bounds;
            }
        }

        private void OnSizePropertyChanged(TValue inValue) {
            SetSize(inValue.RawStruct<Size>());
        }

        protected virtual void SetSize(UnityEngine.Vector3 inSize) {
            var meshSize = m_MeshBounds.size;
            m_ModelTransform.localScale = new UnityEngine.Vector3(
                meshSize.x == 0 ? 1 : inSize.x/meshSize.x,
                meshSize.y == 0 ? 1 : inSize.y/meshSize.y,
                meshSize.z == 0 ? 1 : inSize.z/meshSize.z
            );
        }

        protected virtual string PaintTextureName { get { return FILTER_TEXTURE_SHADER_NAME; } }
        protected virtual Material OpaqueMaterial { get { return SceneGraph.Current?.InternalResources?.OpaqueMaterial; } }
        protected virtual Material TransparentMaterial { get { return SceneGraph.Current?.InternalResources?.TransparentMaterial; } }

        private void OnPaintPropertyChanged(TValue inValue) {
            m_CachedPaint = inValue.RawObject<Paint>();

            PrepPropertyBlock(m_Renderer, ref m_PropertyBlock);

            m_CachedPaint.Apply(m_PropertyBlock, m_CachedOpacity, PaintTextureName);
            m_Renderer.SetPropertyBlock(m_PropertyBlock);
        }

        private void OnOpacityPropertyChanged(TValue inValue) {
            SetOpacity((float)inValue.RawStruct<Portion>().Value);
        }

        protected virtual void SetOpacity(float inOpacity) {
            m_CachedOpacity = inOpacity;
            
            if (m_CachedOpacity < 0.004f) {
                if (m_Renderer.enabled) {
                    m_Renderer.enabled = false;
                }
                return;
            } else if (!m_Renderer.enabled) {
                m_Renderer.enabled = true;
            }

            if (m_CachedOpacity < 0.996f && m_Renderer.sharedMaterial != TransparentMaterial) {
                m_Renderer.sharedMaterial = TransparentMaterial;
            } else if (m_CachedOpacity >= 0.996f && m_Renderer.sharedMaterial != OpaqueMaterial) {
                m_Renderer.sharedMaterial = OpaqueMaterial;
            }

            PrepPropertyBlock(m_Renderer, ref m_PropertyBlock);

            m_CachedPaint.Apply(m_PropertyBlock, m_CachedOpacity, PaintTextureName);
            m_Renderer.SetPropertyBlock(m_PropertyBlock);
        }

        public override void CleanUp() {}

    }
}