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


        [PInteropConstructor]
        public Direction(double right, double up, double backward)
        {
            value = new Vector3(right, up, backward);
        }

        [PInteropConstructor]
        public Direction(Direction clone)
        {
            value = clone.value;
        }

        static public implicit operator UnityEngine.Vector3(Direction inDirection)
        {
            return inDirection != null ? new UnityEngine.Vector3((float)inDirection.value.X, (float)inDirection.value.Y, (float)inDirection.value.Z) : new UnityEngine.Vector3(float.NaN, float.NaN, float.NaN);
        }

        [PInteropMethod]
        public double getRight()
        {
	        return value.X;
	    }

        [PInteropMethod]
        public double getUp() 
        {
            return value.Y;
        }

        [PInteropMethod]
        public double getBackward() 
        {
            return value.Z;
        }

        [PInteropMethod]
        public bool equals(Direction direction) 
        {
            return value == direction.value;
        }

        [PInteropMethod]
        public Direction plus(Direction direction) 
        {
            return new Direction(value + direction.value);
        }

        [PInteropMethod]
        public Direction minus(Direction direction) 
        {
            return new Direction(value - direction.value);
        }

        [PInteropMethod]
        public Direction scale(double scale) 
        {
            return new Direction(value * scale);
        }

        public Direction normalized() 
        {
            return new Direction(Vector3.Normalize(value));
        }

        public static Direction lerp(Direction a, Direction b, Portion t) {
            return new Direction(Vector3.Lerp(a.value, b.value, t.value));
        }

        public Direction transform(VantagePoint vantagePoint) {
            return new Direction(Vector3.TransformNormal(value, vantagePoint.value));
        }

        public Direction rotate(Orientation orientation) {
            return new Direction(Vector3.Transform(value, orientation.value));
        }

    }
}