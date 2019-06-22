using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public class Position
    {
        public readonly Vector3 Value;

        public Position(Vector3 inVector)
        {
            Value = inVector;
        }

        #region Unity Conversion
        public static Position FromUnity(UnityEngine.Vector3 pos) {
            return new Position(-pos.x, pos.y, pos.z);
        }

        public UnityEngine.Vector3 UnityPosition() {
            return new UnityEngine.Vector3(-(float)Value.X, (float)Value.Y, (float)Value.Z);
        }
        #endregion // Unity Conversion

        #region Interop Interfaces
        [PInteropField]
        public static readonly Position NaV = new Position(double.NaN, double.NaN, double.NaN);
        [PInteropField]
        public static readonly Position ZERO = new Position(0, 0, 0);

        [PInteropField]
        public double x { get { return Value.X; } }
        [PInteropField]
        public double y { get { return Value.Y; } }
        [PInteropField]
        public double z { get { return Value.Z; } }

        [PInteropConstructor]
        public Position(double x, double y, double z)
        {
            Value = new Vector3(x, y, z);
        }

        [PInteropMethod]
        public bool equals(Position other) 
        {
            return Value == other.Value;
        }

        [PInteropMethod]
        public Position add(Direction other) {
            return new Position(Value + other.Value);
        }
 
        [PInteropMethod]
        public Position subtract(Direction other) {
            return new Position(Value - other.Value);
        }

        [PInteropMethod]
        public Direction directionFrom(Position other) {
            return new Direction(Value - other.Value);
        }

        [PInteropMethod]
        public Position scaledBy(double factor) {
            return new Position(Value*factor);
        }

        [PInteropMethod]
        public Position interpolatePortion(Position end, Portion portion) {
            return new Position(Vector3.Lerp(Value, end.Value, portion.Value));
        }

        [PInteropMethod]
        public double distanceSquared(Position other) {
            return Vector3.DistanceSquared(Value, other.Value);
        }

        [PInteropMethod]
        public double distance(Position other) {
            return Vector3.Distance(Value, other.Value);
        }
        #endregion // Interop Interfaces

        public Position Transform(VantagePoint vantagePoint) {
            return new Position(Vector3.Transform(Value, vantagePoint.GetMatrix()));
        }

        public Position Rotate(Orientation orientation) {
            return new Position(Vector3.Transform(Value, orientation.Value));
        }

        public override string ToString() {
            return string.Format("Position({0:0.##},{1:0.##},{2:0.##})", Value.X, Value.Y, Value.Z);
        }

        public override bool Equals(object obj) {
            if (obj is Position) {
                return equals((Position)obj);
            }
            return false;
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }

    }
}