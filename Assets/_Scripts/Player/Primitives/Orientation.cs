using System.Diagnostics;
using Alice.Tweedle.Interop;
using System;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public sealed class Orientation
    {
        public readonly Quaternion Value = Quaternion.Identity;

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
            return inOrientation != null ? new UnityEngine.Quaternion((float)inOrientation.Value.X, (float)inOrientation.Value.Y, (float)inOrientation.Value.Z, (float)inOrientation.Value.W) : new UnityEngine.Quaternion(float.NaN, float.NaN, float.NaN, float.NaN);
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