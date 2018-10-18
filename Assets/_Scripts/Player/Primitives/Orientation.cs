using System.Diagnostics;
using Alice.Tweedle.Interop;
using System;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public struct Orientation
    {
        public readonly Quaternion Value;

        public Orientation(Quaternion inRotation)
        {
            Value = inRotation;
        }

        #region  Interop Interfaces
        [PInteropField]
        public static readonly Orientation IDENTITY = new Orientation(Quaternion.Identity);

        [PInteropConstructor]
        public Orientation(double x, double y, double z, double w)
        {
            Value = new Quaternion(x, y, z, w);
        }

        [PInteropConstructor]
        public Orientation(Direction axis, Angle angle) {
            Value = Quaternion.CreateFromAxisAngle(axis.Value, angle.radians);
        }

        [PInteropConstructor]
        public Orientation(Direction forward, Direction up) {
            Value = Quaternion.CreateFromLookVectors(forward.Value, up.Value);
        }

        [PInteropConstructor]
        public Orientation(Direction forward) {
            Vector3 up;
            if( ( forward.Value.X == 0 ) && ( forward.Value.Z == 0 ) ) {
                up = Vector3.UnitX;
            } else {
                up = Vector3.UnitY;
            }
            Value = Quaternion.CreateFromLookVectors(forward.Value, up);
        }

        [PInteropMethod]
        public bool equals(Orientation other) {
            return Value.Equals(other.Value);
        }

        [PInteropMethod]
        public bool isIdentity() {
            return Value.IsIdentity;
        }

        [PInteropMethod]
        public Orientation multiply(Orientation other) {
            return new Orientation(Value * other.Value);
        }

        [PInteropMethod]
        public Orientation inverse() {
            return new Orientation(Quaternion.Inverse(Value));
        }

        [PInteropMethod]
        public Orientation interpolatePortion(Orientation end, Portion portion) {
            return new Orientation(Quaternion.Slerp(Value, end.Value, portion.Value));
        }

        #endregion // Interop Interfaces
        
        static public implicit operator UnityEngine.Quaternion(Orientation inOrientation)
        {
            return new UnityEngine.Quaternion((float)inOrientation.Value.X, (float)inOrientation.Value.Y, (float)inOrientation.Value.Z, (float)inOrientation.Value.W);
        }

        public override string ToString() {
            return string.Format("Orientation({0:0.##},{1:0.##},{2:0.##},{3:0.##})", Value.X, Value.Y, Value.Z, Value.W);
        }

        public override bool Equals(object obj) {
            if (obj is Orientation) {
                return equals((Orientation)obj);
            }
            return false;
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }
    }
}