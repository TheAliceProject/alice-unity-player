using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System;
using System.Collections.Generic;

namespace Alice.Player.Unity {
    
    public abstract class SGModel : SGEntity {
        protected Renderer m_Renderer;
        protected Transform m_ModelTransform;
        private MaterialPropertyBlock m_PropertyBlock;

        public const string TRANSFORMATION_PROPERTY_NAME = "Transform";
        public const string PAINT_PROPERTY_NAME = "Paint";
        public const string SIZE_PROPERTY_NAME = "Size";
        public const string OPACITY_PROPERTY_NAME = "Opacity";

        public const string MAIN_TEXTURE_SHADER_NAME = "_MainTexture";
        public const string FILTER_TEXTURE_SHADER_NAME = "_FilterTexture";
        public const string COLOR_SHADER_NAME = "_Color";

        protected virtual void Init(Transform inModelTransform, Renderer inRenderer) {
            m_ModelTransform = inModelTransform;
            m_Renderer = inRenderer;

            RegisterPropertyDelegate(TRANSFORMATION_PROPERTY_NAME, OnTransformationPropertyChanged);
            RegisterPropertyDelegate(SIZE_PROPERTY_NAME, OnSizePropertyChanged);
            RegisterPropertyDelegate(PAINT_PROPERTY_NAME, OnPaintPropertyChanged);
        }

         private void OnTransformationPropertyChanged(TValue inValue) {
            VantagePoint vp = inValue.RawObject<VantagePoint>();
            cachedTransform.localPosition = vp.position;
            cachedTransform.localRotation = vp.orientation;
        }

        protected abstract void OnSizePropertyChanged(TValue inValue);

        protected virtual void OnPaintPropertyChanged(TValue inValue) {
            SetPaint(inValue, FILTER_TEXTURE_SHADER_NAME);
        }

        protected void SetPaint(TValue inValue, string inTextureName) {
            var paint = inValue.RawObject<Paint>();

            if (m_PropertyBlock == null) {
                m_PropertyBlock = new MaterialPropertyBlock();
            }

            if (m_Renderer.HasPropertyBlock()) {
                m_Renderer.GetPropertyBlock(m_PropertyBlock);
            }

            paint.Apply(m_PropertyBlock, inTextureName);

            m_Renderer.SetPropertyBlock(m_PropertyBlock);
        }

    }
}