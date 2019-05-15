using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Player.Primitives
{


    [PInteropType]
    public class Direction
    {
        public readonly Vector3 Value;

        public Direction(Vector3 inVector)
        {
            Value = inVector;
        }

        #region Interop Interfaces
        [PInteropField]
        public static readonly Direction NaV = new Direction(double.NaN, double.NaN, double.NaN);
        [PInteropField]
        public static readonly Direction RIGHT = new Direction(1, 0, 0);
        [PInteropField]
        public static readonly Direction LEFT = new Direction(-1, 0, 0);
        [PInteropField]
        public static readonly Direction UP = new Direction(0, 1, 0);
        [PInteropField]
        public static readonly Direction DOWN = new Direction(0, -1, 0);
        [PInteropField]
        public static readonly Direction FORWARD = new Direction(0, 0, -1);
        [PInteropField]
        public static readonly Direction BACKWARD = new Direction(0, 0, 1);

        [PInteropField]
        public double x { get { return Value.X; } }
        [PInteropField]
        public double y { get { return Value.Y; } }
        [PInteropField]
        public double z { get { return Value.Z; } }

        [PInteropField]
        public double length { get { return Value.Length(); } }

        [PInteropField]
        public double lengthSquared { get { return Value.LengthSquared(); } }

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
        public Direction negate() 
        {
            return new Direction(-Value);
        }

        [PInteropMethod]
        public Direction scaledBy(double factor) 
        {
            return new Direction(Value * factor);
        }

        [PInteropMethod]
        public Direction normalize() 
        {
            return new Direction(Vector3.Normalize(Value));
        }

        [PInteropMethod]
        public double dotProduct(Direction other) {
            return Vector3.Dot(Value, other.Value);
        }

        [PInteropMethod]
        public Direction crossProduct(Direction other) {
            return new Direction(Vector3.Cross(Value, other.Value));
        }

        [PInteropMethod]
        public Direction project(Direction other) {
            var proj = (Vector3.Dot(Value, other.Value)/Vector3.Dot(other.Value, other.Value))*other.Value;
            return new Direction(proj);
        }

        [PInteropMethod]
        public Direction transform(VantagePoint vantagePoint) {
            return new Direction(Vector3.TransformNormal(Value, vantagePoint.GetMatrix()));
        }

        [PInteropMethod]
        public Direction rotate(Orientation orientation) {
            return new Direction(Vector3.Transform(Value, orientation.Value));
        }

        [PInteropMethod]
        public Direction interpolatePortion(Direction end, Portion portion) {
            return new Direction(Vector3.Lerp(Value, end.Value, portion.Value));
        }

        [PInteropMethod]
        public bool isRight() {
            return Value == RIGHT.Value;
        }

        [PInteropMethod]
        public bool isLeft() {
            return Value == LEFT.Value;
        }

        [PInteropMethod]
        public bool isUp() {
            return Value == UP.Value;
        }

        [PInteropMethod]
        public bool isDown() {
            return Value == DOWN.Value;
        }

        [PInteropMethod]
        public bool isForward() {
            return Value == FORWARD.Value;
        }

        [PInteropMethod]
        public bool isBackward() {
            return Value == BACKWARD.Value;
        }

        #endregion //Interop Interfaces

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