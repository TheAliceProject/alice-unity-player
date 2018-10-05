using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System;
using System.Collections.Generic;

namespace Alice.Player.Modules {
    
    public abstract class SGModel : SGEntity {

        private Renderer m_Renderer;
        private MaterialPropertyBlock m_PropertyBlock;

        public const string PAINT_PROPERTY_NAME = "Paint";
        public const string SIZE_PROPERTY_NAME = "Size";
        public const string OPACITY_PROPERTY_NAME = "Opacity";

        public const string MAIN_TEXTURE_SHADER_NAME = "_MainTexture";
        public const string FILTER_TEXTURE_SHADER_NAME = "_FilterTexture";
        public const string COLOR_SHADER_NAME = "_Color";

        protected virtual void Init(Renderer inRenderer) {
            m_Renderer = inRenderer;

            RegisterPropertyDelegate<Size>(SIZE_PROPERTY_NAME, OnSizePropertyChanged);
            RegisterPropertyDelegate<Paint>(PAINT_PROPERTY_NAME, OnPaintPropertyChanged);
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