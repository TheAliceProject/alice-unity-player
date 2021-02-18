using Alice.Player.Unity;
using Alice.Tweedle.Interop;
using Alice.Tweedle;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public sealed class Color : Paint
    {

        public readonly Color4 Value;

        public Color(Color4 inColor)
        {
            Value = inColor;
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

        [PInteropField]
        public double red { get { return Value.R; } }
        [PInteropField]
        public double green { get { return Value.G; } }
        [PInteropField]
        public double blue { get { return Value.B; } }
        [PInteropField]
        public double alpha { get { return Value.A; } }

        [PInteropConstructor]
        public Color(double red, double green, double blue) 
        {
            Value = new Color4(red, green, blue, 1);
        }

        [PInteropField]
        public override int width { get { return 0; } }

        [PInteropField]
        public override int height { get { return 0; } }

        [PInteropMethod]
        public override bool equals(Paint other) 
        {
            return Equals(other);
        }

        [PInteropMethod]
        public override Paint interpolatePortion(Paint end, Portion portion) 
        {   
            if (end.PaintType == PaintTypeID.Color) {
                return new Color(Color4.Lerp(Value, ((Color)end).Value, portion.Value));
            }

            if (end.PaintType == PaintTypeID.ImageSource) {
                return portion == 0 ? (Paint)this : end;
            }

            throw new TweedleRuntimeException("Could not interpolate paint type");

        }
        #endregion // Interop Interfaces

        public override PaintTypeID PaintType { get { return PaintTypeID.Color; } }

        public override void Apply(UnityEngine.MaterialPropertyBlock inPropertyBlock, float inOpacity, string inTextureName) {
            inPropertyBlock.SetColor(SGModel.COLOR_SHADER_NAME,
                new UnityEngine.Color((float)Value.R, (float)Value.G, (float)Value.B, (float)Value.A*inOpacity));
        }

        public override string ToString() {
            return string.Format("Color({0:0.##},{1:0.##},{2:0.##},{3:0.##})", Value.R, Value.G, Value.B, Value.A);
        }

        public override bool Equals(object obj) {
            if (obj is Color) {
                return ((Color)obj).Value == Value;
            }
            return false;
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }
    }
}