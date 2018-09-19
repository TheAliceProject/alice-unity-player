using System.Diagnostics;
using Alice.Tweedle.Interop;
using System;

namespace Alice.Tweedle.Primitives
{
    [PInteropType]
    public sealed class Angle
    {

        [PInteropField]
        public readonly double radians;

        [PInteropField]
        public double degrees 
        { 
            get {
                const double rad2Deg = 180/System.Math.PI;
                return rad2Deg*radians; 
            }
        }

        [PInteropField]
        public double revolutions 
        {
            get {
                const double rad2Rev = 1/(System.Math.PI*2);
                return radians*rad2Rev;
            }
        }

        [PInteropConstructor]
        public Angle(double revolutions)
        {
            const double rev2rad = System.Math.PI*2;
            this.radians = revolutions*rev2rad;
        }

        [PInteropConstructor]
        public Angle(Angle clone)
        {
            radians = clone.radians;
        }

        [PInteropMethod]
        public bool equals(Angle angle) 
        {
            return this.radians == angle.radians;
        }

        [PInteropMethod]
        public Angle plus(Angle angle) 
        {
            return new Angle(radians + angle.radians);
        }

        [PInteropMethod]
        public Angle minus(Angle angle)
        {
            return new Angle(radians - angle.radians);
        }

        [PInteropMethod]
        public Angle times(double factor) 
        {
            return new Angle(radians * factor);
        }

        [PInteropMethod]
        public Angle dividedBy(double divisor)
        {
            return new Angle(radians / divisor);
        }       

        // TODO: Talk to Daniel if this should use absolute values or account for wrapping
        public static Angle lerp(Angle a, Angle b, Portion t) {
            return new Angle(a.radians + (b.radians - a.radians)*t.value);
        }

        static public implicit operator double(Angle inAngle)
        {
            return inAngle != null ? inAngle.radians : double.NaN;
        }

         
    }
}