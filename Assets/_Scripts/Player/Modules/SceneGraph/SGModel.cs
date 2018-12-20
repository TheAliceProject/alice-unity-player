using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System;
using System.Collections.Generic;

namespace Alice.Player.Unity {
    
    public abstract class SGModel : SGEntity {

        public const string TRANSFORMATION_PROPERTY_NAME = "Transform";
        public const string PAINT_PROPERTY_NAME = "Paint";
        public const string SIZE_PROPERTY_NAME = "Size";
        public const string OPACITY_PROPERTY_NAME = "Opacity";

        public const string MAIN_TEXTURE_SHADER_NAME = "_MainTexture";
        public const string FILTER_TEXTURE_SHADER_NAME = "_FilterTexture";
        public const string COLOR_SHADER_NAME = "_Color";

        protected Renderer m_Renderer;
        protected Transform m_ModelTransform;
        private MaterialPropertyBlock m_PropertyBlock;
        private Paint m_CachedPaint;
        private float m_CachedOpacity = 1;
        private AxisAlignedBox m_MeshBounds;

        protected virtual void Init(Transform inModelTransform, Renderer inRenderer) {
            m_ModelTransform = inModelTransform;
            m_ModelTransform.SetParent(transform, false);
            m_ModelTransform.localPosition = UnityEngine.Vector3.zero;
            m_ModelTransform.localRotation = UnityEngine.Quaternion.identity;
            m_Renderer = inRenderer;
            m_Renderer.sharedMaterial = SceneGraph.Current.InternalResources.OpaqueMaterial;

            CacheMeshBounds();

            RegisterPropertyDelegate(TRANSFORMATION_PROPERTY_NAME, OnTransformationPropertyChanged);
            RegisterPropertyDelegate(SIZE_PROPERTY_NAME, OnSizePropertyChanged);
            RegisterPropertyDelegate(PAINT_PROPERTY_NAME, OnPaintPropertyChanged);
            RegisterPropertyDelegate(OPACITY_PROPERTY_NAME, OnOpacityPropertyChanged);
        }

        public AxisAlignedBox GetBounds(bool inDynamic) {
            var scale = m_ModelTransform.localScale;

            if (inDynamic && m_Renderer is SkinnedMeshRenderer) {
                var skinnedRenderer = (SkinnedMeshRenderer)m_Renderer;
                skinnedRenderer.updateWhenOffscreen = true;
                var bounds = skinnedRenderer.localBounds;
                var center = bounds.center;
                center.Scale(scale);
                var size = bounds.size;
                size.Scale(scale);

                bounds.center = center;
                bounds.size = size;
                return bounds;
            }

            return new AxisAlignedBox(
                new Primitives.Vector3(m_MeshBounds.MinValue.X*scale.x, m_MeshBounds.MinValue.Y*scale.y, m_MeshBounds.MinValue.Z*scale.z),
                new Primitives.Vector3(m_MeshBounds.MaxValue.X*scale.x, m_MeshBounds.MaxValue.Y*scale.y, m_MeshBounds.MaxValue.Z*scale.z)
            );
        }

        public void CacheMeshBounds() {
            if (m_Renderer is SkinnedMeshRenderer) {
                var skinnedRenderer = (SkinnedMeshRenderer)m_Renderer;
                // make sure the skinned mesh renderers local bounds get updated
                skinnedRenderer.updateWhenOffscreen = true;
                m_MeshBounds = skinnedRenderer.localBounds;
            } else  {
                var filter = m_Renderer.GetComponent<MeshFilter>();
                m_MeshBounds = filter.mesh.bounds;
            }
        }

        private void OnTransformationPropertyChanged(TValue inValue) {
            VantagePoint vp = inValue.RawStruct<VantagePoint>();
            cachedTransform.localPosition = vp.position;
            cachedTransform.localRotation = vp.orientation;
        }

        protected abstract void OnSizePropertyChanged(TValue inValue);

        protected virtual string shaderTextureName { get { return FILTER_TEXTURE_SHADER_NAME; } }

        private void OnPaintPropertyChanged(TValue inValue) {
            m_CachedPaint = inValue.RawObject<Paint>();

            PrepPropertyBlock();

            m_CachedPaint.Apply(m_PropertyBlock, m_CachedOpacity, shaderTextureName);

            m_Renderer.SetPropertyBlock(m_PropertyBlock);
        }

        private void OnOpacityPropertyChanged(TValue inValue) {
            m_CachedOpacity = (float)inValue.RawStruct<Portion>().Value;
            
            if (m_CachedOpacity < 0.004f) {
                if (m_Renderer.enabled) {
                    m_Renderer.enabled = false;
                }
                return;
            } else if (!m_Renderer.enabled) {
                m_Renderer.enabled = true;
            }

            if (m_CachedOpacity < 0.996f && m_Renderer.sharedMaterial != SceneGraph.Current.InternalResources.TransparentMaterial) {
                m_Renderer.sharedMaterial = SceneGraph.Current.InternalResources.TransparentMaterial;
            } else if (m_CachedOpacity >= 0.996f && m_Renderer.sharedMaterial != SceneGraph.Current.InternalResources.OpaqueMaterial) {
                m_Renderer.sharedMaterial = SceneGraph.Current.InternalResources.OpaqueMaterial;
            }

            PrepPropertyBlock();

            if (m_CachedPaint != null) {
                m_CachedPaint.Apply(m_PropertyBlock, m_CachedOpacity, shaderTextureName);
            } else {
                var color = new UnityEngine.Color(1,1,1, m_CachedOpacity);
                m_PropertyBlock.SetColor(COLOR_SHADER_NAME, color);
            }

             m_Renderer.SetPropertyBlock(m_PropertyBlock);


        }

        private void PrepPropertyBlock() {
            if (m_PropertyBlock == null) {
                m_PropertyBlock = new MaterialPropertyBlock();
            }

            if (m_Renderer.HasPropertyBlock()) {
                m_Renderer.GetPropertyBlock(m_PropertyBlock);
            }
        }

    }
}