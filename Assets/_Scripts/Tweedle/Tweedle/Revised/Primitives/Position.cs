using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Tweedle.Primitives
{
    [PInteropType]
    public sealed class Position
    {
        public readonly Vector3 value;

        public Position(Vector3 inVector)
        {
            value = inVector;
        }

        #region Interop Interfaces
        [PInteropField]
        public static readonly Position NaV = new Position(double.NaN, double.NaN, double.NaN);
        [PInteropField]
        public static readonly Position ZERO = new Position(0, 0, 0);

        [PInteropField]
        public double x { get { return value.X; } }
        [PInteropField]
        public double y { get { return value.Y; } }
        [PInteropField]
        public double z { get { return value.Z; } }

        [PInteropConstructor]
        public Position(double x, double y, double z)
        {
            value = new Vector3(x, y, z);
        }

        [PInteropConstructor]
        public Position(Position clone)
        {
            value = clone.value;
        }

        [PInteropMethod]
        public Position add(Direction other) {
            return new Position(value + other.value);
        }
 
        [PInteropMethod]
        public Position subtract(Direction other) {
            return new Position(value + other.value);
        }

        [PInteropMethod]
        public Direction subtract(Position other) {
            return new Direction(value + other.value);
        }

        [PInteropMethod]
        public Position scaledBy(double factor) {
            return new Position(value*factor);
        }

        [PInteropMethod]
        public Position interpolatePortion(Position end, double portion) {
            return new Position(Vector3.Lerp(value, end.value, portion));
        }

        [PInteropMethod]
        public double distanceSquared(Position other) {
            return Vector3.DistanceSquared(value, other.value);
        }

        [PInteropMethod]
        public double distance(Position other) {
            return Vector3.Distance(value, other.value);
        }
        #endregion // Interop Interfaces

        public Position transform(VantagePoint vantagePoint) {
            return new Position(Vector3.Transform(value, vantagePoint.value));
        }

        public Position rotate(Orientation orientation) {
            return new Position(Vector3.Transform(value, orientation.value));
        }
       
        static public implicit operator UnityEngine.Vector3(Position inPosition)
        {
            return inPosition != null ? new UnityEngine.Vector3((float)inPosition.value.X, (float)inPosition.value.Y, (float)inPosition.value.Z) : new UnityEngine.Vector3(float.NaN, float.NaN, float.NaN);
        }

        public override string ToString() {
            return string.Format("Position({0:0.##},{1:0.##},{2:0.##})", value.X, value.Y, value.Z);
        }

    }
}