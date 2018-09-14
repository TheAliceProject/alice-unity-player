using System.Diagnostics;
using Alice.Tweedle.Interop;
using System;

namespace Alice.Tweedle.Primatives
{
    [PInteropType]
    public sealed class Orientation
    {
        public readonly Quaternion value = Quaternion.Identity;

        [PInteropConstructor]
        public Orientation(double x, double y, double z, double w)
        {
            value = new Quaternion(x, y, z, w);
        }

        public Orientation(Quaternion inRotation)
        {
            value = inRotation;
        }

        #region Interop Interfaces
        [PInteropConstructor]
        public Orientation() {}

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
        public bool equals(Orientation orientation) {
            return value.Equals(orientation.value);
        }
        #endregion

	    public bool isIdentity() {
            return value.IsIdentity;
        }

        public Orientation multiply(Orientation oriention) {
            return new Orientation(value * oriention.value);
        }

        public Orientation inverse() {
            return new Orientation(Quaternion.Inverse(value));
        }

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

    }
}