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
        [PInteropField]
        public static readonly Color BLACK = new Color( 0, 0, 0 );
        [PInteropField]
	    public static readonly Color BLUE = new Color( 0, 0, 1 );
        [PInteropField]
	    public static readonly Color CYAN = new Color( 0, 1, 1 );
        [PInteropField]
	    public static readonly Color DARK_GRAY = new Color( 64.0/255.0, 64.0/255.0, 64.0/255.0 );
        [PInteropField]
	    public static readonly Color GRAY = new Color( 128.0/255.0, 128.0/255.0, 128.0/255.0  );
        [PInteropField]
	    public static readonly Color GREEN = new Color( 0, 1, 0 );
        [PInteropField]
	    public static readonly Color LIGHT_GRAY = new Color( 192.0/255.0, 192.0/255.0, 192.0/255.0 );
        [PInteropField]
	    public static readonly Color MAGENTA = new Color( 1, 0, 1 );
        [PInteropField]
	    public static readonly Color ORANGE = new Color( 1, 200.0/255.0, 0 );
        [PInteropField]
    	public static readonly Color PINK = new Color( 1, 175.0/255.0, 175.0/255.0 );
        [PInteropField]
	    public static readonly Color RED = new Color( 1, 0, 0 );
        [PInteropField]
	    public static readonly Color WHITE = new Color( 1, 1, 1 );
        [PInteropField]
	    public static readonly Color YELLOW = new Color( 1, 1, 0 );
        [PInteropField]
	    public static readonly Color LIGHT_BLUE = new Color( 149.0 / 255.0, 166.0 / 255.0, 216.0 / 255.0 );
        [PInteropField]
	    public static readonly Color DARK_BLUE = new Color( 0 / 255.0, 0 / 255.0, 150.0 / 255.0 );
        [PInteropField]
	    public static readonly Color PURPLE = new Color( 128.0 / 255.0, 0.0, 128.0 / 255.0 );
	    [PInteropField]
        public static readonly Color BROWN = new Color( 162.0 / 255.0, 42.0 / 255.0, 42.0 / 255.0 );

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