using System.Globalization;
using System.Runtime.CompilerServices;
using System;
 
namespace Alice.Player.Primitives
{
    /// <summary>
    /// A structure encapsulating a color with red, green, blue, and alpha components
    /// </summary>
    public struct Color4 : IEquatable<Color4>
    {
        public double R;
        public double G;
        public double B;
        public double A;

        public Color4(double r, double g, double b, double a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color4(double r, double g, double b)
        {
            R = r;
            G = g;
            B = b;
            A = 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color4 Lerp(Color4 value1, Color4 value2, double amount)
        {
            return new Color4(
                value1.R + (value2.R - value1.R) * amount,
                value1.G + (value2.G - value1.G) * amount,
                value1.B + (value2.B - value1.B) * amount,
                value1.A + (value2.A - value1.A) * amount);
            
        }

        public static bool operator ==(Color4 value1, Color4 value2)
        {
            return (value1.R == value2.R &&
                    value1.G == value2.G &&
                    value1.B == value2.B &&
                    value1.A == value2.A);
        }
 
        public static bool operator !=(Color4 value1, Color4 value2)
        {
            return (value1.R != value2.R ||
                    value1.G != value2.G ||
                    value1.B != value2.B ||
                    value1.A != value2.A);
        }

        public bool Equals(Color4 other)
        {
            return (R == other.R &&
                    G == other.G &&
                    B == other.B &&
                    A == other.A);
        }

        public override bool Equals(object obj)
        {
            if (obj is Color4)
            {
                return Equals((Color4)obj);
            }
 
            return false;
        }

        public override string ToString()
        {
            CultureInfo ci = CultureInfo.CurrentCulture;
 
            return String.Format(ci, "{{R:{0} G:{1} B:{2} A:{3}}}", R.ToString(ci), G.ToString(ci), B.ToString(ci), A.ToString(ci));
        }

        public override int GetHashCode()
        {
            return R.GetHashCode() + G.GetHashCode() + B.GetHashCode() + A.GetHashCode();
        }
    }

}