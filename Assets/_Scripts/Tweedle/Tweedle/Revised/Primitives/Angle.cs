using System.Diagnostics;
using Alice.Tweedle.Interop;
using System;

namespace Alice.Tweedle.Primitives
{
    [PInteropType]
    public sealed class Angle
    {
        public readonly double value;

        #region Interop Interfaces
        [PInteropField]
        public double radians 
        { 
            get {
                return value; 
            }
        }

        [PInteropField]
        public double degrees 
        { 
            get {
                const double rad2Deg = 180/System.Math.PI;
                return rad2Deg*value; 
            }
        }

        [PInteropField]
        public double revolutions 
        {
            get {
                const double rad2Rev = 1/(System.Math.PI*2);
                return value*rad2Rev;
            }
        }

        [PInteropConstructor]
        public Angle(double revolutions)
        {
            const double rev2rad = System.Math.PI*2;
            this.value = revolutions*rev2rad;
        }

        [PInteropConstructor]
        public Angle(Angle clone)
        {
            value = clone.value;
        }

        [PInteropMethod]
        public bool equals(Angle other) 
        {
            return this.value == other.value;
        }

        [PInteropMethod]
        public Angle add(Angle other) 
        {
            return new Angle(value + other.value);
        }

        [PInteropMethod]
        public Angle subtract(Angle other)
        {
            return new Angle(value - other.value);
        }

        [PInteropMethod]
        public Angle scaledBy(double factor) 
        {
            return new Angle(value * factor);
        }

        [PInteropMethod]
        public Angle dividedBy(double divisor)
        {
            return new Angle(value / divisor);
        }

        [PInteropMethod]
        public Angle interpolatePortion(Angle end, double portion) {
            return new Angle((end.value - value)*portion + value);
        }
        #endregion // Interop Interfaces

        static public implicit operator double(Angle inAngle)
        {
            return inAngle != null ? inAngle.value : double.NaN;
        }

        public override string ToString() {
            return string.Format("Angle({0:0.##})", value);
        }
    }
}