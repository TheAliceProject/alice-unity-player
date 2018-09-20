using System.Diagnostics;
using Alice.Tweedle.Interop;
using System;

namespace Alice.Tweedle.Primitives
{
    [PInteropType]
    public sealed class Orientation
    {
        public readonly Quaternion value = Quaternion.Identity;

        public Orientation(Quaternion inRotation)
        {
            value = inRotation;
        }

        #region  Interop Interfaces
        [PInteropField]
        public static readonly Orientation identity = new Orientation(Quaternion.Identity);

        [PInteropConstructor]
        public Orientation(double x, double y, double z, double w)
        {
            value = new Quaternion(x, y, z, w);
        }

        [PInteropConstructor]
        public Orientation(Direction direction, Angle angle) {
            value = Quaternion.CreateFromAxisAngle(direction.value, angle.radians);
        }

        [PInteropConstructor]
        public Orientation(Orientation clone)
        {
            value = clone.value;
        }

        [PInteropMethod]
        public bool equals(Orientation other) {
            return value.Equals(other.value);
        }

        [PInteropMethod]
        public bool isIdentity() {
            return value.IsIdentity;
        }

        [PInteropMethod]
        public Orientation multiply(Orientation other) {
            return new Orientation(value * other.value);
        }

        [PInteropMethod]
        public Orientation inverse() {
            return new Orientation(Quaternion.Inverse(value));
        }

        #endregion // Interop Interfaces

        public static Orientation slerp(Orientation a, Orientation b, Portion t) {
            return new Orientation(Quaternion.Slerp(a.value, b.value, t.value));
        }

        public static Orientation lerp(Orientation a, Orientation b, Portion t) {
            return new Orientation(Quaternion.Lerp(a.value, b.value, t.value));
        }

        static public implicit operator UnityEngine.Quaternion(Orientation inOrientation)
        {
            return inOrientation != null ? new UnityEngine.Quaternion((float)inOrientation.value.X, (float)inOrientation.value.Y, (float)inOrientation.value.Z, (float)inOrientation.value.W) : new UnityEngine.Quaternion(float.NaN, float.NaN, float.NaN, float.NaN);
        }

        public override string ToString() {
            return string.Format("Orientation({0:0.##},{1:0.##},{2:0.##},{3:0.##})", value.X, value.Y, value.Z, value.W);
        }

    }
}