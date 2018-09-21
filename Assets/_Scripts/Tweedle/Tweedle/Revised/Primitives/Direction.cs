using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Tweedle.Primitives
{


    [PInteropType]
    public sealed class Direction
    {
        public readonly Vector3 value = Vector3.Zero;

        public Direction(Vector3 inVector)
        {
            value = inVector;
        }

        #region Interop Interfaces
        [PInteropField]
        public static readonly Direction NaV = new Direction(double.NaN, double.NaN, double.NaN);
        [PInteropField]
        public static readonly Direction POSITIVE_X_AXIS  = new Direction(1, 0, 0);
        [PInteropField]
        public static readonly Direction NEGATIVE_X_AXIS  = new Direction(-1, 0, 0);
        [PInteropField]
        public static readonly Direction POSITIVE_Y_AXIS  = new Direction(0, 1, 0);
        [PInteropField]
        public static readonly Direction NEGATIVE_Y_AXIS  = new Direction(0, -1, 0);
        [PInteropField]
        public static readonly Direction POSITIVE_Z_AXIS  = new Direction(0, 0, 1);
        [PInteropField]
        public static readonly Direction NEGATIVE_Z_AXIS  = new Direction(0, 0, -1);

        [PInteropField]
        public double x { get { return value.X; } }
        [PInteropField]
        public double y { get { return value.Y; } }
        [PInteropField]
        public double z { get { return value.Z; } }

        [PInteropConstructor]
        public Direction(double x, double y, double z)
        {
            value = new Vector3(x, y, z);
        }

        [PInteropConstructor]
        public Direction(Direction clone)
        {
            value = clone.value;
        }
       
        [PInteropMethod]
        public bool equals(Direction other) 
        {
            return value == other.value;
        }

        [PInteropMethod]
        public Direction add(Direction other) 
        {
            return new Direction(value + other.value);
        }

        [PInteropMethod]
        public Direction negated() 
        {
            return new Direction(-value);
        }

        [PInteropMethod]
        public Direction scaledBy(double factor) 
        {
            return new Direction(value * factor);
        }

        [PInteropMethod]
        public Direction normalized() 
        {
            return new Direction(Vector3.Normalize(value));
        }

        public double dotProduct(Direction other) {
            return Vector3.Dot(value, other.value);
        }

        public Direction crossProduct(Direction other) {
            return new Direction(Vector3.Cross(value, other.value));
        }

        [PInteropMethod]
        public Direction interpolatePortion(Direction end, double portion) {
            return new Direction(Vector3.Lerp(value, end.value, portion));      
        }

        [PInteropMethod]
        public bool isPositiveXAxis() {
            return value == POSITIVE_X_AXIS.value;
        }

        [PInteropMethod]
        public bool isNegativeXAxis() {
            return value == NEGATIVE_X_AXIS.value;
        }

        [PInteropMethod]
        public bool isPositiveYAxis() {
            return value == POSITIVE_Y_AXIS.value;
        }

        [PInteropMethod]
        public bool isNegativeYAxis() {
            return value == NEGATIVE_Y_AXIS.value;
        }

        [PInteropMethod]
        public bool isPositiveZAxis() {
            return value == POSITIVE_Z_AXIS.value;
        }

        [PInteropMethod]
        public bool isNegativeZAxis() {
            return value == NEGATIVE_Z_AXIS.value;
        }

        #endregion //Interop Interfaces

        public static Direction lerp(Direction a, Direction b, Portion t) {
            return new Direction(Vector3.Lerp(a.value, b.value, t.value));
        }

        public Direction transform(VantagePoint vantagePoint) {
            return new Direction(Vector3.TransformNormal(value, vantagePoint.value));
        }

        public Direction rotate(Orientation orientation) {
            return new Direction(Vector3.Transform(value, orientation.value));
        }

        static public implicit operator UnityEngine.Vector3(Direction inDirection)
        {
            return inDirection != null ? new UnityEngine.Vector3((float)inDirection.value.X, (float)inDirection.value.Y, (float)inDirection.value.Z) : new UnityEngine.Vector3(float.NaN, float.NaN, float.NaN);
        }


        public override string ToString() {
            return string.Format("Direction({0:0.##},{1:0.##},{2:0.##})", value.X, value.Y, value.Z);
        }

    }
}