using System.Diagnostics;
using Alice.Tweedle.Interop;
using Alice.Player.Modules;
using System;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public struct Angle
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
                return MathModule.RAD2DEG*Value; 
            }
        }

        [PInteropField]
        public double revolutions 
        {
            get {
                return Value*MathModule.RAD2REV;
            }
        }

        [PInteropConstructor]
        public Angle(double revolutions)
        {
            this.Value = revolutions*MathModule.REV2RAD;
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
            return inAngle.Value;
        }

        static public implicit operator float(Angle inAngle)
        {
            return (float)inAngle.Value;
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