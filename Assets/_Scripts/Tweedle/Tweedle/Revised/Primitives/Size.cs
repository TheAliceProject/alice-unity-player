using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Tweedle.Primitives
{
    [PInteropType]
    public sealed class Size
    {
        public readonly Vector3 value;

        [PInteropField]
        public double x { get {return value.X; } }
        [PInteropField]
        public double y { get {return value.Y; } }
        [PInteropField]
        public double z { get {return value.Z; } }

        public Size(Vector3 inVector)
        {
            value = inVector;
        }

        [PInteropConstructor]
        public Size(double width, double height, double depth)
        {
            value = new Vector3(width, height, depth);
        }

        [PInteropConstructor]
        public Size(Size clone)
        {
            value = clone.value;
        }

        [PInteropMethod]
        public double getWidth() {
	        return value.X;
	    }

        [PInteropMethod]
        public double getHeight() {
            return value.Y;
        }

        [PInteropMethod]
        public double getDepth() {
            return value.Z;
        }

        public static Size lerp(Size a, Size b, Portion t) {
            return new Size(Vector3.Lerp(a.value, b.value, t.value));
        }
        
        static public implicit operator UnityEngine.Vector3(Size inPosition)
        {
            return inPosition != null ? new UnityEngine.Vector3((float)inPosition.value.X, (float)inPosition.value.Y, (float)inPosition.value.Z) : new UnityEngine.Vector3(float.NaN, float.NaN, float.NaN);
        }

    }
}