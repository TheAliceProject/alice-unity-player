using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Tweedle;
using System;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public sealed class ImageSource : Paint
    {
        public readonly UnityEngine.Texture2D Value;

        public ImageSource(UnityEngine.Texture2D inTexture)
        {
            Value = inTexture;
        }

        #region Interop Interfaces
        [PInteropConstructor]
        public ImageSource(string resource)
        {
            Value = UnityEngine.Texture2D.whiteTexture;
        }

        [PInteropConstructor]
        public ImageSource(ImageSource clone)
        {
            // reference to same unity texture
            Value = clone.Value;
        }

        [PInteropMethod]
        public override bool equals(Paint other) 
        {
            return Equals(other);
        }

        [PInteropMethod]
        public override Paint interpolatePortion(Paint end, double portion) 
        {   
            if (end.PaintType == PaintTypeID.Color) {
                return portion == 0 ? (Paint)new ImageSource(this) : new Color((Color)end);
            }

            if (end.PaintType == PaintTypeID.ImageSource) {
                return new ImageSource(portion == 0 ? this : (ImageSource)end);
            }

            throw new TweedleRuntimeException("Could not interpolate paint type");

        }
        #endregion // Interop Interfaces

        public override PaintTypeID PaintType { get { return PaintTypeID.ImageSource; } }

        public override void Apply(UnityEngine.Material inMaterial) {
            inMaterial.mainTexture = Value;
            inMaterial.color = UnityEngine.Color.white;
        }

        public override string ToString() {
            return string.Format("ImageSource({0})", Value.name);
        }

        public override bool Equals(object obj) 
        {
            if (obj is ImageSource) {
                return ((ImageSource)obj).Value == Value;
            }
            return false;
        }

        public override int GetHashCode() 
        {
            return Value != null ? Value.GetHashCode() : base.GetHashCode();
        }
    }
}