using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Player.Primitives
{


    [PInteropType]
    public sealed class Direction
    {
        public readonly Vector3 Value = Vector3.Zero;

        public Direction(Vector3 inVector)
        {
            Value = inVector;
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
        public double x { get { return Value.X; } }
        [PInteropField]
        public double y { get { return Value.Y; } }
        [PInteropField]
        public double z { get { return Value.Z; } }

        [PInteropConstructor]
        public Direction(double x, double y, double z)
        {
            Value = new Vector3(x, y, z);
        }

        [PInteropMethod]
        public bool equals(Direction other) 
        {
            return Value == other.Value;
        }

        [PInteropMethod]
        public Direction add(Direction other) 
        {
            return new Direction(Value + other.Value);
        }

        [PInteropMethod]
        public Direction negated() 
        {
            return new Direction(-Value);
        }

        [PInteropMethod]
        public Direction scaledBy(double factor) 
        {
            return new Direction(Value * factor);
        }

        [PInteropMethod]
        public Direction normalized() 
        {
            return new Direction(Vector3.Normalize(Value));
        }

        public double dotProduct(Direction other) {
            return Vector3.Dot(Value, other.Value);
        }

        public Direction crossProduct(Direction other) {
            return new Direction(Vector3.Cross(Value, other.Value));
        }

        [PInteropMethod]
        public Direction interpolatePortion(Direction end, double portion) {
            return new Direction(Vector3.Lerp(Value, end.Value, portion));      
        }

        [PInteropMethod]
        public bool isPositiveXAxis() {
            return Value == POSITIVE_X_AXIS.Value;
        }

        [PInteropMethod]
        public bool isNegativeXAxis() {
            return Value == NEGATIVE_X_AXIS.Value;
        }

        [PInteropMethod]
        public bool isPositiveYAxis() {
            return Value == POSITIVE_Y_AXIS.Value;
        }

        [PInteropMethod]
        public bool isNegativeYAxis() {
            return Value == NEGATIVE_Y_AXIS.Value;
        }

        [PInteropMethod]
        public bool isPositiveZAxis() {
            return Value == POSITIVE_Z_AXIS.Value;
        }

        [PInteropMethod]
        public bool isNegativeZAxis() {
            return Value == NEGATIVE_Z_AXIS.Value;
        }

        #endregion //Interop Interfaces

        public Direction Transform(VantagePoint vantagePoint) {
            return new Direction(Vector3.TransformNormal(Value, vantagePoint.Value));
        }

        public Direction Rotate(Orientation orientation) {
            return new Direction(Vector3.Transform(Value, orientation.Value));
        }

        static public implicit operator UnityEngine.Vector3(Direction inDirection)
        {
            return inDirection != null ? new UnityEngine.Vector3((float)inDirection.Value.X, (float)inDirection.Value.Y, (float)inDirection.Value.Z) : new UnityEngine.Vector3(float.NaN, float.NaN, float.NaN);
        }


        public override string ToString() {
            return string.Format("Direction({0:0.##},{1:0.##},{2:0.##})", Value.X, Value.Y, Value.Z);
        }

        public override bool Equals(object obj) {
            if (obj is Direction) {
                return equals((Direction)obj);
            }
            return false;
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }

    }
}