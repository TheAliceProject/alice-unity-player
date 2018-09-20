using System.Diagnostics;
using Alice.Tweedle.Interop;
using System;

namespace Alice.Tweedle.Primitives
{
    [PInteropType]
    public sealed class Angle
    {
        #region Interop Interfaces
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
        public bool equals(Angle other) 
        {
            return this.radians == other.radians;
        }

        [PInteropMethod]
        public Angle add(Angle other) 
        {
            return new Angle(radians + other.radians);
        }

        [PInteropMethod]
        public Angle subtract(Angle other)
        {
            return new Angle(radians - other.radians);
        }

        [PInteropMethod]
        public Angle scaledBy(double factor) 
        {
            return new Angle(radians * factor);
        }

        [PInteropMethod]
        public Angle dividedBy(double divisor)
        {
            return new Angle(radians / divisor);
        }

        [PInteropMethod]
        public Angle interpolatePortion(Angle end, double portion) {
            return new Angle(radians + (end.radians - radians)*portion);
        }
        #endregion // Interop Interfaces

        static public implicit operator double(Angle inAngle)
        {
            return inAngle != null ? inAngle.radians : double.NaN;
        }

        public override string ToString() {
            return string.Format("Angle({0:0.##})", radians);
        }
    }
}