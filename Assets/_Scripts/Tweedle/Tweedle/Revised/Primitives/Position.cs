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

        [PInteropConstructor]
        public Position(double right, double up, double backward)
        {
            value = new Vector3(right, up, backward);
        }

        [PInteropConstructor]
        public Position(Position clone)
        {
            value = clone.value;
        }


        [PInteropMethod]
        public double getRight() {
	        return value.X;
	    }

        [PInteropMethod]
        public double getUp() {
            return value.Y;
        }

        [PInteropMethod]
        public double getBackward() {
            return value.Z;
        }

        [PInteropMethod]
        public Position move(Direction direction) {
            return new Position(value + direction.value);
        }
 
        public Position transform(VantagePoint vantagePoint) {
            return new Position(Vector3.Transform(value, vantagePoint.value));
        }

        public Position rotate(Orientation orientation) {
            return new Position(Vector3.Transform(value, orientation.value));
        }

        public static Position lerp(Position a, Position b, Portion t) {
            return new Position(Vector3.Lerp(a.value, b.value, t.value));
        }
        
        static public implicit operator UnityEngine.Vector3(Position inPosition)
        {
            return inPosition != null ? new UnityEngine.Vector3((float)inPosition.value.X, (float)inPosition.value.Y, (float)inPosition.value.Z) : new UnityEngine.Vector3(float.NaN, float.NaN, float.NaN);
        }

    }
}