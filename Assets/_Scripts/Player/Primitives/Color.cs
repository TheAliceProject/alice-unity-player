using System.Diagnostics;
using Alice.Tweedle.Interop;
using System;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public sealed class Color : Paint
    {
        public Color(Color4 inColor) : base(inColor)
        {
        }

        #region Interop Interfaces
        [PInteropConstructor]
        public Color(double red, double green, double blue) : base(red, green, blue)
        {
        }


        [PInteropConstructor]
        public Color(Color clone) : base(clone)
        {
        }

        [PInteropMethod]
        public bool equals(Color other) 
        {
            return ColorValue == other.ColorValue;
        }

        [PInteropMethod]
        public Color interpolatePortion(Color end, double portion) 
        {
            return new Color(Color4.Lerp(ColorValue, end.ColorValue, portion));
        }
        #endregion // Interop Interfaces

        static public implicit operator UnityEngine.Color(Color inColor)
        {
            return inColor != null ? new UnityEngine.Color((float)inColor.red, (float)inColor.green, (float)inColor.blue, (float)inColor.alpha) : new UnityEngine.Color(float.NaN,float.NaN,float.NaN);
        }

        public override string ToString() {
            return string.Format("Color({0:0.##},{1:0.##},{2:0.##},{3:0.##})", red, green, blue, alpha);
        }

        public override bool Equals(object obj) {
            if (obj is Color) {
                return equals((Color)obj);
            }
            return false;
        }

        public override int GetHashCode() {
            return ColorValue.GetHashCode();
        }
    }
}