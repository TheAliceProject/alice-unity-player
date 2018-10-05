using System.Diagnostics;
using Alice.Tweedle.Interop;
using System;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public sealed class Angle
    {
        public readonly double Value;

        #region Interop Interfaces
        [PInteropField]
        public double radians 
        { 
            get {
                return Value; 
            }
        }

        [PInteropField]
        public double degrees 
        { 
            get {
                const double rad2Deg = 180/System.Math.PI;
                return rad2Deg*Value; 
            }
        }

        [PInteropField]
        public double revolutions 
        {
            get {
                const double rad2Rev = 1/(System.Math.PI*2);
                return Value*rad2Rev;
            }
        }

        [PInteropConstructor]
        public Angle(double revolutions)
        {
            const double rev2rad = System.Math.PI*2;
            this.Value = revolutions*rev2rad;
        }

        [PInteropMethod]
        public bool equals(Angle other) 
        {
            return this.Value == other.Value;
        }

        [PInteropMethod]
        public Angle add(Angle other) 
        {
            return new Angle(Value + other.Value);
        }

        [PInteropMethod]
        public Angle subtract(Angle other)
        {
            return new Angle(Value - other.Value);
        }

        [PInteropMethod]
        public Angle scaledBy(double factor) 
        {
            return new Angle(Value * factor);
        }

        [PInteropMethod]
        public Angle dividedBy(double divisor)
        {
            return new Angle(Value / divisor);
        }

        [PInteropMethod]
        public Angle interpolatePortion(Angle end, Portion portion) {
            return new Angle((end.Value - Value)*portion.Value + Value);
        }
        #endregion // Interop Interfaces

        static public implicit operator double(Angle inAngle)
        {
            return inAngle != null ? inAngle.Value : double.NaN;
        }

        public override string ToString() {
            return string.Format("Angle({0:0.##})", Value);
        }

        public override bool Equals(object obj) {
            if (obj is Angle) {
                return equals((Angle)obj);
            }
            return false;
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }
    }
}