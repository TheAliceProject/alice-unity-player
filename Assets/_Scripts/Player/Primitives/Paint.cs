using System.Diagnostics;
using Alice.Tweedle.Interop;
using System;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public class Paint
    {
        public readonly Color4 ColorValue;
        public readonly UnityEngine.Texture TextureValue;

        public Paint(UnityEngine.Texture inTexture)
        {
            TextureValue = inTexture;
            ColorValue = new Color4(double.NaN,double.NaN,double.NaN,double.NaN);
        }

        public Paint(Color4 inColor) {
            ColorValue = inColor;
            TextureValue = null;
        }

        public Paint(double red, double green, double blue)
        {
            ColorValue = new Color4(red, green, blue, 1);
            TextureValue = null;
        }

        #region Interop Interfaces
        [PInteropField]
        public double red { get { return ColorValue.R; } }
        [PInteropField]
        public double green { get { return ColorValue.G; } }
        [PInteropField]
        public double blue { get { return ColorValue.B; } }
        [PInteropField]
        public double alpha { get { return ColorValue.A; } }

        [PInteropConstructor]
        public Paint(string resource)
        {
            // TODO: load and assign texture value
            TextureValue = UnityEngine.Texture2D.whiteTexture;
            ColorValue = new Color4(double.NaN,double.NaN,double.NaN,double.NaN);

        }

        [PInteropConstructor]
        public Paint(Paint clone)
        {
            ColorValue = clone.ColorValue;
            TextureValue = clone.TextureValue;
        }

        [PInteropMethod]
        public bool equals(Paint other) 
        {
            return (TextureValue != null && TextureValue == other.TextureValue) || ColorValue == other.ColorValue;
        }

        [PInteropMethod]
        public Paint interpolatePortion(Paint end, double portion) 
        {
            if (portion == 0) {
                return new Paint(this);
            }
            return TextureValue == null && end.TextureValue == null ? new Paint(Color4.Lerp(ColorValue, end.ColorValue, portion)) : new Paint (end);
        }
        #endregion // Interop Interfaces

        static public implicit operator UnityEngine.Color(Paint inPaint)
        {
            return inPaint != null && inPaint.TextureValue == null ? new UnityEngine.Color((float)inPaint.red, (float)inPaint.green, (float)inPaint.blue, (float)inPaint.alpha) : new UnityEngine.Color(float.NaN,float.NaN,float.NaN);
        }

        static public implicit operator UnityEngine.Texture(Paint inPaint)
        {
            return inPaint?.TextureValue;
        }

        public override string ToString() {
            return TextureValue != null ? string.Format("Paint({0})", TextureValue.name) : string.Format("Paint({0:0.##},{1:0.##},{2:0.##},{3:0.##})", red, green, blue, alpha);
        }

        public override bool Equals(object obj) 
        {
            if (obj is Paint) {
                return equals((Paint)obj);
            }
            return false;
        }

        public override int GetHashCode() 
        {
            return TextureValue != null ? TextureValue.GetHashCode() : ColorValue.GetHashCode();
        }
    }
}