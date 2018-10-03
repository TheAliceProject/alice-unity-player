using UnityEngine;
using Alice.Player.Modules;
using Alice.Player.Primitives;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System;
using System.Collections.Generic;

namespace Alice.Player.Modules {
    
    [PInteropType]
    public abstract class SGModel : SGEntity {

        private Renderer m_Renderer;
        private Material m_Material;
        
        #region Interop Interfaces
        [PInteropField]
        public const string PAINT_PROPERTY_NAME = "Paint";
        [PInteropField]
        public const string SIZE_PROPERTY_NAME = "Size";

        #endregion

        
        protected virtual void Init(Renderer inRenderer) {
            m_Renderer = inRenderer;
            m_Material = new Material(inRenderer.material);
            inRenderer.sharedMaterial = m_Material;

            RegisterPropertyDelegate<Size>(SIZE_PROPERTY_NAME, OnSizePropertyChanged);
            RegisterPropertyDelegate<Paint>(PAINT_PROPERTY_NAME, OnPaintPropertyChanged);
        }

        protected virtual void OnDestroy() {
            Destroy(m_Material);
        }

        protected abstract void OnSizePropertyChanged(PropertyBase<Size> inProperty);

        private void OnPaintPropertyChanged(PropertyBase<Paint> inProperty) {
            var paint = inProperty.getValue();
            paint.Apply(m_Material);
        }
    }
}