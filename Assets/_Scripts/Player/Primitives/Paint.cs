using System.Diagnostics;
using Alice.Tweedle.Interop;
using System;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public abstract class Paint
    {   
        public enum PaintTypeID {
            Color,
            ImageSource
        }

        #region Interop Interfaces
        [PInteropField]
        public bool isImage { get { return PaintType == PaintTypeID.ImageSource; } }
        [PInteropField]
        public abstract double width { get; }
        [PInteropField]
        public abstract double height { get; }
        [PInteropMethod]
        public abstract bool equals(Paint other);
        [PInteropMethod]
        public abstract Paint interpolatePortion(Paint end, Portion portion);
        
        #endregion //Interop Interfaces
        
        public abstract PaintTypeID PaintType { get; }
        public abstract void Apply(UnityEngine.MaterialPropertyBlock inPropertyBlock, float inOpacity, string inTextureName);
        

    }
}